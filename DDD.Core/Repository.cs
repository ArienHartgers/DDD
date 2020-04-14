using System;
using System.Linq;
using System.Reflection;

namespace DDD.Core
{
    public class Repository<TAggregateRoot, TIdentity>
        where TIdentity : IIdentity
        where TAggregateRoot : AggregateRoot<TIdentity>, IAggregateLoader
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public TAggregateRoot Get(IIdentity identity)
        {
            var agg = Find(identity);
            if (agg == null)
            {
                throw new Exception($"{typeof(TAggregateRoot).Name} with identity '{identity}' not found");
            }

            return agg;
        }

        public TAggregateRoot? Find(IIdentity identity)
        {
            var eventsResult = _eventStore.GetStreamEvents(identity.Identity);
            if (eventsResult.Events.Any())
            {
                var constructors =
                    typeof(TAggregateRoot).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                var constructor = constructors.SingleOrDefault();
                if (constructor == null)
                {
                    throw new Exception(
                        $"Cannot find single private constructor for class {typeof(TAggregateRoot).FullName}");
                }

                var aggregate = (TAggregateRoot) constructor.Invoke(new object[0]);

                IAggregateLoader loader = aggregate;
                loader.LoadFromHistory(eventsResult.Version, eventsResult.Events);
                return aggregate;
            }

            return null;
        }

        public void Save(TAggregateRoot aggregate)
        {
            IAggregateLoader loader = aggregate;
            var changes = loader.GetUncommittedChanges();
            var streamName = aggregate.Identity.ToString();
            _eventStore.SaveEvents(streamName, aggregate.Version,changes.ToArray());
            loader.MarkChangesAsCommitted();
        }
    }
}