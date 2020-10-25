namespace DDD.SharedKernel.Events
{
    public class OrderCustomerNameChanged : IDomainEvent
    {
        public OrderCustomerNameChanged(string orderIdentifier, string customerName)
        {
            OrderIdentifier = orderIdentifier;
            CustomerName = customerName;
        }

        public OrderCustomerNameChanged()
        {
        }

        public string OrderIdentifier { get; set; } = null!;

        public string CustomerName { get; set; } = null!;
    }
}