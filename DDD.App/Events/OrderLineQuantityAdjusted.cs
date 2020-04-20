using System;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App.Events
{
    public class OrderLineQuantityAdjusted
    {
        public OrderLineQuantityAdjusted(string orderIdentity, string orderLineIdentity, int quantity)
        {
            OrderIdentity = orderIdentity;
            OrderLineIdentity = orderLineIdentity;
            Quantity = quantity;
        }

        public string OrderIdentity { get; set; }
        public string OrderLineIdentity { get; set; }
        public int Quantity { get; set; }
    }
}