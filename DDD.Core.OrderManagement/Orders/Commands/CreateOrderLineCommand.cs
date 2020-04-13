using System.Linq;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class CreateOrderLineCommand
    {
        public static OrderLineIdentity CreateOrderLine(this Order order, string product, int quantity)
        {
            var itemLine = order.OrderLines.Entities.FirstOrDefault(ol => ol.ProductName == product);
            if (itemLine != null)
            {
                order.ApplyChange(new OrderLineQuantityAdjustedEvent(
                    order.Identity,
                    itemLine.Identity,
                    itemLine.Quantity + quantity));

                return itemLine.Identity;
            }

            var identity = OrderLineIdentity.NextIdentity(order.OrderLines.MaxIdentity);
            order.ApplyChange(new OrderLineCreatedEvent
            {
                OrderLineIdentity = identity,
                ProductName = product,
                Quantity = quantity,
            });

            return identity;
        }
    }
}