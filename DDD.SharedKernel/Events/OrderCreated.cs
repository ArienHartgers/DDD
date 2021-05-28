namespace DDD.SharedKernel.Events
{
    public class OrderCreated : IDomainEvent
    {
        public OrderCreated(string orderIdentifier, string customerIdentifier)
        {
            OrderIdentifier = orderIdentifier;
            CustomerIdentifier = customerIdentifier;
        }

        public OrderCreated()
        {
            OrderIdentifier = null!;
            CustomerIdentifier = null!;
        }

        public string OrderIdentifier { get; set; }
        public string CustomerIdentifier { get; set; }
    }
}