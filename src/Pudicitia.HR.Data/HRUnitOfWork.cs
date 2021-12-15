using Pudicitia.Common.Events;
using Pudicitia.HR.Domain;

namespace Pudicitia.HR.Data;

public class HRUnitOfWork : IHRUnitOfWork
{
    private readonly HRContext _context;
    private readonly IEventBus _eventBus;

    public HRUnitOfWork(HRContext context, IEventBus eventBus)
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

            await _eventBus.PublishAsync(eventLog.Event);

            eventLog.Complete();
            await _context.SaveChangesAsync();
        });

        await Task.WhenAll(tasks);
    }
}
