using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineQuantityAdjusted : Event
    {
        public OrderLineQuantityAdjusted(OrderIdentifier orderIdentifier, OrderLineIdentifier orderLineIdentifier, int quantity)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
            Quantity = quantity;
        }

        public OrderIdentifier OrderIdentifier { get; set; }
        public OrderLineIdentifier OrderLineIdentifier { get; set; }
        public int Quantity { get; set; }
    }
}