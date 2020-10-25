using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products
{
    public class ProductRepository : Repository<Product, ProductIdentifier>
    {
        public ProductRepository(IEventStore eventStore) : base(eventStore)
        {
        }

    }
}