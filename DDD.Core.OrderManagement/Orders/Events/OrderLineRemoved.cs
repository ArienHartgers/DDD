using DDD.Core.OrderManagement.Orders.Identifiers;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineRemoved : Event
    {
        public OrderLineRemoved(OrderIdentifier orderIdentifier, OrderLineIdentifier orderLineIdentifier)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
        }

        public OrderIdentifier OrderIdentifier { get; set; }
        public OrderLineIdentifier OrderLineIdentifier { get; }
    }
}