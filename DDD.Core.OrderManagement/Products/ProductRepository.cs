using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Identitfiers;

namespace DDD.Core.OrderManagement.Products
{
    public class ProductRepository : Repository<Product, ProductIdentifier>
    {
        public ProductRepository(IEventStore eventStore, IAggregateContext context) 
            : base(eventStore, context)
        {
        }

        protected override string GetStreamName(IIdentifier identifier)
        {
            return $"product-{identifier.Identifier}";
        }
    }
}