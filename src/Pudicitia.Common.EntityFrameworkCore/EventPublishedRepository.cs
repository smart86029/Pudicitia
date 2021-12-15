using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore;

public class EventPublishedRepository<TContext> : IEventPublishedRepository
    where TContext : DbContext
{
    private readonly TContext context;

    public EventPublishedRepository(TContext context)
    {
        this.context = context;
    }

    public async Task<ICollection<EventPublished>> GetUnpublishedEventsAsync()
    {
        var result = await context
            .Set<EventPublished>()
            .Where(x => x.PublishState != PublishState.Completed)
            .ToListAsync();

        return result;
    }

    public async Task UpdateAndCommit(EventPublished eventPublished)
    {
        context.Set<EventPublished>().Update(eventPublished);
        await context.SaveChangesAsync();
    }
}
