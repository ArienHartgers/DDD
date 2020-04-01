namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCustomerNameChangedEvent : Event
    {
        public string CustomerName { get; set; } = null!;
    }
}