using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Identitfiers;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class Order
    {
        public static Order Create(
            IAggregateContext context, 
            OrderIdentifier orderIdentifier, 
            CustomerIdentifier customerIdentifier)
        {
            return CreateWithEvent<Order, OrderCreated>(
                context,
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
            if (Lines.Any(ol => ol.ProductIdentifier == productIdentifier))
            {
                throw new InvalidOperationException($"An orderline with product {productIdentifier} already exists");
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

        public void AdjustOrderLine(OrderLineIdentifier orderLineIdentifier, in int quantity)
        {
            var orderLine = _orderLines.Get(orderLineIdentifier);
            if (orderLine.Quantity != quantity)
            {
                ApplyChange(new OrderLineQuantityAdjusted(
                    OrderIdentifier,
                    orderLine.Identifier,
                    quantity));
            }
        }
    }
}