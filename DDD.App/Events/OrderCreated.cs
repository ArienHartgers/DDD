using System;

namespace DDD.App.Events
{
    public class OrderCreated : IDomainEvent
    {
        public OrderCreated(string orderIdentifier, Guid customerGuid)
        {
            OrderIdentifier = orderIdentifier;
            CustomerGuid = customerGuid;
        }

        public string OrderIdentifier { get; set; }
        public Guid CustomerGuid { get; }
    }
}