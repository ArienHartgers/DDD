using System.Text.Json;
using DDD.Core;
using DDD.Core.OrderManagement.BDD;
using DDD.Core.OrderManagement.Orders.Commands;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Commands;
using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.App
{
    class Program
    {
        static void Main()
        {

            //var e = new Core.OrderManagement.Orders.Events.OrderCreated(OrderIdentity.New());
            //System.Console.WriteLine(JsonSerializer.Serialize(e, new JsonSerializerOptions { WriteIndented = true }));


            //var a = EventMapping.Convert(e);
            //var json = JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = true, Converters = { new IdentityJsonConverter() } });

            //System.Console.WriteLine(json);

            //var data2 = JsonSerializer.Deserialize<OrderCreated>(json);
            //var e2 = EventMapping.Convert2(data2);

            //System.Console.WriteLine(JsonSerializer.Serialize(e, e.GetType(), new JsonSerializerOptions { WriteIndented = true }));

            //return;

            var eventStore = new TestEventStore();

            var product = Product.Create(ProductName.Create("Brood"));

            var productRepository = new Repository<Product, ProductIdentity>(eventStore);
            productRepository.Save(product);

            product.ChangeProductName(ProductName.Create("Boterham"));
            productRepository.Save(product);


            var productRepository2 = new Repository<Product, ProductIdentity>(eventStore);
            
            var product2 = productRepository2.Get(product.Identity);

            System.Console.WriteLine(JsonSerializer.Serialize(product, new JsonSerializerOptions { WriteIndented = true }));
            System.Console.WriteLine(JsonSerializer.Serialize(product2, new JsonSerializerOptions { WriteIndented = true }));



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
