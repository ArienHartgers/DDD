using DDD.Core.OrderManagement.Orders.Events;

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