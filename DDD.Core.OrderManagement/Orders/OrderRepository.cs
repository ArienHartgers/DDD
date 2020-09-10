using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders
{
    public class OrderRepository : Repository<Order, OrderIdentity>
    {
        public OrderRepository(IEventStore eventStore) : base(eventStore)
        {
        }

        public Order CreateOrder()
        {
            return Create(new OrderCreated(OrderIdentity.New(), CustomerIdentity.New()));
        }
    }
}