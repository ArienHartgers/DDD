using System;
using System.Text.Json;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCreated : Event
    {
        public OrderCreated(OrderIdentity orderIdentity, CustomerIdentity customerIdentity)
        {
            OrderIdentity = orderIdentity;
            CustomerIdentity = customerIdentity;
        }

        public OrderIdentity OrderIdentity { get; }
        public CustomerIdentity CustomerIdentity { get; }
    }
}