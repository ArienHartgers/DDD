using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App.EventConverters
{
    public class OrderLineQuantityAdjustedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted, Events.OrderLineQuantityAdjusted>
    {
        public override Events.OrderLineQuantityAdjusted ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted e)
        {
            return new Events.OrderLineQuantityAdjusted(
                e.OrderIdentity.ToString(),
                e.OrderLineIdentity.Identity,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted ConvertToIntern(Events.OrderLineQuantityAdjusted e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineQuantityAdjusted(
                OrderIdentity.Create(e.OrderIdentity),
                OrderLineIdentity.Create(e.OrderLineIdentity),
                e.Quantity);
        }
    }
}