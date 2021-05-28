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
            OrderIdentifier = null!; ;
            CustomerName = null!; ;
        }

        public string OrderIdentifier { get; set; }

        public string CustomerName { get; set; }
    }
}