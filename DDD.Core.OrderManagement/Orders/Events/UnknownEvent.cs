namespace DDD.Core.OrderManagement.Orders.Events
{
    public class UnknownEvent : Event
    {
        public UnknownEvent(string x)
        {
            X = x;
        }

        public string X { get; }
    }
}