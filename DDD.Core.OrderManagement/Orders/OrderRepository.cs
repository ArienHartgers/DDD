using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identifiers;

namespace DDD.Core.OrderManagement.Orders
{
    public class OrderRepository : Repository<Order, OrderIdentifier>
    {
        public OrderRepository(IEventStore eventStore, IAggregateContext context) 
            : base(eventStore, context)
        {
        }

        protected override string GetStreamName(IIdentifier identifier)
        {
            return $"order-{identifier.Identifier}";
        }
    }
}