using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineCreated : Event
    {
        public OrderLineCreated(
            OrderIdentifier orderIdentifier, 
            OrderLineIdentifier orderLineIdentifier, 
            ProductIdentifier productIdentifier,
            ProductName productName,
            int quantity)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
            ProductIdentifier = productIdentifier;
            ProductName = productName;
            Quantity = quantity;
        }

        public OrderIdentifier OrderIdentifier { get; set; }
        public OrderLineIdentifier OrderLineIdentifier { get; } 
        public ProductIdentifier ProductIdentifier { get; }
        public ProductName ProductName { get; }
        public int Quantity { get;  }
    }
}