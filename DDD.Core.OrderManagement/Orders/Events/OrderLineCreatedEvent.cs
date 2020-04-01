namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineCreatedEvent : Event
    {
        public int LineId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
    }
}