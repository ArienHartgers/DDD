using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identitfiers;
using DDD.Core.OrderManagement.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DDD.Core.OrderManagement.Tests.Orders.Entities
{
    [TestClass]
    public class OrderTest
    {
        private readonly AggregateTester<OrderRepository> _tester;
        private readonly OrderIdentifier _orderIdentifier;
        private readonly CustomerIdentifier _customerIdentifier;

        public OrderTest()
        {
            _tester = new AggregateTester<OrderRepository>(eventStore => new OrderRepository(eventStore));

            _orderIdentifier = OrderIdentifier.New();
            _customerIdentifier = CustomerIdentifier.New();
        }

        [TestMethod]
        public void CreateOrder()
        {

            _tester.Run(
                new LoadedEvent[0],
                when => Order.Create(
                    DateTimeOffset.Now,
                    _orderIdentifier,
                    _customerIdentifier),
                then=>
                {
                    then.Events.Count.ShouldBe(1);
                    var loadedEvent = then.Events.First();
                    var e = (OrderCreated) loadedEvent.Data;
                    e.OrderIdentifier.ShouldBe(_orderIdentifier);
                    e.CustomerIdentifier.ShouldBe(_customerIdentifier);
                }
            );

        }

        [TestMethod]
        public void ChangeCustomerName()
        {

            _tester.Run(
                new LoadedEvent[]
                {
                    new LoadedEvent(DateTimeOffset.Now, 
                        new OrderCreated(_orderIdentifier, CustomerIdentifier.New())), 
                },
                when =>
                {
                    var order = when.Repository.Get(_orderIdentifier);
                    order.ChangeCustomerName("Changed");
                    return order;
                },
                then =>
                {
                    then.Events.Count.ShouldBe(1);
                    var loadedEvent = then.Events.First();
                    var e = (OrderCreated)loadedEvent.Data;
                    e.OrderIdentifier.ShouldBe(_orderIdentifier);
                    e.CustomerIdentifier.ShouldBe(_customerIdentifier);
                }
            );

        }

    }
}