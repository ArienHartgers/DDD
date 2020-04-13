using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class AdjustOrderLineQuantityCommand
    {
        public static void AdjustOrderLineQuantity(
            this Order order, 
            OrderLineIdentity orderLineIdentity,
            int quantity)
        {
            var orderLine = order.FindOrderLine(orderLineIdentity);

            if (orderLine != null && orderLine.Quantity != quantity)
            {
                order.ApplyChange(
                    new OrderLineQuantityAdjustedEvent(
                        order.Identity,
                        orderLine.Identity,
                        quantity));
            }
        }
    }
}