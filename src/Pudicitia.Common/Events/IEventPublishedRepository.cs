namespace Pudicitia.Common.Events;

public interface IEventPublishedRepository
{
    Task<ICollection<EventPublished>> GetUnpublishedEventsAsync();

    Task UpdateAndCommit(EventPublished eventPublished);
}
