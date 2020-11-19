using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Events;
using Pudicitia.HR.Domain;

namespace Pudicitia.HR.Data
{
    public class HRUnitOfWork : IHRUnitOfWork
    {
        private readonly HRContext context;
        private readonly IEventBus eventBus;

        public HRUnitOfWork(HRContext context, IEventBus eventBus)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<bool> CommitAsync()
        {
            var entities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .ToList();
            var eventLogs = entities
                .SelectMany(x => x.Entity.DomainEvents)
                .Select(x => new EventLog(x))
                .ToList();

            context.Set<EventLog>().AddRange(eventLogs);
            foreach (var entity in entities)
                entity.Entity.AcceptChanges();

            await context.SaveChangesAsync();
            await PublishEventsAsync(eventLogs);

            return true;
        }

        private async Task PublishEventsAsync(IEnumerable<EventLog> eventLogs)
        {
            var tasks = eventLogs.Select(async eventLog =>
            {
                eventLog.Publish();
                await context.SaveChangesAsync();

                await eventBus.PublishAsync(eventLog.Event);

                eventLog.Complete();
                await context.SaveChangesAsync();
            });

            await Task.WhenAll(tasks);
        }
    }
}
