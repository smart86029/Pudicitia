using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Pudicitia.Common.RabbitMQ;

public class EventBus : IEventBus, IDisposable
{
    private const int ConcurrentCount = 100;

    private readonly IServiceProvider _serviceProvider;
    private readonly string _clientName;
    private readonly string _exchangeName;
    private readonly HashSet<string> _queueNames = new();
    private readonly Dictionary<string, Subscription> _subscriptions = new();
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly AsyncEventingBasicConsumer _consumer;
    private readonly SemaphoreSlim _semaphoreSlim = new(ConcurrentCount);

    public EventBus(IServiceProvider serviceProvider, IOptions<RabbitMQOptions> options)
    {
        _serviceProvider = serviceProvider;
        _clientName = options.Value.ClientName;
        _exchangeName = options.Value.ExchangeName;

        var factory = new ConnectionFactory
        {
            Uri = options.Value.Uri,
            DispatchConsumersAsync = true,
        };
        _connection = factory.CreateConnection(_clientName);
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, durable: true);

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.Received += async (sender, e) =>
        {
            if (!_subscriptions.TryGetValue(e.RoutingKey, out var subscription))
            {
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }

            var @event = e.Body.Span.ToObject(subscription.EventType);
            await _semaphoreSlim.WaitAsync();

            _ = Task.Run(async () =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
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
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            });

            _channel.BasicAck(e.DeliveryTag, false);
        };
    }

    internal BlockingCollection<Event> Events { get; private init; } = new();

    internal IConnection Connection => _connection;

    public void Publish<TEvent>(TEvent @event)
        where TEvent : Event
    {
        Events.TryAdd(@event);
    }

    public void Subscribe<TEvent, TEventHandler>()
        where TEvent : Event
        where TEventHandler : IEventHandler<TEvent>
    {
        Subscribe(typeof(TEvent), typeof(TEventHandler));
    }

    public void Subscribe(Type eventType, Type eventHandlerType)
    {
        var @namespace = eventType.Namespace?.Split('.').LastOrDefault() ?? string.Empty;
        var queueName = $"{_clientName}.{@namespace}";
        var routingKey = eventType.Name;

        if (!_queueNames.Contains(queueName))
        {
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            _channel.BasicQos(0, ConcurrentCount, false);
            _channel.BasicConsume(queueName, false, _consumer);
            _queueNames.Add(queueName);
        }

        _channel.QueueBind(queueName, _exchangeName, routingKey);
        _subscriptions.Add(routingKey, new Subscription(eventType, eventHandlerType));
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
