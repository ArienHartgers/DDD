using System;
using System.Text.Json;
using System.Threading.Tasks;
using DDD.Adapters.Store;
using DDD.Core;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products;
using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;
using DDD.EventConverter;

namespace DDD.App
{
    class Program
    {
        static async Task Main()
        {
            var eventsConverter = new EventsConverter();

            //var e = new Core.OrderManagement.Orders.Events.OrderCreated(OrderIdentity.New());
            //System.Console.WriteLine(JsonSerializer.Serialize(e, new JsonSerializerOptions { WriteIndented = true }));


            //var a = EventMapping.Convert(e);
            //var json = JsonSerializer.Serialize(a, new JsonSerializerOptions { WriteIndented = true, Converters = { new IdentityJsonConverter() } });

            //System.Console.WriteLine(json);

            //var data2 = JsonSerializer.Deserialize<OrderCreated>(json);
            //var e2 = EventMapping.Convert2(data2);

            //System.Console.WriteLine(JsonSerializer.Serialize(e, e.GetType(), new JsonSerializerOptions { WriteIndented = true }));

            //return;

            var aggregateContext = new AggregateContext();

            IEventStore eventStore = new EventStoreDb(eventsConverter, new Uri("http://localhost:2113"));
            var productRepository = new ProductRepository(eventStore, aggregateContext);

            var product1 = Product.Create(aggregateContext, ProductName.Create("Brood"));

            await productRepository.SaveAsync(product1);

            product1.ChangeName(ProductName.Create("Boterham"));
            await productRepository.SaveAsync(product1);



            var productRepository2 = new ProductRepository(eventStore, aggregateContext);
            
            var product1A = await productRepository2.GetAsync(product1.Identifier);

            //System.Console.WriteLine(JsonSerializer.Serialize(product, new JsonSerializerOptions { WriteIndented = true }));
            //System.Console.WriteLine(JsonSerializer.Serialize(product2, new JsonSerializerOptions { WriteIndented = true }));


            var orderRepository = new OrderRepository(eventStore, aggregateContext);


            var order = Order.Create(
                aggregateContext,
                OrderIdentifier.New(), 
                CustomerIdentifier.New());

            var faultyLine = order.CreateOrderLine(ProductIdentifier.New(), 1);
            faultyLine.Remove();

            var prod2 = ProductIdentifier.New();
            var prod3 = ProductIdentifier.New();
            var prod4 = ProductIdentifier.New();


            order.CreateOrderLine(product1, 1);
            order.CreateOrderLine(prod2, 2);
            order.CreateOrderLine(prod3, 1);
            order.CreateOrderLine(prod4, 1);

            var prod3OrderLine = order.Lines.Get(prod3);
            prod3OrderLine.Remove();

            var prod1OrderLine = order.Lines.Get(product1.Identifier);
            prod1OrderLine.AdjustQuantity(3);


            await orderRepository.SaveAsync(order);

            var order2 = await orderRepository.GetAsync(order.OrderIdentifier);


            var product1B = await productRepository2.GetAsync(product1.Identifier);
            product1B.ChangeName(ProductName.Create("Stok brood"));
            await productRepository2.SaveAsync(product1B);

        }
    }
}
