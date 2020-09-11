using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Core.OrderManagement.Tests.Helpers
{
    public class AggregateTester<TRepository>
    {
        private readonly TestEventStore _eventStore;
        private readonly Func<IEventStore, TRepository> _repositoryFactory;

        public AggregateTester(Func<IEventStore, TRepository> repositoryFactory)
        {
            _eventStore = new TestEventStore();
            _repositoryFactory = repositoryFactory;
        }


        public void Run(
            LoadedEvent[] given, 
            Func<WhenContext, IAggregateLoader> when,
            Action<ThenContext> then)
        {
            _eventStore.AddTestEvents("1", given);

            var repository = _repositoryFactory(_eventStore);

            var whenContext = new WhenContext(repository);

            var aggregateLoader =  when(whenContext);
            
            var uncommittedChanges = aggregateLoader.GetUncommittedChanges().ToList();

            var thenContext = new ThenContext(uncommittedChanges);

            then(thenContext);
        }

        public class WhenContext
        {
            public TRepository Repository { get; }

            public WhenContext(TRepository repository)
            {
                Repository = repository;
            }
        }

        public class ThenContext
        {
            public List<LoadedEvent> Events { get; }

            public ThenContext(List<LoadedEvent> events)
            {
                Events = events;
            }
        }

    }
}