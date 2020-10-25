using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Events
{
    public class ProductCreated : Event
    {
        public ProductCreated(ProductIdentifier productIdentifier, ProductName productName)
        {
            ProductIdentifier = productIdentifier;
            ProductName = productName;
        }

        public ProductIdentifier ProductIdentifier { get; }

        public ProductName ProductName { get; }
    }
}