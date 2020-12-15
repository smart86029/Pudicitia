using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore
{
    public static class DBContextExtensions
    {
        public static ICollection<EventLog> LogEvents(this DbContext context)
        {
            var entities = context.ChangeTracker
                .Entries<Entity>()
                .Select(x => x.Entity)
                .Where(x => x.DomainEvents.Any())
                .ToList();
            var eventLogs = entities
                .SelectMany(x => x.DomainEvents)
                .Select(x => new EventLog(x))
                .ToList();

            context.Set<EventLog>().AddRange(eventLogs);
            foreach (var entity in entities)
                entity.ClearEvents();

            return eventLogs;
        }
    }
}