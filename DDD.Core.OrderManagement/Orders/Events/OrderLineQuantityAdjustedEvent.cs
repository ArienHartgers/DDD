using System;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineQuantityAdjustedEvent : Event
    {
        public OrderLineQuantityAdjustedEvent()
        {
        }

        public OrderLineQuantityAdjustedEvent(OrderIdentity orderIdentity, OrderLineIdentity orderLineIdentity, int quantity)
        {
            OrderId = orderIdentity;
            OrderLineIdentity = orderLineIdentity;
            Quantity = quantity;
        }

        public OrderIdentity OrderId { get; set; }
        public OrderLineIdentity OrderLineIdentity { get; set; }
        public int Quantity { get; set; }
    }
}