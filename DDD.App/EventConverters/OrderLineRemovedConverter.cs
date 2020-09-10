using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App.EventConverters
{
    public class OrderLineRemovedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineRemoved, Events.OrderLineRemoved>
    {
        public override Events.OrderLineRemoved ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineRemoved e)
        {
            return new Events.OrderLineRemoved(
                e.OrderIdentity.ToString(),
                e.OrderLineIdentity.Identity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineRemoved ConvertToIntern(Events.OrderLineRemoved e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineRemoved(
                OrderIdentity.Create(e.OrderIdentity),
                OrderLineIdentity.Create(e.OrderLineIdentity));
        }
    }
}