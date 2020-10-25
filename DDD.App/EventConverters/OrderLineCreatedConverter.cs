using DDD.Core.OrderManagement.Orders.Identitfiers;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.App.EventConverters
{
    public class OrderLineCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineCreated, Events.OrderLineCreated>
    {
        public override Events.OrderLineCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineCreated e)
        {
            return new Events.OrderLineCreated(
                e.OrderIdentifier.Identifier,
                e.ProductIdentifier.Identifier,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineCreated ConvertToIntern(Events.OrderLineCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineCreated(
                OrderIdentifier.Create(e.OrderLineIdentifier),
                OrderLineIdentifier.Create(e.OrderLineIdentifier),
                ProductIdentifier.Create(e.ProductIdentifier), 
                e.Quantity);
        }
    }
}