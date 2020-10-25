using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class OrderLineCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineCreated, OrderLineCreated>
    {
        public override OrderLineCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineCreated e)
        {
            return new OrderLineCreated(
                e.OrderIdentifier.Identifier,
                e.ProductIdentifier.Identifier,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineCreated ConvertToIntern(OrderLineCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineCreated(
                OrderIdentifier.Create(e.OrderLineIdentifier),
                OrderLineIdentifier.Create(e.OrderLineIdentifier),
                ProductIdentifier.Create(e.ProductIdentifier), 
                e.Quantity);
        }
    }
}