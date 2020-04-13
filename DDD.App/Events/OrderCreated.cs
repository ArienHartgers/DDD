namespace DDD.App.Events
{
    public class OrderCreated
    {
        public OrderCreated(string orderIdentity)
        {
            OrderIdentity = orderIdentity;
        }

        public OrderCreated()
        {
        }

        public string OrderIdentity { get; set; }
    }
}