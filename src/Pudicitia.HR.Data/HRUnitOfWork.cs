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

        public HRUnitOfWork(/*HRContext context, IEventBus eventBus*/)
        {
            //this.context = context ?? throw new ArgumentNullException(nameof(context));
            //this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<bool> CommitAsync()
        {
            //var entities = context.ChangeTracker
            //    .Entries<Entity>()
            //    .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            //    .ToList();
            //var events = entities.SelectMany(x => x.Entity.DomainEvents).ToList();
            //var eventLogs = events.Select(e => new EventLog(e)).ToList();

            //context.Set<EventLog>().AddRange(eventLogs);
            //foreach (var entity in entities)
            //    entity.Entity.AcceptChanges();

            //await context.SaveChangesAsync();
            //await PublishEventsAsync(eventLogs);

            return true;
        }
    }
}
