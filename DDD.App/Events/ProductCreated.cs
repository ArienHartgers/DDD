namespace DDD.App.Events
{
    public class ProductCreated : IDomainEvent
    {
        public ProductCreated(string productIdentity, string productName)
        {
            ProductIdentity = productIdentity;
            ProductName = productName;
        }

        public string ProductIdentity { get; }

        public string ProductName { get; }
    }
}