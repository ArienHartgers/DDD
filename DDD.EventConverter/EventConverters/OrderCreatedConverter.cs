using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class OrderCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderCreated, OrderCreated>
    {
        public override OrderCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderCreated e)
        {
            return new OrderCreated(
                e.OrderIdentifier.ToString(),
                e.CustomerIdentifier.ToString());
        }

        public override Core.OrderManagement.Orders.Events.OrderCreated ConvertToIntern(OrderCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderCreated(
                OrderIdentifier.Parse(e.OrderIdentifier),
                CustomerIdentifier.Parse(e.CustomerIdentifier));
        }
    }
}