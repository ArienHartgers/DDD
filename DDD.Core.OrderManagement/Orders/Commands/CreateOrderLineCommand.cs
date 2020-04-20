using System.Linq;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class CreateOrderLineCommand
    {
        public static OrderLineIdentity CreateOrderLine(this Order order, ProductIdentity productIdentity, int quantity)
        {
            var itemLine = order.OrderLines.Entities.FirstOrDefault(ol => ol.ProductIdentity == productIdentity);
            if (itemLine != null)
            {
                order.ApplyChange(new OrderLineQuantityAdjusted(
                    order.Identity,
                    itemLine.Identity,
                    itemLine.Quantity + quantity));

                return itemLine.Identity;
            }

            var identity = OrderLineIdentity.NextIdentity(order.OrderLines.MaxIdentity);
            order.ApplyChange(new OrderLineCreated(order.Identity, identity, productIdentity, quantity));

            return identity;
        }
    }
}