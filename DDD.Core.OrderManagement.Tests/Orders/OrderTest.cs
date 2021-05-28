using System;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;
using DDD.Core.OrderManagement.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DDD.Core.OrderManagement.Tests.Orders
{
    [TestClass]
    public class OrderTest : AggregateTester<Order, OrderIdentifier>
    {
        private readonly CustomerIdentifier _customerIdentifier;

        public OrderTest()
            : base(OrderIdentifier.New())
        {
            _customerIdentifier = CustomerIdentifier.New();
        }


        [TestMethod]
        public void Tst001_CreateOrder()
        {
            When(Order.Create(
                AggregateContext,
                Identifier,
                _customerIdentifier));

            Then<OrderCreated>(e =>
            {
                e.OrderIdentifier.ShouldBe(Identifier);
                e.CustomerIdentifier.ShouldBe(_customerIdentifier);
            });
        }

        [TestMethod]
        public void Tst002_ChangeCustomerName()
        {
            var newName = "newName";

            Given(new OrderCreated(Identifier, _customerIdentifier));

            When().ChangeCustomerName(newName);

            ThenAggregate(a =>
            {
                a.Created.ShouldBe(GivenDateTime);
                a.LastUpdate.ShouldBe(WhenDateTime);
            });

            Then<OrderCustomerNameChanged>(e =>
            {
                e.OrderIdentifier.ShouldBe(Identifier);
                e.CustomerName.ShouldBe(newName);
            });
        }

        [TestMethod]
        public void Tst003_ChangeCustomerName()
        {
            var newName = "newName";
            var time1 = WhenDateTime;
            var time2 = time1.AddDays(1);

            When(Order.Create(
                AggregateContext,
                Identifier,
                _customerIdentifier));

            WhenDateTime = time2;

            When().ChangeCustomerName(newName);

            ThenAggregate(a =>
            {
                a.Created.ShouldBe(time1);
                a.LastUpdate.ShouldBe(time2);
            });

            Then<OrderCreated>(e =>
            {
                e.OrderIdentifier.ShouldBe(Identifier);
                e.CustomerIdentifier.ShouldBe(_customerIdentifier);
            });

            Then<OrderCustomerNameChanged>(e =>
            {
                e.OrderIdentifier.ShouldBe(Identifier);
                e.CustomerName.ShouldBe(newName);
            });
        }

        [TestMethod]
        public void Tst004_ChangeCustomerName_SameName_Nothing()
        {
            var newName = "newName";

            Given(new OrderCreated(Identifier, _customerIdentifier));
            Given(new OrderCustomerNameChanged(Identifier, newName));

            When().ChangeCustomerName(newName);

            ThenNothing();
        }

        [TestMethod]
        public void Tst005_ChangeCustomerName_SameName_Nothing()
        {
            var newName = "newName1";

            Given(new OrderCreated(Identifier, _customerIdentifier));

            When().ChangeCustomerName(newName);
            When().ChangeCustomerName(newName);

            Then<OrderCustomerNameChanged>();
        }

        [TestMethod]
        public void Tst011_AddOrderLine()
        {
            var productIdentifier = ProductIdentifier.New();
            var quantity = 2;

            Given(new OrderCreated(Identifier, _customerIdentifier));

            var orderline = When().CreateOrderLine(productIdentifier, quantity);


            Then<OrderLineCreated>(e =>
            {
                e.OrderIdentifier.ShouldBe(Identifier);
                e.OrderLineIdentifier.ShouldBe(OrderLineIdentifier.Create(1));
                e.ProductIdentifier.ShouldBe(productIdentifier);
                e.Quantity.ShouldBe(quantity);
            });
        }

        [TestMethod]
        public void Tst012_AddOrderLine()
        {
            var productIdentifier = ProductIdentifier.New();
            var quantity = 2;
            var name = ProductName.Create("Test");

            Given(new OrderCreated(Identifier, _customerIdentifier));
            Given(new OrderLineCreated(Identifier, OrderLineIdentifier.Create(1), productIdentifier, name, quantity));

            Should.Throw<InvalidOperationException>(() => When().CreateOrderLine(productIdentifier, quantity));

            ThenNothing();
        }

        [TestMethod]
        public void Tst021_AdjustOrderLine()
        {
            var orderLineIdentifier = OrderLineIdentifier.Create(1);
            var productIdentifier = ProductIdentifier.New();
            var name = ProductName.Create("Test");
            var oldQuantity = 2;
            var newQuantity = 5;

            Given(new OrderCreated(Identifier, _customerIdentifier));
            Given(new OrderLineCreated(Identifier, orderLineIdentifier, productIdentifier, name, oldQuantity));

            When().AdjustOrderLine(orderLineIdentifier, newQuantity);

            Then<OrderLineQuantityAdjusted>(e =>
            {
                e.OrderIdentifier.ShouldBe(Identifier);
                e.OrderLineIdentifier.ShouldBe(OrderLineIdentifier.Create(1));
                e.Quantity.ShouldBe(newQuantity);
            });
        }

        [TestMethod]
        public void Tst022_AdjustOrderLine()
        {
            var orderLineIdentifier = OrderLineIdentifier.Create(1);
            var quantity = 5;

            Given(new OrderCreated(Identifier, _customerIdentifier));

            var exception = Should.Throw<Exception>(() => When().AdjustOrderLine(orderLineIdentifier, quantity));
            exception.Message.ShouldContain("not found");

            ThenNothing();
        }
    }
}