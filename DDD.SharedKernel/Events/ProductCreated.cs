namespace DDD.SharedKernel.Events
{
    public class ProductCreated : IDomainEvent
    {
        public ProductCreated(string productIdentifier, string productName)
        {
            ProductIdentifier = productIdentifier;
            ProductName = productName;
        }

        public ProductCreated()
        {
            ProductIdentifier = null!;
            ProductName = null!;
        }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }
    }
}