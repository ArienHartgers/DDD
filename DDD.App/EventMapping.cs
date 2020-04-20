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
                case Core.OrderManagement.Orders.Events.OrderCreated e:
                    return new DDD.App.Events.OrderCreated(
                        e.OrderIdentity.Identity);

                case Core.OrderManagement.Orders.Events.OrderCustomerNameChanged e:
                    return new DDD.App.Events.OrderCustomerNameChanged(
                        e.OrderIdentity.Identity, 
                        e.CustomerName);

                default: throw new Exception("Unknown event type");
            }
        }

        public static Event Convert2(object @event)
        {
            switch (@event)
            {
                case DDD.App.Events.OrderCreated orderCreated:
                    return new Core.OrderManagement.Orders.Events.OrderCreated(
                        OrderIdentity.Create(orderCreated.OrderIdentity),
                        CustomerIdentity.New());

                case DDD.App.Events.OrderCustomerNameChanged e:
                    return new Core.OrderManagement.Orders.Events.OrderCustomerNameChanged(
                        OrderIdentity.Create(e.OrderIdentity), 
                        e.CustomerName);



                default: throw new Exception("Unknown event type");
            }
        }
    }
}