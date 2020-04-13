using System;
using System.Text.Json;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCreated : Event
    {
        public OrderCreated(OrderIdentity orderIdentity)
        {
            OrderIdentity = orderIdentity;
        }

        public OrderIdentity OrderIdentity { get; }
    }
}