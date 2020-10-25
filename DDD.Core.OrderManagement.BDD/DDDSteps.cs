using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Core.OrderManagement.Orders;
using TechTalk.SpecFlow;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Entities;
using DDD.Core.OrderManagement.Orders.Identitfiers;
using DDD.Core.OrderManagement.Products.Identities;
using Shouldly;

namespace DDD.Core.OrderManagement.BDD
{
    [Binding]
    public class DDDSteps
    {
        // Common
        private readonly OrderIdentifier _orderIdentifier;

        // Given
        private TestEventStore? _eventStore;

        // When
        private Order? _order;

        // Then
        private Stack<LoadedEvent>? _eventStack;

        public DDDSteps()
        {
            _eventStore = new TestEventStore();
            _orderIdentifier = OrderIdentifier.New();
        }

        [Given(@"I have an order")]
        public void GivenIHaveAnOrder()
        {
            AddEvent(new OrderCreated(_orderIdentifier, CustomerIdentifier.New()));
        }

        [Given(@"order has an item product (.*) whith quantity (.*)")]
        public void GivenOrderHasAnItemProductWithQuantity(string product, int quantity)
        {
            AddEvent(new OrderLineCreated(
                _orderIdentifier,
                OrderLineIdentifier.Create(1),
                ProductIdentifier.Parse(product),
                quantity));
        }

        [When(@"I create an order")]
        public void WhenICreateAnOrder()
        {
            CreateOrder();
        }

        [When(@"I add (.*) items of product (.*) to the order")]
        public void WhenIAddItemOneToTheOrder(int quantity, string product)
        {
            var order = GetOrder();
            order.CreateOrderLine(ProductIdentifier.Parse(product), quantity);
        }

        [When(@"I change quantity to (.*) from orderline with id (.*)")]
        public void WhenIChangeQuantityToFromOrderlineWithId(int quantity, string id)
        {
            var orderLineIdentity = OrderLineIdentifier.Create(id);
            var order = GetOrder();
            var orderLine = order.Lines.Get(orderLineIdentity);
            orderLine.AdjustQuantity(quantity);
        }

        [When(@"I remove line with identity (.*) from order")]
        public void WhenIRemoveLineWithIdentityFromOrder(string id)
        {
            var order = GetOrder();
            var orderLine = order.Lines.Get(OrderLineIdentifier.Create(id));
            orderLine.Remove();
        }

        [Then("No Result is expected")]
        public void ThenNoResultIsExpected()
        {
            Then();
        }

        [Then("Order is created")]
        public void ThenOrderIsCreated()
        {
            var orderCreatedEvent = ThenGetEvent<OrderCreated>();
            orderCreatedEvent.ShouldNotBeNull();
            //orderCreatedEvent.Id.ShouldBe(_orderIdentity);
        }

        [Then(@"Product (.*) is added with an quantity of (.*)")]
        public void ThenProductPepperIsAddedWithAnQuantityOf(string product, int count)
        {
            var orderLineCreatedEvent = ThenGetEvent<OrderLineCreated>();
            orderLineCreatedEvent.ShouldNotBeNull();
            orderLineCreatedEvent.ProductIdentifier.ShouldBe(ProductIdentifier.Parse(product));
            orderLineCreatedEvent.Quantity.ShouldBe(count);
        }

        [Then(@"itemline (.*) is removed")]
        public void ThenItemlineIsRemoved(int id)
        {
            var orderLineCreatedEvent = ThenGetEvent<OrderLineRemoved>();
            orderLineCreatedEvent.ShouldNotBeNull();
            orderLineCreatedEvent.OrderLineIdentifier.LineId.ShouldBe(id);
        }

        [Then(@"itemline (.*) quantity is changed to (.*)")]
        public void ThenItemlineQuantityIsChangedTo(int id, int quantity)
        {
            var orderLineQuantityAdjustedEvent = ThenGetEvent<OrderLineQuantityAdjusted>();
            orderLineQuantityAdjustedEvent.ShouldNotBeNull();
            orderLineQuantityAdjustedEvent.OrderIdentifier.ShouldBe(_orderIdentifier);
            orderLineQuantityAdjustedEvent.OrderLineIdentifier.LineId.ShouldBe(id);
            orderLineQuantityAdjustedEvent.Quantity.ShouldBe(quantity);
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            _eventStack.ShouldNotBeNull("No THEN statements");
            _eventStack.ShouldBeEmpty("Not all events are checked");
        }

        private void AddEvent(Event @event)
        {
            if (_eventStore == null)
            {
                throw new Exception("Add event can only be used in 'Given' mode");
            }

            _eventStore.AddTestEvents(_orderIdentifier.ToString(), new LoadedEvent(DateTimeOffset.Now, @event));
        }

        private void Then()
        {
            if (_eventStack == null)
            {
                IAggregateLoader loader = GetOrder();
                var uncommitedEvents = loader.GetUncommittedChanges();
                _eventStack = new Stack<LoadedEvent>(uncommitedEvents.Reverse());
            }
        }

        private T ThenGetEvent<T>()
        {
            Then();

            var loadedEvent = _eventStack?.Pop();
            if (loadedEvent == null)
            {
                throw new Exception("No more event");
            }

            if (loadedEvent.Data is T expectedEvent)
            {
                return expectedEvent;
            }

            throw new Exception("Event is not of expected type");
        }

        private Order CreateOrder()
        {
            if (_eventStore == null)
            {
                throw new Exception("Event store is null");
            }

            var orderRepository = new OrderRepository(_eventStore);

            _order = Order.Create(
                DateTimeOffset.Now,
                OrderIdentifier.New(), 
                CustomerIdentifier.New());

            return _order;
        }

        private Order GetOrder()
        {
            if (_order == null)
            {
                if (_eventStore == null)
                {
                    throw new Exception("Event store is null");
                }

                var orderRepository = new OrderRepository(_eventStore);
                _order = orderRepository.Get(_orderIdentifier);
                _eventStore = null;
            }

            return _order;
        }
    }
}
