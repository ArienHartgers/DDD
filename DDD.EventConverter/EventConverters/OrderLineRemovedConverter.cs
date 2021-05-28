using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class OrderLineRemovedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineRemoved, OrderLineRemoved>
    {
        public override OrderLineRemoved ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineRemoved e)
        {
            return new OrderLineRemoved(
                e.OrderIdentifier.ToString(),
                e.OrderLineIdentifier.Identifier);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineRemoved ConvertToIntern(OrderLineRemoved e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineRemoved(
                OrderIdentifier.Parse(e.OrderIdentifier),
                OrderLineIdentifier.Parse(e.OrderLineIdentifier));
        }
    }
}