using System;
using System.Linq;
using System.Reflection;

namespace DDD.Core.OrderManagement.Orders
{
    public class Repository<TAggregateRoot, TIdentity>
        where TAggregateRoot : AggregateRoot<TIdentity>, IAggregateLoader
        where TIdentity : class
    {
        private readonly EventStore _eventStore;

        public Repository(EventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public TAggregateRoot Load(Guid id)
        {
            var constructors = typeof(TAggregateRoot).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            var constructor = constructors.SingleOrDefault();
            if (constructor == null)
            {
                throw new Exception($"Cannot find single private constructor for class {typeof(TAggregateRoot).FullName}");
            }

            var aggregate = (TAggregateRoot)constructor.Invoke(new object[0]);

            IAggregateLoader loader = aggregate;
            loader.LoadFromHistory(_eventStore.GetStreamEvents(id.ToString()));
            return aggregate;
        }

        public void Save(TAggregateRoot aggregate)
        {
            IAggregateLoader loader = aggregate;
            var changes = loader.GetUncommittedChanges();
            var streamName = aggregate.Identity.ToString();
            _eventStore.SaveEvents(streamName, changes.ToArray());
            loader.MarkChangesAsCommitted();
        }
    }
}