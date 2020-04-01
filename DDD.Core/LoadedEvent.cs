using System;

namespace DDD.Core
{
    public class LoadedEvent
    {
        public LoadedEvent(DateTimeOffset eventDateTime, object data)
        {
            EventDateTime = eventDateTime;
            Data = data;
        }

        public DateTimeOffset EventDateTime { get; }

        public object Data { get; }

        public override string ToString()
        {
            return $"Event: {Data.GetType().Name} Time: {EventDateTime}";
        }
    }
}