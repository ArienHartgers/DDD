using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class OrderCustomerNameChangedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderCustomerNameChanged, OrderCustomerNameChanged>
    {
        public override OrderCustomerNameChanged ConvertToExtern(Core.OrderManagement.Orders.Events.OrderCustomerNameChanged e)
        {
            return new OrderCustomerNameChanged(
                e.OrderIdentifier.ToString(),
                e.CustomerName);
        }

        public override Core.OrderManagement.Orders.Events.OrderCustomerNameChanged ConvertToIntern(OrderCustomerNameChanged e)
        {
            return new Core.OrderManagement.Orders.Events.OrderCustomerNameChanged(
                OrderIdentifier.Create(e.OrderIdentifier),
                e.CustomerName);
        }
    }
}