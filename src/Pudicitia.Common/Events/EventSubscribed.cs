namespace Pudicitia.Common.Events;

public class EventSubscribed
{
    private Event? _event;

    private EventSubscribed()
    {
    }

    public EventSubscribed(Event @event)
    {
        var eventType = @event.GetType();

        EventId = @event.Id;
        EventTypeName = eventType.Name;
        EventContent = @event.ToJson();
        Event = @event;
    }

    public Guid EventId { get; private init; }

    public string EventTypeName { get; private init; } = string.Empty;

    public string EventContent { get; private init; } = string.Empty;

    public DateTime CreatedOn { get; private init; } = DateTime.UtcNow;

    public Event Event
    {
        get
        {
            if (_event is null)
            {
                var type = TypeUtility.GetType(EventTypeName)!;
                _event = EventContent.ToObject(type) as Event;
            }

            return _event!;
        }
        set => _event = value;
    }
}
