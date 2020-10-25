using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.App.EventConverters
{
    public class OrderCustomerNameChangedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderCustomerNameChanged, Events.OrderCustomerNameChanged>
    {
        public override Events.OrderCustomerNameChanged ConvertToExtern(Core.OrderManagement.Orders.Events.OrderCustomerNameChanged e)
        {
            return new Events.OrderCustomerNameChanged(
                e.OrderIdentifier.ToString(),
                e.CustomerName);
        }

        public override Core.OrderManagement.Orders.Events.OrderCustomerNameChanged ConvertToIntern(Events.OrderCustomerNameChanged e)
        {
            return new Core.OrderManagement.Orders.Events.OrderCustomerNameChanged(
                OrderIdentifier.Create(e.OrderIdentifier),
                e.CustomerName);
        }
    }
}