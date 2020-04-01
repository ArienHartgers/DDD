using System;
using System.Text.Json;
using DDD.Core;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Commands;

namespace DDD
{
    class Program
    {
        static void Main()
        {
            var eventStore = new EventStore();

            var orderRepository = new Repository<Order, OrderIdentity>(eventStore);

            var order = orderRepository.Load(Guid.NewGuid());

            order.ChangeOrderCustomerName("Ilonka");





            var orderLineIdentity = order.CreateOrderLine("Brood", 1);

            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions {WriteIndented = true}));

            orderRepository.Save(order);


            order.AdjustOrderLineQuantity(orderLineIdentity, 10);


            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions {WriteIndented = true}));

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
