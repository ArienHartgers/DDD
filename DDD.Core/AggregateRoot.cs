using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDD.Core
{
    public abstract class AggregateRoot<TIdentifier> : EntityBase<TIdentifier>, IAggregateLoader, IEntityModifier
    {
        private static bool _isChecked = false;
        private readonly List<LoadedEvent> _changes = new List<LoadedEvent>();
        private bool _isRemoved = false;


        public int Version { get; internal set; }

        protected AggregateRoot()
        {
            if (!_isChecked)
            {
                _isChecked = true;
                AssureReadonlyProperties();
            }
        }

        IReadOnlyCollection<LoadedEvent> IAggregateLoader.GetUncommittedChanges()
        {
            return _changes;
        }

        void IAggregateLoader.MarkChangesAsCommitted()
        {
            Version += _changes.Count;
            _changes.Clear();
        }

        void IAggregateLoader.LoadFromHistory(int version, IEnumerable<LoadedEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
            Version = version;
        }

        internal void ApplyInitialEvent(LoadedEvent @event)
        {
            if (_changes.Any())
            {
                throw new Exception("ApplyInitialEvent can only be used for a new aggregate");
            }

            _changes.Add(@event);
        }

        protected void ApplyChange(Event @event)
        {
            if (_isRemoved)
            {
                throw new Exception("Aggregate is marked as removed");
            }

            ApplyChange(
                new LoadedEvent(DateTimeOffset.Now, @event),
                true);
        }

        void IEntityModifier.ApplyChange(Event @event)
        {
            ApplyChange(@event);
        }

        void IEntityModifier.MarkAsRemoved()
        {
            _isRemoved = true;
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(LoadedEvent @event, bool isNew)
        {
            IEventApplier applier = this;
            applier.ProcessMessage(@event);
            if (isNew)
            {
                _changes.Add(@event);
            }
        }

        private void AssureReadonlyProperties()
        {
            var properties = GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.CanWrite
                    && !propertyInfo.SetMethod.IsPrivate
                    && !(propertyInfo.DeclaringType?.Name.StartsWith("AggregateRoot") ?? false))
                {
                    throw new Exception($"Aggregate '{GetType().Name}' may only have readonly properties. Property '{propertyInfo.Name}' can be public written");
                }
            }
        }

        protected static TAggregateRoot CreateWithEvent<TAggregateRoot, TEvent>(DateTimeOffset dateTimeOffset, TEvent firstEvent)
            where TAggregateRoot : AggregateRoot<TIdentifier>
            where TEvent : Event
        {
            var aggregateRoot = CallAggregateConstructor<TAggregateRoot, TEvent>(new TypedEvent<TEvent>(dateTimeOffset, firstEvent));
            aggregateRoot.ApplyInitialEvent(new LoadedEvent(dateTimeOffset, firstEvent));
            return aggregateRoot;
        }

        private static TAggregateRoot CallAggregateConstructor<TAggregateRoot, TEvent>(TypedEvent<TEvent> typedEvent)
                   where TAggregateRoot : AggregateRoot<TIdentifier>
                   where TEvent : Event
        {

            var constructors = typeof(TAggregateRoot).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (constructors.Length == 1)
            {
                var constructor = constructors[0];
                if (constructor.IsPrivate)
                {

                    var paramTypes = constructor.GetParameters();
                    if (paramTypes.Length == 1 && paramTypes[0].ParameterType == typedEvent.GetType())
                    {
                        return (TAggregateRoot)constructor.Invoke(new object[] { typedEvent });
                    }
                }
            }

            throw new Exception($"Aggregate {typeof(TAggregateRoot).Name} must have 1 private constructor(TypedEvent<{typeof(TEvent).Name}> initialEvent)");
        }

    }
}