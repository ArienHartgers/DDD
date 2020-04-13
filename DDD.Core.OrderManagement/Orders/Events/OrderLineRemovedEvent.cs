using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineRemovedEvent : Event
    {
        public OrderLineRemovedEvent(OrderLineIdentity orderLineIdentity)
        {
            OrderLineIdentity = orderLineIdentity;
        }

        public OrderLineIdentity OrderLineIdentity { get; }
    }
}