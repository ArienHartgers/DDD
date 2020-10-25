using System;

namespace DDD.App.Events
{
    public class OrderLineQuantityAdjusted : IDomainEvent
    {
        public OrderLineQuantityAdjusted(string orderIdentifier, string orderLineIdentifier, int quantity)
        {
            OrderIdentifier = orderIdentifier;
            OrderLineIdentifier = orderLineIdentifier;
            Quantity = quantity;
        }

        public string OrderIdentifier { get; set; }
        public string OrderLineIdentifier { get; set; }
        public int Quantity { get; set; }
    }
}