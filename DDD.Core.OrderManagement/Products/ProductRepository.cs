using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Products
{
    public class ProductRepository : Repository<Product, ProductIdentifier>
    {
        public ProductRepository(IEventStore eventStore, IAggregateContext context) 
            : base(eventStore, context)
        {
        }

    }
}