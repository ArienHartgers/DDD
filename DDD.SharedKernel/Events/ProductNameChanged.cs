namespace DDD.SharedKernel.Events
{
    public class ProductNameChanged : IDomainEvent
    {
        public ProductNameChanged(string productIdentifier, string productName)
        {
            ProductIdentifier = productIdentifier;
            ProductName = productName;
        }

        public string ProductIdentifier { get; }

        public string ProductName { get; }
    }
}