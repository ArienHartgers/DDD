using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineCreated : Event
    {
        public OrderLineCreated(OrderIdentity orderIdentity, OrderLineIdentity orderLineIdentity, ProductIdentity productIdentity, int quantity)
        {
            OrderIdentity = orderIdentity;
            OrderLineIdentity = orderLineIdentity;
            ProductIdentity = productIdentity;
            Quantity = quantity;
        }

        public OrderIdentity OrderIdentity { get; set; }
        public OrderLineIdentity OrderLineIdentity { get; } 
        public ProductIdentity ProductIdentity { get; }
        public int Quantity { get;  }
    }
}