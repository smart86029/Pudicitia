using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pudicitia.Common.Events;

internal class EventBusBackgroundService : BackgroundService
{
    private const string ServiceName = nameof(EventBusBackgroundService);

    private readonly ILogger<EventBusBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventBus _eventBus;

    public EventBusBackgroundService(
        ILogger<EventBusBackgroundService> logger,
        IServiceProvider serviceProvider,
        IEventBus eventBus)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        _logger.LogInformation($"{ServiceName} has started.");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await ProcessEventPublishedsAsync();
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"{ServiceName} execute failed.");
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
    }

    private async Task ProcessEventPublishedsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IEventPublishedRepository>();
        var eventPublisheds = await repository.GetUnpublishedEventsAsync();
        var tasks = eventPublisheds.Select(x => ProcessEventPublishedAsync(x));

        await Task.WhenAll(tasks);
    }

    private async Task ProcessEventPublishedAsync(EventPublished eventPublished)
    {
        using var scope = _serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IEventPublishedRepository>();

        eventPublished.Publish();
        await repository.UpdateAndCommit(eventPublished);

        await _eventBus.PublishAsync(eventPublished.Event);

        eventPublished.Complete();
        await repository.UpdateAndCommit(eventPublished);
    }
}
