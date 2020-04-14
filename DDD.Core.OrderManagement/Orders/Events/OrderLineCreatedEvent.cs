using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineCreatedEvent : Event
    {
        public OrderLineCreatedEvent(OrderLineIdentity orderLineIdentity, ProductIdentity productIdentity, int quantity)
        {
            OrderLineIdentity = orderLineIdentity;
            ProductIdentity = productIdentity;
            Quantity = quantity;
        }

        public OrderLineIdentity OrderLineIdentity { get; } 
        public ProductIdentity ProductIdentity { get; }
        public int Quantity { get;  }
    }
}