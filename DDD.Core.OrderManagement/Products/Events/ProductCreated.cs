using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Events
{
    public class ProductCreated : Event
    {
        public ProductCreated(ProductIdentity productIdentity, ProductName productName)
        {
            ProductIdentity = productIdentity;
            ProductName = productName;
        }

        public ProductIdentity ProductIdentity { get; }

        public ProductName ProductName { get; }
    }
}