using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class ProductNameChangedConverter : EventConverter<Core.OrderManagement.Products.Events.ProductNameChanged, ProductNameChanged>
    {
        public override ProductNameChanged ConvertToExtern(Core.OrderManagement.Products.Events.ProductNameChanged e)
        {
            return new ProductNameChanged(
                e.ProductIdentifier.Identifier,
                e.ProductName.Name);
        }

        public override Core.OrderManagement.Products.Events.ProductNameChanged ConvertToIntern(ProductNameChanged e)
        {
            return new Core.OrderManagement.Products.Events.ProductNameChanged(
                ProductIdentifier.Create(e.ProductIdentifier),
                ProductName.Create(e.ProductName));
        }
    }
}