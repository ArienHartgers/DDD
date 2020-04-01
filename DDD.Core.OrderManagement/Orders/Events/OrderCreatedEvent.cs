using System;

namespace DDD.Core.OrderManagement.Orders.Events
{
    public class OrderCreatedEvent: Event
    {
        public OrderCreatedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}