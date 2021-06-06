using System;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Core
{
    public abstract class Repository<TAggregateRoot, TIdentifier>
        where TIdentifier : IIdentifier
        where TAggregateRoot : AggregateRoot, IAggregateLoader
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore, IAggregateContext aggregateContext)
        {
            AggregateContext = aggregateContext;
            _eventStore = eventStore;
        }

        public IAggregateContext AggregateContext { get; }

        public async Task<TAggregateRoot> GetAsync(TIdentifier identifier)
        {
            var agg = await FindAsync(identifier);
            if (agg == null)
            {
                throw new Exception($"{typeof(TAggregateRoot).Name} with identifier '{identifier}' not found");
            }

            return agg;
        }

        public async Task<TAggregateRoot?> FindAsync(TIdentifier identifier)
        {
            var streamName = GetStreamName(identifier);
            var eventsResult = await _eventStore.GetStreamEventsAsync(streamName);

            var firstLoadedEvent = eventsResult.Events.FirstOrDefault();
            if (firstLoadedEvent != null)
            {
                TAggregateRoot aggregate = AggregateFactory.CreateAggregateRoot<TAggregateRoot>(firstLoadedEvent, firstLoadedEvent.Data);
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
                var identifier = aggregate.GetIdentifier();
                var streamName = GetStreamName(identifier);
                await _eventStore.SaveEventsAsync(streamName, aggregate.Version, changes);
                loader.MarkChangesAsCommitted();
            }
        }

        protected abstract string GetStreamName(IIdentifier identifier);
    }
}