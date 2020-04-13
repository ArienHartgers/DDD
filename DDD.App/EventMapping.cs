using System;
using DDD.Core;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App
{
    internal static class EventMapping
    {
        public static object Convert(Event @event)
        {
            switch (@event)
            {
                case Core.OrderManagement.Orders.Events.OrderCreated orderCreated:
                    return new DDD.App.Events.OrderCreated(
                        orderCreated.OrderIdentity.Identity);
                default: throw new Exception("Unknown event type");
            }
        }

        public static Event Convert2(object @event)
        {
            switch (@event)
            {
                case DDD.App.Events.OrderCreated orderCreated:
                    return new Core.OrderManagement.Orders.Events.OrderCreated(
                        OrderIdentity.Parse(orderCreated.OrderIdentity));
                default: throw new Exception("Unknown event type");
            }
        }
    }
}