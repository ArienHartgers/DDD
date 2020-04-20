using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class RemoveOrderLineCommand
    {
        public static void RemoveOrderLine(this Order order, OrderLineIdentity orderLineIdentity)
        {
            if (order.FindOrderLine(orderLineIdentity) != null)
            {
                order.ApplyChange(new OrderLineRemoved(
                    order.Identity,
                    orderLineIdentity));
            }
        }
    }
}