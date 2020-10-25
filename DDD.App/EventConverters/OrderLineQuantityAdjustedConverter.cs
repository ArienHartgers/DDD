using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.App.EventConverters
{
    public class OrderLineQuantityAdjustedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted, Events.OrderLineQuantityAdjusted>
    {
        public override Events.OrderLineQuantityAdjusted ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted e)
        {
            return new Events.OrderLineQuantityAdjusted(
                e.OrderIdentifier.ToString(),
                e.OrderLineIdentifier.Identifier,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted ConvertToIntern(Events.OrderLineQuantityAdjusted e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted(
                OrderIdentifier.Create(e.OrderIdentifier),
                OrderLineIdentifier.Create(e.OrderLineIdentifier),
                e.Quantity);
        }
    }
}