using System.Text.Json;
using DDD.Core.OrderManagement.BDD;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Commands;
using DDD.Core.OrderManagement.Products;
using DDD.Core.OrderManagement.Products.Commands;
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
            var productRepository = new ProductRepository(eventStore);

            var product = productRepository.Create(ProductName.Create("Brood"));

            productRepository.Save(product);

            product.ChangeProductName(ProductName.Create("Boterham"));
            productRepository.Save(product);


            var productRepository2 = new ProductRepository(eventStore);
            
            var product2 = productRepository2.Get(product.Identity);

            System.Console.WriteLine(JsonSerializer.Serialize(product, new JsonSerializerOptions { WriteIndented = true }));
            System.Console.WriteLine(JsonSerializer.Serialize(product2, new JsonSerializerOptions { WriteIndented = true }));



            var orderRepository = new OrderRepository(eventStore);

            var order = orderRepository.Create();

            orderRepository.Save(order);

            var order2 = orderRepository.Get(order.Identity);


            order.ChangeOrderCustomerName("Ilonka");





            var orderLineIdentity = order.CreateOrderLine(ProductIdentity.Parse( "Brood"), 1);

            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

            orderRepository.Save(order);


            order.AdjustOrderLineQuantity(orderLineIdentity, 10);


            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

            orderRepository.Save(order);


            order.AdjustOrderLineQuantity(orderLineIdentity, 10);
            orderRepository.Save(order);


            order.RemoveOrderLine(orderLineIdentity);
            order.CreateOrderLine(ProductIdentity.Parse("Brood2"), 1);

            orderRepository.Save(order);
            System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

        }
    }
}
