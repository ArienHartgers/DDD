using System;
using System.Linq;
using System.Text.Json;
using DDD.App.Events;
using DDD.Core;
using DDD.Core.OrderManagement.BDD;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products;
using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.App
{
    class Program
    {
        static void Main()
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

            IEventStore eventStore = new TestEventStore();
            var productRepository = new ProductRepository(eventStore);

            var product = Product.Create(DateTimeOffset.Now, ProductName.Create("Brood"));

            productRepository.Save(product);

            product.ChangeName(ProductName.Create("Boterham"));
            productRepository.Save(product);



            var productRepository2 = new ProductRepository(eventStore);
            
            var product2 = productRepository2.Get(product.Identity);

            //System.Console.WriteLine(JsonSerializer.Serialize(product, new JsonSerializerOptions { WriteIndented = true }));
            //System.Console.WriteLine(JsonSerializer.Serialize(product2, new JsonSerializerOptions { WriteIndented = true }));




            var order = Order.Create(
                DateTimeOffset.Now,
                OrderIdentity.New(), 
                CustomerIdentity.New());

            var faultyLine = order.CreateOrderLine(ProductIdentity.Parse("Fout"), 1);
            faultyLine.Remove();
            
            
            order.CreateOrderLine(ProductIdentity.Parse("Brood"), 1);
            order.CreateOrderLine(ProductIdentity.Parse("Boter"), 1);
            order.CreateOrderLine(ProductIdentity.Parse("Pindakaas"), 1);
            order.CreateOrderLine(ProductIdentity.Parse("Boterhamworst"), 1);

            var pindaKaasOrderLine = order.Lines.Get(ProductIdentity.Parse("Pindakaas"));
            pindaKaasOrderLine.Remove();

            var boterOrderLine = order.Lines.Get(ProductIdentity.Parse("Boter"));
            boterOrderLine.AdjustQuantity(3);


            var orderRepository = new OrderRepository(eventStore);
            orderRepository.Save(order);

            var order2 = orderRepository.Get(order.Identity);

            var streamEvents = eventStore.GetStreamEvents(product.Identity.Identity);
            foreach (var loadedEvent in streamEvents.Events)
            {
                if (loadedEvent.Data is Event @event)
                {
                    if (eventsConverter.TryConvert(@event, out var domainEvent))
                    {
                        System.Console.WriteLine($"{loadedEvent.EventDateTime} {domainEvent.GetType().Name}");
                        System.Console.WriteLine(JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), new JsonSerializerOptions { WriteIndented = true }));
                        //System.Console.WriteLine(JsonSerializer.Serialize(@event, @event.GetType(), new JsonSerializerOptions { WriteIndented = true }));
                    }
                }
            }


            order.ChangeCustomerName("Ilonka");





            var orderLine = order.CreateOrderLine(ProductIdentity.Parse( "Brood"), 1);

            //System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

            orderRepository.Save(order);

            var orderLine1 = order.Lines.Get(orderLine.Identity);
            orderLine1.AdjustQuantity(10);


            //System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

            orderRepository.Save(order);

            var orderLine2 = order.Lines.Get(orderLine.Identity);
            orderLine2.AdjustQuantity(10);

            orderRepository.Save(order);

            var orderLine3 = order.Lines.Get(orderLine.Identity);
            orderLine3.Remove();

            order.CreateOrderLine(ProductIdentity.Parse("Brood2"), 1);

            orderRepository.Save(order);
            //System.Console.WriteLine(JsonSerializer.Serialize(order, new JsonSerializerOptions { WriteIndented = true }));

        }
    }
}
