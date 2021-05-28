namespace DDD.SharedKernel.Events
{
    public class OrderLineRemoved : IDomainEvent
    {
        public OrderLineRemoved(string orderIdentifier, string orderLineIdentifier)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
        }

        public OrderLineRemoved()
        {
            OrderIdentifier = null!;
            OrderLineIdentifier = null!;
        }

        public string OrderIdentifier { get; set; }
        public string OrderLineIdentifier { get; set; }
    }
}