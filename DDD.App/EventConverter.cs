using System;
using DDD.App.Events;
using DDD.Core;

namespace DDD.App
{
    public abstract class EventConverter
    {
        public abstract IDomainEvent Convert(Event intEvent);
        public abstract Event Convert(IDomainEvent extEvent);

        public abstract Type ExternalType { get; }
        public abstract Type InternalType { get; }
    }

    public abstract class EventConverter<TInternalEvent, TExternalEvent> : EventConverter
        where TInternalEvent : Event
        where TExternalEvent : IDomainEvent
    {
        public override Type ExternalType => typeof(TExternalEvent);
        public override Type InternalType => typeof(TInternalEvent);

        public override IDomainEvent Convert(Event intEvent)
        {
            return ConvertToExtern((TInternalEvent)intEvent);
        }

        public override Event Convert(IDomainEvent extEvent)
        {
            return ConvertToIntern((TExternalEvent)extEvent);
        }

        public abstract TExternalEvent ConvertToExtern(TInternalEvent intEvent);
        public abstract TInternalEvent ConvertToIntern(TExternalEvent extEvent);
    }

}