namespace DDD.App.Events
{
    public class ProductNameChanged : IDomainEvent
    {
        public ProductNameChanged(string productIdentity, string productName)
        {
            ProductIdentity = productIdentity;
            ProductName = productName;
        }

        public string ProductIdentity { get; }

        public string ProductName { get; }
    }
}