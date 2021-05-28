using System;

namespace DDD.Core
{
    public class HandlerEvent<TEvent>  : LoadedEvent
        where TEvent : Event
    {
        public HandlerEvent(DateTimeOffset eventDateTime, TEvent data)
            : base(eventDateTime, data)
        {
            Event = data;
        }

        public TEvent Event { get; }

        public void ForwardTo(IEventApplier applier)
        {
            applier.ProcessMessage(this);
        }
    }
}