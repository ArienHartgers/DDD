using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.App.EventConverters
{
    public class ProductNameChangedConverter : EventConverter<Core.OrderManagement.Products.Events.ProductNameChanged, Events.ProductNameChanged>
    {
        public override Events.ProductNameChanged ConvertToExtern(Core.OrderManagement.Products.Events.ProductNameChanged e)
        {
            return new Events.ProductNameChanged(
                e.ProductIdentity.Identity,
                e.ProductName.Name);
        }

        public override Core.OrderManagement.Products.Events.ProductNameChanged ConvertToIntern(Events.ProductNameChanged e)
        {
            return new Core.OrderManagement.Products.Events.ProductNameChanged(
                ProductIdentity.Create(e.ProductIdentity),
                ProductName.Create(e.ProductName));
        }
    }
}