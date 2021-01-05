using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pudicitia.Common.Events
{
    internal class EventBusBackgroundService : BackgroundService
    {
        private const string ServiceName = nameof(EventBusBackgroundService);

        private readonly ILogger<EventBusBackgroundService> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IEventBus eventBus;

        public EventBusBackgroundService(
            ILogger<EventBusBackgroundService> logger,
            IServiceProvider serviceProvider,
            IEventBus eventBus)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            logger.LogInformation($"{ServiceName} has started.");

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
                    logger.LogWarning(ex, $"{ServiceName} execute failed.");
                    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
                }
            }
        }

        private async Task ProcessEventPublishedsAsync()
        {
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IEventPublishedRepository>();
            var eventPublisheds = await repository.GetUnpublishedEventsAsync();
            var tasks = eventPublisheds.Select(x => ProcessEventPublishedAsync(x));

            await Task.WhenAll(tasks);
        }

        private async Task ProcessEventPublishedAsync(EventPublished eventPublished)
        {
            using var scope = serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IEventPublishedRepository>();

            eventPublished.Publish();
            await repository.UpdateAndCommit(eventPublished);

            await eventBus.PublishAsync(eventPublished.Event);

            eventPublished.Complete();
            await repository.UpdateAndCommit(eventPublished);
        }
    }
}