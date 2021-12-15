namespace Pudicitia.Common.Events;

public class EventPublished
{
    private Event? _event;

    private EventPublished()
    {
    }

    public EventPublished(Event @event)
    {
        var eventType = @event.GetType();

        EventId = @event.Id;
        EventTypeNamespace = eventType.Namespace!;
        EventTypeName = eventType.Name;
        EventContent = @event.ToJson();
        CreatedOn = @event.CreatedOn;
        Event = @event;
    }

    public Guid EventId { get; private init; }

    public string EventTypeNamespace { get; private init; } = string.Empty;

    public string EventTypeName { get; private init; } = string.Empty;

    public string EventContent { get; private init; } = string.Empty;

    public PublishState PublishState { get; private set; } = PublishState.Waiting;

    public int PublishCount { get; private set; }

    public DateTime CreatedOn { get; private init; }

    public Event Event
    {
        get
        {
            if (_event is null)
            {
                var type = TypeUtility.GetType($"{EventTypeNamespace}.{EventTypeName}")!;
                _event = EventContent.ToObject(type) as Event;
            }

            return _event!;
        }
        set => _event = value;
    }

    public void Publish()
    {
        switch (PublishState)
        {
            case PublishState.Waiting:
            case PublishState.Failed:
                PublishState = PublishState.InProgress;
                PublishCount++;
                break;

            case PublishState.InProgress:
                PublishCount++;
                break;

            default:
                return;
        }
    }

    public void Complete()
    {
        if (PublishState == PublishState.InProgress)
        {
            PublishState = PublishState.Completed;
        }
    }

    public void Fail()
    {
        if (PublishState != PublishState.Completed)
        {
            PublishState = PublishState.Failed;
        }
    }
}
