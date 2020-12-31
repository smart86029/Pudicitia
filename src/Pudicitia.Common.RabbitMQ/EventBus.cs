using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.RabbitMQ
{
    public class EventBus : Events.IEventBus, IDisposable
    {
        private readonly IAdvancedBus advancedBus;
        private readonly IExchange exchange;
        private readonly IQueue queue;
        private readonly Dictionary<string, Subscription> subscriptions = new Dictionary<string, Subscription>();

        public EventBus(IServiceProvider serviceProvider, IOptions<RabbitMQOptions> options)
        {
            advancedBus = RabbitHutch.CreateBus(options.Value.ConnectionString).Advanced;
            exchange = advancedBus.ExchangeDeclare("pudicitia", ExchangeType.Topic);
            queue = advancedBus.QueueDeclare(options.Value.QueueName);

            advancedBus.Consume(queue, async (byte[] body, MessageProperties properties, MessageReceivedInfo info) =>
            {
                if (subscriptions.TryGetValue(info.RoutingKey, out var subscription))
                {
                    var @event = Encoding.UTF8.GetString(body).ToObject(subscription.EventType);
                    using var scope = serviceProvider.CreateScope();
                    var eventHandler = scope.ServiceProvider.GetService(subscription.EventHandlerType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(subscription.EventType);
                    if (eventHandler is null)
                        return;

                    await Task.Yield();
                    var repository = scope.ServiceProvider.GetService(typeof(IEventSubscribedRepository)) as IEventSubscribedRepository;
                    repository.Add(new EventSubscribed(@event as Event));
                    await (Task)concreteType.GetMethod("HandleAsync").Invoke(eventHandler, new[] { @event });
                }
            });
        }

        public void Dispose()
        {
            advancedBus.Dispose();
        }

        public async Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : Event
        {
            var routingKey = @event.GetType().Name;
            var properties = new MessageProperties();
            var body = Encoding.UTF8.GetBytes(@event.ToJson());

            await advancedBus.PublishAsync(exchange, routingKey, false, properties, body);
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>
        {
            Subscribe(typeof(TEvent), typeof(TEventHandler));
        }

        public void Subscribe(Type eventType, Type eventHandlerType)
        {
            var routingKey = eventType.Name;

            advancedBus.Bind(exchange, queue, routingKey);
            subscriptions.Add(routingKey, new Subscription(eventType, eventHandlerType));
        }
    }
}