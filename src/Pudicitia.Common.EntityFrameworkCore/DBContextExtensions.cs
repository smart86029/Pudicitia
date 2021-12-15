using Pudicitia.Common.Domain;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore;

public static class DBContextExtensions
{
    public static ICollection<EventPublished> LogEvents(this DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<Entity>()
            .Select(x => x.Entity)
            .Where(x => x.DomainEvents.Any())
            .ToList();
        var eventLogs = entities
            .SelectMany(x => x.DomainEvents)
            .Select(x => new EventPublished(x))
            .ToList();

        context.Set<EventPublished>().AddRange(eventLogs);
        foreach (var entity in entities)
            entity.ClearEvents();

        return eventLogs;
    }
}
