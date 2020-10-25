using System;
using System.Linq;
using System.Reflection;

namespace DDD.Core
{
    public abstract class Repository<TAggregateRoot, TIdentifier>
        where TIdentifier : IIdentifier
        where TAggregateRoot : AggregateRoot<TIdentifier>, IAggregateLoader
    {
        private readonly IEventStore _eventStore;

        protected Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }


        //protected TAggregateRoot Create(Event @event)
        //{
        //    var loadedEvent = new LoadedEvent(DateTimeOffset.Now, @event);
        //    TAggregateRoot aggregate = CreateInternal(loadedEvent);

        //    aggregate.ApplyInitialEvent(loadedEvent);

        //    return aggregate;
        //}

        public TAggregateRoot Get(IIdentifier identifier)
        {
            var agg = Find(identifier);
            if (agg == null)
            {
                throw new Exception($"{typeof(TAggregateRoot).Name} with identifier '{identifier}' not found");
            }

            return agg;
        }

        public TAggregateRoot? Find(IIdentifier identifier)
        {
            var eventsResult = _eventStore.GetStreamEvents(identifier.Identifier);

            var firstLoadedEvent = eventsResult.Events.FirstOrDefault();
            if (firstLoadedEvent != null)
            {
                TAggregateRoot aggregate = CreateInternal(firstLoadedEvent);

                IAggregateLoader loader = aggregate;
                loader.LoadFromHistory(eventsResult.Version, eventsResult.Events.Skip(1));
                return aggregate;
            }

            return null;
        }

        public void Save(TAggregateRoot aggregate)
        {
            IAggregateLoader loader = aggregate;
            var changes = loader.GetUncommittedChanges();
            if (changes.Any())
            {
                var streamName = aggregate.GetIdentifier().ToString();
                _eventStore.SaveEvents(streamName, aggregate.Version, changes);
                loader.MarkChangesAsCommitted();
            }
        }

        private TAggregateRoot CreateInternal(LoadedEvent firstLoadedEvent)
        {
            var eventType = firstLoadedEvent.Data.GetType();
            var typedEventType = typeof(TypedEvent<>).MakeGenericType(eventType);
            var typedEvent = typedEventType.GetConstructors().First().Invoke(new object[] { firstLoadedEvent.EventDateTime, firstLoadedEvent.Data });

            var constructors = typeof(TAggregateRoot).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (constructors.Length == 1)
            {
                var constructor = constructors[0];
                if (constructor.IsPrivate)
                {

                    var paramTypes = constructor.GetParameters();
                    if (paramTypes.Length == 1 && paramTypes[0].ParameterType == typedEventType)
                    {
                        return (TAggregateRoot) constructor.Invoke(new object[] { typedEvent });
                    }
                }
            }

            throw new Exception($"Aggregate {typeof(TAggregateRoot).Name} must have 1 private constructor(TypedEvent<{eventType.Name}> initialEvent)");
        }

    }
}