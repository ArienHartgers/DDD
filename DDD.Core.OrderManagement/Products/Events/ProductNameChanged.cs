using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Events
{
    public class ProductNameChanged : Event
    {
        public ProductNameChanged(ProductIdentifier productIdentifier, ProductName productName)
        {
            ProductIdentifier = productIdentifier;
            ProductName = productName;
        }

        public ProductIdentifier ProductIdentifier { get; }

        public ProductName ProductName { get; }
    }
}