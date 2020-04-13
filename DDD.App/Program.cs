using System.Text.Json;
using DDD.App.Events;
using DDD.Core;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Commands;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.App
{
    class Program
    {
        static void Main()
        {

            var e = new Core.OrderManagement.Orders.Events.OrderCreated(OrderIdentity.New());
            System.Console.WriteLine(JsonSerializer.Serialize(e, new JsonSerializerOptions { WriteIndented = true }));


            var a = EventMapping.Convert(e);
            var json = JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = true, Converters = { new IdentityJsonConverter() } });

            System.Console.WriteLine(json);

            var data2 = JsonSerializer.Deserialize<OrderCreated>(json);
            var e2 = EventMapping.Convert2(data2);

            System.Console.WriteLine(JsonSerializer.Serialize(e, e.GetType(), new JsonSerializerOptions { WriteIndented = true }));

            return;

            var eventStore = new EventStore();

            var orderRepository = new Repository<Order, OrderIdentity>(eventStore);

            //var order = orderRepository.Load(OrderIdentity.New());
            var order = Order.Create();

            order.ChangeOrderCustomerName("Ilonka");





            var orderLineIdentity = order.CreateOrderLine("Brood", 1);

            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

            orderRepository.Save(order);


            order.AdjustOrderLineQuantity(orderLineIdentity, 10);


            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

            orderRepository.Save(order);


            order.AdjustOrderLineQuantity(orderLineIdentity, 10);
            orderRepository.Save(order);


            order.RemoveOrderLine(orderLineIdentity);
            order.CreateOrderLine("Brood2", 1);

            orderRepository.Save(order);
            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

        }
    }
}
