using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.App.EventConverters
{
    public class ProductCreatedConverter : EventConverter<Core.OrderManagement.Products.Events.ProductCreated, Events.ProductCreated>
    {
        public override Events.ProductCreated ConvertToExtern(Core.OrderManagement.Products.Events.ProductCreated e)
        {
            return new Events.ProductCreated(
                e.ProductIdentifier.Identifier,
                e.ProductName.Name);
        }

        public override Core.OrderManagement.Products.Events.ProductCreated ConvertToIntern(Events.ProductCreated e)
        {
            return new Core.OrderManagement.Products.Events.ProductCreated(
                ProductIdentifier.Create(e.ProductIdentifier),
                ProductName.Create(e.ProductName));
        }
    }
}