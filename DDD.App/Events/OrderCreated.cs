using System;

namespace DDD.App.Events
{
    public class OrderCreated : IDomainEvent
    {
        public OrderCreated(string orderIdentity, Guid customerGuid)
        {
            OrderIdentity = orderIdentity;
            CustomerGuid = customerGuid;
        }

        public string OrderIdentity { get; set; }
        public Guid CustomerGuid { get; }
    }
}