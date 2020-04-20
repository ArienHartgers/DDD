using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineRemoved : Event
    {
        public OrderLineRemoved(OrderIdentity orderIdentity, OrderLineIdentity orderLineIdentity)
        {
            OrderIdentity = orderIdentity;
            OrderLineIdentity = orderLineIdentity;
        }

        public OrderIdentity OrderIdentity { get; set; }
        public OrderLineIdentity OrderLineIdentity { get; }
    }
}