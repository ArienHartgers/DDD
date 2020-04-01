namespace DDD.Core.OrderManagement.Orders.Events
{
    public class UnknownEvent : Event
    {
        public string X { get; set; } = null!;
    }
}