using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.Core.OrderManagement.Orders
{
    public class OrderRepository : Repository<Order, OrderIdentifier>
    {
        public OrderRepository(IEventStore eventStore, IAggregateContext context) 
            : base(eventStore, context)
        {
        }
    }
}