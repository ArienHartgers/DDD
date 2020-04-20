using System;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderLineQuantityAdjusted : Event
    {
        public OrderLineQuantityAdjusted(OrderIdentity orderIdentity, OrderLineIdentity orderLineIdentity, int quantity)
        {
            OrderIdentity = orderIdentity;
            OrderLineIdentity = orderLineIdentity;
            Quantity = quantity;
        }

        public OrderIdentity OrderIdentity { get; set; }
        public OrderLineIdentity OrderLineIdentity { get; set; }
        public int Quantity { get; set; }
    }
}