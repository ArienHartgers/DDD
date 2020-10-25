namespace DDD.SharedKernel.Events
{
    public class OrderLineRemoved : IDomainEvent
    {
        public OrderLineRemoved(string orderIdentifier, string orderLineIdentifier)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
        }

        public string OrderIdentifier { get; }
        public string OrderLineIdentifier { get; }
    }
}