using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DDD.Core
{
    public class Repository<TAggregateRoot, TIdentifier>
        where TIdentifier : IIdentifier
        where TAggregateRoot : AggregateRoot<TIdentifier>, IAggregateLoader
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore, IAggregateContext aggregateContext)
        {
            AggregateContext = aggregateContext;
            _eventStore = eventStore;
        }

        public IAggregateContext AggregateContext { get; }

        public async Task<TAggregateRoot> GetAsync(IIdentifier identifier)
        {
            var agg = await FindAsync(identifier);
            if (agg == null)
            {
                throw new Exception($"{typeof(TAggregateRoot).Name} with identifier '{identifier}' not found");
            }

            return agg;
        }

        public async Task<TAggregateRoot?> FindAsync(IIdentifier identifier)
        {
            var streamName = GetStreamName(identifier);
            var eventsResult = await _eventStore.GetStreamEventsAsync(streamName);

            var firstLoadedEvent = eventsResult.Events.FirstOrDefault();
            if (firstLoadedEvent != null)
            {
                TAggregateRoot aggregate = AggregateFactory.CreateAggregateRoot<TAggregateRoot, TIdentifier>(firstLoadedEvent, firstLoadedEvent.Data);
                IAggregateLoader loader = aggregate;
                loader.SetAggregateContext(AggregateContext);
                loader.LoadFromHistory(eventsResult.Version, eventsResult.Events.Skip(1));
                return aggregate;
            }

            return null;
        }

        public async Task SaveAsync(TAggregateRoot aggregate)
        {
            IAggregateLoader loader = aggregate;
            var changes = loader.GetUncommittedChanges();
            if (changes.Any())
            {
                var streamName = GetStreamName(aggregate.GetIdentifier());
                await _eventStore.SaveEventsAsync(streamName, aggregate.Version, changes);
                loader.MarkChangesAsCommitted();
            }
        }

        protected virtual string GetStreamName(IIdentifier identifier)
        {
            return identifier.Identifier;
        }
    }
}