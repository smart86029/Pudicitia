using System.Text;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.RabbitMQ;

public class EventBus : Events.IEventBus, IDisposable
{
    private readonly IAdvancedBus _advancedBus;
    private readonly IExchange _exchange;
    private readonly IQueue _queue;
    private readonly Dictionary<string, Subscription> _subscriptions = new();

    public EventBus(IServiceProvider serviceProvider, IOptions<RabbitMQOptions> options)
    {
        _advancedBus = RabbitHutch.CreateBus(options.Value.ConnectionString).Advanced;
        _exchange = _advancedBus.ExchangeDeclare("pudicitia", ExchangeType.Topic);
        _queue = _advancedBus.QueueDeclare(options.Value.QueueName);

        _advancedBus.Consume(_queue, async (byte[] body, MessageProperties properties, MessageReceivedInfo info) =>
        {
            if (!_subscriptions.TryGetValue(info.RoutingKey, out var subscription))
            {
                return;
            }

            var @event = Encoding.UTF8.GetString(body).ToObject(subscription.EventType);
            using var scope = serviceProvider.CreateScope();
            var eventHandler = scope.ServiceProvider.GetService(subscription.EventHandlerType);
            var concreteType = typeof(IEventHandler<>).MakeGenericType(subscription.EventType);
            if (@event is null || eventHandler is null)
            {
                return;
            }

            await Task.Yield();
            if (scope.ServiceProvider.GetService(typeof(IEventSubscribedRepository)) is not IEventSubscribedRepository repository)
            {
                return;
            }

            repository.Add(new EventSubscribed((@event as Event)!));
            await (Task)concreteType.GetMethod("HandleAsync")?.Invoke(eventHandler, new[] { @event })!;
        });
    }

    public void Dispose()
    {
        _advancedBus.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task PublishAsync<TEvent>(TEvent @event)
        where TEvent : Event
    {
        var routingKey = @event.GetType().Name;
        var properties = new MessageProperties();
        var body = Encoding.UTF8.GetBytes(@event.ToJson());

        await _advancedBus.PublishAsync(_exchange, routingKey, false, properties, body);
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

        _advancedBus.Bind(_exchange, _queue, routingKey);
        _subscriptions.Add(routingKey, new Subscription(eventType, eventHandlerType));
    }
}
