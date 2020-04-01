namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineRemovedEvent : Event
    {
        public int LineId { get; set; }
    }
}