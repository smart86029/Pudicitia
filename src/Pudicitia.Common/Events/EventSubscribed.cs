using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pudicitia.Common.Events
{
    public class EventSubscribed
    {
        //private Event @event;

        //private EventSubscribed()
        //{
        //}

        //public EventSubscribed(Event @event)
        //{
        //    var eventType = @event.GetType();

        //    EventId = @event.Id;
        //    EventTypeNamespace = eventType.Namespace;
        //    EventTypeName = eventType.Name;
        //    EventContent = @event.ToJson();
        //    CreatedOn = @event.CreatedOn;
        //    Event = @event;
        //}

        //public Guid EventId { get; private set; }

        //public DateTime CreatedOn { get; private set; }

        //public Event Event
        //{
        //    get
        //    {
        //        if (@event == default)
        //            @event = EventContent.ToObject(Type.GetType(EventTypeName)) as Event;

        //        return @event;
        //    }
        //    set
        //    {
        //        @event = value;
        //    }
        //}
    }
}
