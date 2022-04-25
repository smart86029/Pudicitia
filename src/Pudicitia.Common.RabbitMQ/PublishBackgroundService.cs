using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pudicitia.Common.Events;
using Pudicitia.Common.Extensions;
using RabbitMQ.Client;

namespace Pudicitia.Common.RabbitMQ;

internal class PublishBackgroundService : BackgroundService
{
    private readonly string _exchangeName;
    private readonly EventBus _eventBus;
    private readonly BlockingCollection<Event> _events;
    private readonly IModel _channel;

    public PublishBackgroundService(IOptions<RabbitMQOptions> options, IEventBus eventBus)
    {
        _exchangeName = options.Value.ExchangeName;
        _eventBus = (EventBus)eventBus;
        _events = _eventBus.Events;
        _channel = _eventBus.Connection.CreateModel();

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic, durable: true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && !_events.IsCompleted)
        {
            if (!_events.TryTake(out var @event))
            {
                await Task.Yield();
                continue;
            }

            var routingKey = @event.GetType().Name;
            var properties = _channel.CreateBasicProperties();
            var body = @event.ToUtf8Bytes();

            _channel.BasicPublish(_exchangeName, routingKey, false, properties, body);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Dispose();

        await base.StopAsync(cancellationToken);
    }
}
