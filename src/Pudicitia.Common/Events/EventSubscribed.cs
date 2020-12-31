using System;
using Pudicitia.Common.Extensions;

namespace Pudicitia.Common.Events
{
    public class EventSubscribed
    {
        private Event @event;

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

        public string EventTypeName { get; private init; }

        public string EventContent { get; private init; }

        public DateTime CreatedOn { get; private init; } = DateTime.UtcNow;

        public Event Event
        {
            get
            {
                if (@event is null)
                    @event = EventContent.ToObject(Type.GetType(EventTypeName)) as Event;

                return @event;
            }
            set
            {
                @event = value;
            }
        }
    }
}