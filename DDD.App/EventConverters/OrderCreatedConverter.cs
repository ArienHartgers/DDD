using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App.EventConverters
{
    public class OrderCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderCreated, Events.OrderCreated>
    {
        public override Events.OrderCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderCreated e)
        {
            return new Events.OrderCreated(
                e.OrderIdentity.ToString(),
                e.CustomerIdentity.CustomerGuid);
        }

        public override Core.OrderManagement.Orders.Events.OrderCreated ConvertToIntern(Events.OrderCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderCreated(
                OrderIdentity.Create(e.OrderIdentity),
                CustomerIdentity.Create(e.CustomerGuid));
        }
    }
}