namespace DDD.SharedKernel.Events
{
    public class OrderLineCreated : IDomainEvent
    {
        public OrderLineCreated(
            string orderIdentifier,
            string orderLineIdentifier, 
            string productIdentifier,
            string productName, 
            int quantity)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
            ProductIdentifier = productIdentifier;
            ProductName = productName;
            Quantity = quantity;
        }

        public OrderLineCreated()
        {
            OrderIdentifier = null!;
            OrderLineIdentifier = null!;
            ProductIdentifier = null!; 
            ProductName = null!; 
        }

        public string OrderIdentifier { get; set; } 
        public string OrderLineIdentifier { get; set; } 
        public string ProductIdentifier { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }
}