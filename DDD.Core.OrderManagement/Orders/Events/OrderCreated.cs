using DDD.Core.OrderManagement.Orders.Identifiers;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCreated : Event
    {
        public OrderCreated(OrderIdentifier orderIdentifier, CustomerIdentifier customerIdentifier)
        {
            OrderIdentifier = orderIdentifier;
            CustomerIdentifier = customerIdentifier;
        }

        public OrderIdentifier OrderIdentifier { get; }
        public CustomerIdentifier CustomerIdentifier { get; }
    }
}