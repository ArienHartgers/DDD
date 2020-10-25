using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCustomerNameChanged : Event
    {
        public OrderCustomerNameChanged(OrderIdentifier orderIdentifier, string customerName)
        {
            OrderIdentifier = orderIdentifier;
            CustomerName = customerName;
        }

        public OrderIdentifier OrderIdentifier { get; set; }
        public string CustomerName { get; }
    }
}