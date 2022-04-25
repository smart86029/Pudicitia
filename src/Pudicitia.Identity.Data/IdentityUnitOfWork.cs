using Pudicitia.Common.Events;
using Pudicitia.Identity.Domain;

namespace Pudicitia.Identity.Data;

public class IdentityUnitOfWork : IIdentityUnitOfWork
{
    private readonly IdentityContext _context;
    private readonly IEventBus _eventBus;

    public IdentityUnitOfWork(IdentityContext context, IEventBus eventBus)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async Task<bool> CommitAsync()
    {
        var eventLogs = _context.LogEvents();
        await _context.SaveChangesAsync();
        await PublishEventsAsync(eventLogs);

        return true;
    }

    private async Task PublishEventsAsync(IEnumerable<EventPublished> eventLogs)
    {
        var tasks = eventLogs.Select(async eventLog =>
        {
            eventLog.Publish();
            await _context.SaveChangesAsync();

            _eventBus.Publish(eventLog.Event);

            eventLog.Complete();
            await _context.SaveChangesAsync();
        });

        await Task.WhenAll(tasks);
    }
}
