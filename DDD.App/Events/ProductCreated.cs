namespace DDD.App.Events
{
    public class ProductCreated : IDomainEvent
    {
        public ProductCreated(string productIdentifier, string productName)
        {
            ProductIdentifier = productIdentifier;
            ProductName = productName;
        }

        public string ProductIdentifier { get; }

        public string ProductName { get; }
    }
}