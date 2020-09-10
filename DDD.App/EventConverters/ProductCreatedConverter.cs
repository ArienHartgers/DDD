using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.App.EventConverters
{
    public class ProductCreatedConverter : EventConverter<Core.OrderManagement.Products.Events.ProductCreated, Events.ProductCreated>
    {
        public override Events.ProductCreated ConvertToExtern(Core.OrderManagement.Products.Events.ProductCreated e)
        {
            return new Events.ProductCreated(
                e.ProductIdentity.Identity,
                e.ProductName.Name);
        }

        public override Core.OrderManagement.Products.Events.ProductCreated ConvertToIntern(Events.ProductCreated e)
        {
            return new Core.OrderManagement.Products.Events.ProductCreated(
                ProductIdentity.Create(e.ProductIdentity),
                ProductName.Create(e.ProductName));
        }
    }
}