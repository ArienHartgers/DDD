namespace DDD.SharedKernel.Events
{
    public class ProductNameChanged : IDomainEvent
    {
        public ProductNameChanged(string productIdentifier, string productName)
        {
            ProductIdentifier = productIdentifier;
            ProductName = productName;
        }

        public ProductNameChanged()
        {
            ProductIdentifier = null!;
            ProductName = null!;
        }

        public string ProductIdentifier { get; set; }

        public string ProductName { get; set; }
    }
}