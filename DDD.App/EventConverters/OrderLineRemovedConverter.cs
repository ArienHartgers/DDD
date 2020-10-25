using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.App.EventConverters
{
    public class OrderLineRemovedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineRemoved, Events.OrderLineRemoved>
    {
        public override Events.OrderLineRemoved ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineRemoved e)
        {
            return new Events.OrderLineRemoved(
                e.OrderIdentifier.ToString(),
                e.OrderLineIdentifier.Identifier);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineRemoved ConvertToIntern(Events.OrderLineRemoved e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineRemoved(
                OrderIdentifier.Create(e.OrderIdentifier),
                OrderLineIdentifier.Create(e.OrderLineIdentifier));
        }
    }
}