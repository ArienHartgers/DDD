using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App.Events
{
    public class OrderLineRemoved 
    {
        public OrderLineRemoved(string orderLineIdentity)
        {
            OrderLineIdentity = orderLineIdentity;
        }

        public string OrderLineIdentity { get; }
    }
}