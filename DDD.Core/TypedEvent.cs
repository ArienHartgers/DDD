using System;

namespace DDD.Core
{
    public class TypedEvent<TEvent>
        where TEvent: Event
    {
        public TypedEvent(DateTimeOffset eventDateTime, TEvent @event)
        {
            EventDateTime = eventDateTime;
            Event = @event;
        }

        public DateTimeOffset EventDateTime { get; }

        public TEvent Event { get; }

        public override string ToString()
        {
            return $"Event: {typeof(TEvent).Name} Time: {EventDateTime}";
        }
    }
}