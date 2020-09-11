using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DDD.Core.OrderManagement.Tests.Orders.Entities
{
    [TestClass]
    public class OrderTest
    {
        private readonly AggregateTester<OrderRepository> _tester;
        private readonly OrderIdentity _orderIdentity;
        private readonly CustomerIdentity _customerIdentity;

        public OrderTest()
        {
            _tester = new AggregateTester<OrderRepository>(eventStore => new OrderRepository(eventStore));

            _orderIdentity = OrderIdentity.New();
            _customerIdentity = CustomerIdentity.New();
        }

        [TestMethod]
        public void CreateOrder()
        {

            _tester.Run(
                new LoadedEvent[0],
                when => Order.Create(
                    DateTimeOffset.Now,
                    _orderIdentity,
                    _customerIdentity),
                then=>
                {
                    then.Events.Count.ShouldBe(1);
                    var loadedEvent = then.Events.First();
                    var e = (OrderCreated) loadedEvent.Data;
                    e.OrderIdentity.ShouldBe(_orderIdentity);
                    e.CustomerIdentity.ShouldBe(_customerIdentity);
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
                        new OrderCreated(_orderIdentity, CustomerIdentity.New())), 
                },
                when =>
                {
                    var order = when.Repository.Get(_orderIdentity);
                    order.ChangeCustomerName("Changed");
                    return order;
                },
                then =>
                {
                    then.Events.Count.ShouldBe(1);
                    var loadedEvent = then.Events.First();
                    var e = (OrderCreated)loadedEvent.Data;
                    e.OrderIdentity.ShouldBe(_orderIdentity);
                    e.CustomerIdentity.ShouldBe(_customerIdentity);
                }
            );

        }

    }
}