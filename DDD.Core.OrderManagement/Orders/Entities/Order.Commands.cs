using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identitfiers;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class Order
    {
        public static Order Create(DateTimeOffset now, OrderIdentifier orderIdentifier, CustomerIdentifier customerIdentifier)
        {
            return CreateWithEvent<Order, OrderCreated>(
                now,
                new OrderCreated(
                    orderIdentifier,
                    customerIdentifier));
        }

        public void ChangeCustomerName(string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new Exception("Invalid name");
            }

            if (CustomerName != customerName)
            {
                ApplyChange(new OrderCustomerNameChanged(OrderIdentifier, customerName));
            }
        }

        public OrderLine CreateOrderLine(ProductIdentifier productIdentifier, int quantity)
        {
            var itemLine = Lines.FirstOrDefault(ol => ol.ProductIdentifier == productIdentifier);
            if (itemLine != null)
            {
                ApplyChange(new OrderLineQuantityAdjusted(
                    OrderIdentifier,
                    itemLine.Identifier,
                    itemLine.Quantity + quantity));

                return itemLine;
            }

            var orderLineIdentifier = OrderLineIdentifier.NextIdentifier(Lines.LastIdentifier);
            
            ApplyChange(
                new OrderLineCreated(
                    OrderIdentifier, 
                    orderLineIdentifier, 
                    productIdentifier, 
                    quantity));

            return Lines.Get(orderLineIdentifier);
        }
    }
}