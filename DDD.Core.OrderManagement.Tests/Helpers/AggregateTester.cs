using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDD.Core.OrderManagement.Tests.Helpers
{
    [DebuggerStepThrough]
    public class AggregateTester<TAggregate, TIdentifier> : IDisposable
    where TIdentifier : IIdentifier
    where TAggregate : AggregateRoot, IAggregateLoader
    {
        private readonly TestEventStore _eventStore;

        private readonly Repository<TAggregate, TIdentifier> _repository;
        private TAggregate? _aggregate;
        private List<LoadedEvent>? _uncommittedChanges;
        private int _uncommitedCount;

        public enum Mode
        {
            Given,
            When,
            Then
        }

        public AggregateTester(TIdentifier identifier)//Func<IEventStore, TRepository> repositoryFactory)
        {
            Identifier = identifier;
            _eventStore = new TestEventStore();
            //_repositoryFactory = repositoryFactory;
            GivenDateTime = new DateTimeOffset(2000, 1, 20, 8, 0, 0, TimeSpan.FromHours(2));
            WhenDateTime = new DateTimeOffset(2000, 2, 20, 8, 0, 0, TimeSpan.FromHours(2));

            AggregateContext = new TestAggregateContext(this);
            _repository = new TestRepository<TAggregate, TIdentifier>(_eventStore, AggregateContext);
        }

        public Mode TestMode { get; private set; }

        public IAggregateContext AggregateContext { get; }

        public TIdentifier Identifier { get; }

        public DateTimeOffset GivenDateTime { get; set; }

        public DateTimeOffset WhenDateTime { get; set; }

        public void Given(Event @event)
        {
            if (TestMode != Mode.Given)
            {
                throw new InvalidOperationException("TestMode error. Cannot execute Given() when TestMode is not Mode.Given");
            }

            _eventStore.AddTestEvents(
                Identifier.Identifier,
                new LoadedEvent(
                    GivenDateTime,
                    @event));
        }

        internal void When(TAggregate aggregate)
        {
            if (TestMode != Mode.Given)
            {
                throw new InvalidOperationException("TestMode error. Cannot execute When() when TestMode is not Given");
            }

            TestMode = Mode.When;

            _aggregate = aggregate;
        }

        public TAggregate When()
        {
            if (TestMode == Mode.Then)
            {
                throw new InvalidOperationException("TestMode error. Cannot execute When() when TestMode is not Given");
            }

            if (TestMode == Mode.Given)
            {
                TestMode = Mode.When;
                _aggregate = _repository.GetAsync(Identifier).Result;
            }

            if (_aggregate == null)
            {
                throw new NullReferenceException("_aggregate cannot be null");
            }

            return _aggregate;
        }

        public void ThenAggregate(Action<TAggregate> a)
        {
            PrepareThen();

            if (_aggregate == null)
            {
                throw new NullReferenceException("_aggregate cannot be null");
            }

            a(_aggregate);
        }


        internal void Then<TEvent>()
            where TEvent : Event
        {
            PrepareThen();
            GetNextEvent<TEvent>();
        }

        internal void Then<TEvent>(Action<TEvent> e)
            where TEvent : Event
        {
            PrepareThen();

            e(GetNextEvent<TEvent>());
        }

        internal void ThenNothing()
        {
            PrepareThen();

            var e = _uncommittedChanges?.FirstOrDefault();
            if (e != null)
            {
                _uncommittedChanges = null;
                Assert.Fail($"No events expected but event {e.Data.GetType().Name} found.");
            }
        }

        private TEvent GetNextEvent<TEvent>()
            where TEvent : Event
        {
            _uncommitedCount++;
            var e = _uncommittedChanges?.FirstOrDefault();
            if (e == null)
            {
                Assert.Fail($"There is no event {_uncommitedCount} of type {typeof(TEvent).Name} available.");
            }
            else
            {
                _uncommittedChanges?.RemoveAt(0);

                if (e.Data is TEvent @event)
                {
                    return @event;
                }

                Assert.Fail(
                    $"Expect event {_uncommitedCount} of type {typeof(TEvent).Name}, but event of type {e.Data.GetType().Name} found.");
            }

            return null!;
        }

        public void Dispose()
        {
            if (TestMode != Mode.Then)
            {
                throw new InvalidOperationException("TestMode error. Test has no Then.");
            }

            var e = _uncommittedChanges?.FirstOrDefault();
            if (e != null)
            {
                Assert.Fail($"Not all events are verified in the test. Please call ThenNothing() or Then<{e.Data.GetType().Name}>(e=>{{}});");
            }
        }

        private void PrepareThen()
        {
            if (TestMode == Mode.Given)
            {
                throw new InvalidOperationException("TestMode error. Cannot execute Then() when TestMode is Given");
            }

            if (_aggregate == null)
            {
                throw new NullReferenceException("_aggregate cannot be null");
            }

            if (TestMode == Mode.When)
            {
                _uncommittedChanges = _aggregate.GetUncommittedChanges().ToList();
                TestMode = Mode.Then;
            }
        }

        private class TestAggregateContext : IAggregateContext
        {
            private readonly AggregateTester<TAggregate, TIdentifier> _aggregateTester;

            public TestAggregateContext(AggregateTester<TAggregate, TIdentifier> aggregateTester)
            {
                _aggregateTester = aggregateTester;
            }

            public DateTimeOffset GetDateTime()
            {
                return _aggregateTester.WhenDateTime;
            }
        }
    }
}