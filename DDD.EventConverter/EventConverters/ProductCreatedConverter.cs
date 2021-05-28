using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class ProductCreatedConverter : EventConverter<Core.OrderManagement.Products.Events.ProductCreated, ProductCreated>
    {
        public override ProductCreated ConvertToExtern(Core.OrderManagement.Products.Events.ProductCreated e)
        {
            return new ProductCreated(
                e.ProductIdentifier.Identifier,
                e.ProductName.Name);
        }

        public override Core.OrderManagement.Products.Events.ProductCreated ConvertToIntern(ProductCreated e)
        {
            return new Core.OrderManagement.Products.Events.ProductCreated(
                ProductIdentifier.Parse(e.ProductIdentifier),
                ProductName.Create(e.ProductName));
        }
    }
}