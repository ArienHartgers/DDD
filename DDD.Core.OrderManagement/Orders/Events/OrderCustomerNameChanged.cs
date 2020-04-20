using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCustomerNameChanged : Event
    {
        public OrderCustomerNameChanged(OrderIdentity orderIdentity, string customerName)
        {
            OrderIdentity = orderIdentity;
            CustomerName = customerName;
        }

        public OrderIdentity OrderIdentity { get; set; }
        public string CustomerName { get; }
    }
}