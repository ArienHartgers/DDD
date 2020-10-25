namespace DDD.App.Events
{
    public class OrderLineCreated : IDomainEvent
    {
        public OrderLineCreated(string orderLineIdentifier, string productIdentifier, int quantity)
        {
            OrderLineIdentifier = orderLineIdentifier;
            ProductIdentifier = productIdentifier;
            Quantity = quantity;
        }

        public string OrderLineIdentifier { get; } 
        public string ProductIdentifier { get; }
        public int Quantity { get;  }
    }
}