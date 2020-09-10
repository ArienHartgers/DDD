using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.App.EventConverters
{
    public class OrderLineCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineCreated, Events.OrderLineCreated>
    {
        public override Events.OrderLineCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineCreated e)
        {
            return new Events.OrderLineCreated(
                e.OrderIdentity.Identity,
                e.ProductIdentity.Identity,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineCreated ConvertToIntern(Events.OrderLineCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineCreated(
                OrderIdentity.Create(e.OrderLineIdentity),
                OrderLineIdentity.Create(e.OrderLineIdentity),
                ProductIdentity.Create(e.ProductIdentity), 
                e.Quantity);
        }
    }
}