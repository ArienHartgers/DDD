using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineCreatedEvent : Event
    {
        public OrderLineIdentity OrderLineIdentity { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}