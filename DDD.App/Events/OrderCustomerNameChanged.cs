namespace DDD.App.Events
{
    public class OrderCustomerNameChanged 
    {
        public OrderCustomerNameChanged(string orderIdentity, string customerName)
        {
            OrderIdentity = orderIdentity;
            CustomerName = customerName;
        }

        public OrderCustomerNameChanged()
        {
        }

        public string OrderIdentity { get; set; } = null!;

        public string CustomerName { get; set; } = null!;
    }
}