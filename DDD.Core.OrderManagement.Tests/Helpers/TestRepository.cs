namespace DDD.Core.OrderManagement.Tests.Helpers
{
    public class TestRepository<TAggregate, TIdentifier> : Repository<TAggregate, TIdentifier>
        where TAggregate : AggregateRoot, IAggregateLoader
        where TIdentifier : IIdentifier
    {
        public TestRepository(IEventStore eventStore, IAggregateContext aggregateContext) 
            : base(eventStore, aggregateContext)
        {
        }

        protected override string GetStreamName(IIdentifier identifier)
        {
            return identifier.Identifier;
        }
    }
}