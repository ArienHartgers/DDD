using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products
{
    public class ProductRepository : Repository<Product, ProductIdentity>
    {
        public ProductRepository(IEventStore eventStore) : base(eventStore)
        {
        }

        public Product Create(ProductName productName)
        {
            return Create(new ProductCreated(ProductIdentity.New(), productName));
        }
    }
}