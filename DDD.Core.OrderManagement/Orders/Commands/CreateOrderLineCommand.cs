using System.Linq;
using DDD.Core.OrderManagement.Orders.Events;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class CreateOrderLineCommand
    {
        public static OrderLineIdentity CreateOrderLine(this Order order, string product, int quantity)
        {
            var itemLine = order.OrderLines.FirstOrDefault(ol => ol.ProductName == product);
            if (itemLine != null)
            {
                order.ApplyChange(new OrderLineQuantityAdjustedEvent(
                    order.Identity,
                    itemLine.Identity,
                    itemLine.Quantity + quantity));

                return itemLine.Identity;
            }

            var identity = new OrderLineIdentity(order.LastOrderLineId + 1);
            order.ApplyChange(new OrderLineCreatedEvent
            {
                LineId = identity.LineId,
                ProductName = product,
                Quantity = quantity,
            });

            return identity;
        }
    }
}