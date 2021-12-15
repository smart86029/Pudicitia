namespace Pudicitia.Common.Events;

public abstract class Event
{
    public Guid Id { get; private init; } = GuidUtility.NewGuid();

    public DateTime CreatedOn { get; private init; } = DateTime.UtcNow;
}
