using System;
using System.Collections.Generic;

namespace DDD.Core
{
    public abstract class AggregateRoot<TIdentity> : Entity<TIdentity>, IAggregateLoader
        where TIdentity : class
    {

        private readonly List<LoadedEvent> _changes = new List<LoadedEvent>();

        public int Version { get; internal set; }

        IEnumerable<LoadedEvent> IAggregateLoader.GetUncommittedChanges()
        {
            return _changes;
        }

        void IAggregateLoader.MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        void IAggregateLoader.LoadFromHistory(IEnumerable<LoadedEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        public void ApplyChange(Event @event)
        {
            ApplyChange(
                new LoadedEvent(DateTimeOffset.Now, @event),
                true);
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(LoadedEvent @event, bool isNew)
        {
            IEventApplier applier = this;
            applier.ProcessMessage(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}