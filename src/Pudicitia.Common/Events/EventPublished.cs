using System;
using Pudicitia.Common.Extensions;
using Pudicitia.Common.Utilities;

namespace Pudicitia.Common.Events
{
    public class EventPublished
    {
        private Event @event;

        private EventPublished()
        {
        }

        public EventPublished(Event @event)
        {
            var eventType = @event.GetType();

            EventId = @event.Id;
            EventTypeNamespace = eventType.Namespace;
            EventTypeName = eventType.Name;
            EventContent = @event.ToJson();
            CreatedOn = @event.CreatedOn;
            Event = @event;
        }

        public Guid EventId { get; private init; }

        public string EventTypeNamespace { get; private init; }

        public string EventTypeName { get; private init; }

        public string EventContent { get; private init; }

        public PublishState PublishState { get; private set; } = PublishState.Waiting;

        public int PublishCount { get; private set; }

        public DateTime CreatedOn { get; private init; }

        public Event Event
        {
            get
            {
                if (@event is null)
                {
                    var type = TypeUtility.GetType($"{EventTypeNamespace}.{EventTypeName}");
                    @event = EventContent.ToObject(type) as Event;
                }

                return @event;
            }
            set
            {
                @event = value;
            }
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
                PublishState = PublishState.Completed;
        }

        public void Fail()
        {
            if (PublishState != PublishState.Completed)
                PublishState = PublishState.Failed;
        }
    }
}