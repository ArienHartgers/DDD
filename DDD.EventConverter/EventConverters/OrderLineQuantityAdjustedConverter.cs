using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class OrderLineQuantityAdjustedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted, OrderLineQuantityAdjusted>
    {
        public override OrderLineQuantityAdjusted ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted e)
        {
            return new OrderLineQuantityAdjusted(
                e.OrderIdentifier.ToString(),
                e.OrderLineIdentifier.Identifier,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted ConvertToIntern(OrderLineQuantityAdjusted e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted(
                OrderIdentifier.Create(e.OrderIdentifier),
                OrderLineIdentifier.Create(e.OrderLineIdentifier),
                e.Quantity);
        }
    }
}