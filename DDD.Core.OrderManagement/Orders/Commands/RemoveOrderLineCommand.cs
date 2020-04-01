using DDD.Core.OrderManagement.Orders.Events;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class RemoveOrderLineCommand
    {
        public static void RemoveOrderLine(this Order order, OrderLineIdentity orderLineIdentity)
        {
            if (order.FindOrderLine(orderLineIdentity) != null)
            {
                order.ApplyChange(new OrderLineRemovedEvent
                {
                    LineId = orderLineIdentity.LineId,
                });
            }
        }
    }
}