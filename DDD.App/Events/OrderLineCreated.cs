using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.App.Events
{
    public class OrderLineCreated
    {
        public OrderLineCreated(string orderLineIdentity, string productIdentity, int quantity)
        {
            OrderLineIdentity = orderLineIdentity;
            ProductIdentity = productIdentity;
            Quantity = quantity;
        }

        public string OrderLineIdentity { get; } 
        public string ProductIdentity { get; }
        public int Quantity { get;  }
    }
}