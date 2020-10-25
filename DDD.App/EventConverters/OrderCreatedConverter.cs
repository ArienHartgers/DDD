using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.App.EventConverters
{
    public class OrderCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderCreated, Events.OrderCreated>
    {
        public override Events.OrderCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderCreated e)
        {
            return new Events.OrderCreated(
                e.OrderIdentifier.ToString(),
                e.CustomerIdentifier.CustomerGuid);
        }

        public override Core.OrderManagement.Orders.Events.OrderCreated ConvertToIntern(Events.OrderCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderCreated(
                OrderIdentifier.Create(e.OrderIdentifier),
                CustomerIdentifier.Create(e.CustomerGuid));
        }
    }
}