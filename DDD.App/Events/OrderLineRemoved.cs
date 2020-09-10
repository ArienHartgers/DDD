using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App.Events
{
    public class OrderLineRemoved : IDomainEvent
    {
        public OrderLineRemoved(string orderIdentity, string orderLineIdentity)
        {
            OrderIdentity = orderIdentity;
            OrderLineIdentity = orderLineIdentity;
        }

        public string OrderIdentity { get; }
        public string OrderLineIdentity { get; }
    }
}