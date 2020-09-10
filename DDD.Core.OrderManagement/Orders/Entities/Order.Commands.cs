using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class Order
    {
        public void ChangeCustomerName(string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new Exception("Invalid name");
            }

            if (CustomerName != customerName)
            {
                ApplyChange(new OrderCustomerNameChanged(Identity, customerName));
            }
        }

        public OrderLine CreateOrderLine(ProductIdentity productIdentity, int quantity)
        {
            var itemLine = Lines.FirstOrDefault(ol => ol.ProductIdentity == productIdentity);
            if (itemLine != null)
            {
                ApplyChange(new OrderLineQuantityAdjusted(
                    Identity,
                    itemLine.Identity,
                    itemLine.Quantity + quantity));

                return itemLine;
            }

            var identity = OrderLineIdentity.NextIdentity(Lines.LastIdentity);
            
            ApplyChange(
                new OrderLineCreated(
                    Identity, 
                    identity, 
                    productIdentity, 
                    quantity));

            return Lines.Get(identity);
        }
    }
}