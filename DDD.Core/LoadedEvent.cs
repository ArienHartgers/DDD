using System;

namespace DDD.Core
{
    public class LoadedEvent : IEventContext
    {
        public LoadedEvent(DateTimeOffset eventDateTime, Event data)
        {
            EventDateTime = eventDateTime;
            Data = data;
        }

        public DateTimeOffset EventDateTime { get; }

        public Event Data { get; }

        public override string ToString()
        {
            return $"Event: {Data.GetType().Name} Time: {EventDateTime}";
        }
    }
}