using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineCreated : Event
    {
        public OrderLineCreated(OrderIdentifier orderIdentifier, OrderLineIdentifier orderLineIdentifier, ProductIdentifier productIdentifier, int quantity)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
            ProductIdentifier = productIdentifier;
            Quantity = quantity;
        }

        public OrderIdentifier OrderIdentifier { get; set; }
        public OrderLineIdentifier OrderLineIdentifier { get; } 
        public ProductIdentifier ProductIdentifier { get; }
        public int Quantity { get;  }
    }
}