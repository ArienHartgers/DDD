using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Core.OrderManagement.Orders;
using DDD.Core.OrderManagement.Orders.Events;
using TechTalk.SpecFlow;
using DDD.Core.OrderManagement.Orders.Commands;
using Shouldly;

namespace DDD.Core.OrderManagement.BDD
{
    [Binding]
    public class DDDSteps
    {
        // Common
        private readonly Guid _orderGuid;

        // Given
        private EventStore? _eventStore;

        // When
        private Order? _order;

        // Then
        private Stack<LoadedEvent>? _eventStack;

        public DDDSteps()
        {
            _eventStore = new EventStore();
            _orderGuid = Guid.NewGuid();
        }

        [Given(@"I have an order")]
        public void GivenIHaveAnOrder()
        {
            AddEvent(new OrderCreatedEvent(_orderGuid));
        }

        [Given(@"order has an item product (.*) whith quantity (.*)")]
        public void GivenOrderHasAnItemProductWhithQuantity(string product, int quantity)
        {
            AddEvent(new OrderLineCreatedEvent
            {
                LineId = 1,
                ProductName = product,
                Quantity = quantity,
            });
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
            order.CreateOrderLine(product, quantity);
        }

        [When(@"I change quantity to (.*) from orderline with id (.*)")]
        public void WhenIChangeQuantityToFromOrderlineWithId(int quantity, int id)
        {
            var orderLineIdentity = new OrderLineIdentity(id);
            var order = GetOrder();
            order.AdjustOrderLineQuantity(orderLineIdentity, quantity);
        }

        [When(@"I remove line with identity (.*) from order")]
        public void WhenIRemoveLineWithIdentityFromOrder(int id)
        {
            var order = GetOrder();
            order.RemoveOrderLine(new OrderLineIdentity(id));
        }

        [Then("No Result is expected")]
        public void ThenNoResultIsExpected()
        {
            Then();
        }

        [Then("Order is created")]
        public void ThenOrderIsCreated()
        {
            var orderCreatedEvent = ThenGetEvent<OrderCreatedEvent>();
            orderCreatedEvent.ShouldNotBeNull();
            //orderCreatedEvent.Id.ShouldBe(_orderGuid);
        }

        [Then(@"Product (.*) is added with an quantity of (.*)")]
        public void ThenProductPepperIsAddedWithAnQuantityOf(string product, int count)
        {
            var orderLineCreatedEvent = ThenGetEvent<OrderLineCreatedEvent>();
            orderLineCreatedEvent.ShouldNotBeNull();
            orderLineCreatedEvent.ProductName.ShouldBe(product);
            orderLineCreatedEvent.Quantity.ShouldBe(count);
        }

        [Then(@"itemline (.*) is removed")]
        public void ThenItemlineIsRemoved(int id)
        {
            var orderLineCreatedEvent = ThenGetEvent<OrderLineRemovedEvent>();
            orderLineCreatedEvent.ShouldNotBeNull();
            orderLineCreatedEvent.LineId.ShouldBe(id);
        }

        [Then(@"itemline (.*) quantity is changed to (.*)")]
        public void ThenItemlineQuantityIsChangedTo(int id, int quantity)
        {
            var orderLineQuantityAdjustedEvent = ThenGetEvent<OrderLineQuantityAdjustedEvent>();
            orderLineQuantityAdjustedEvent.ShouldNotBeNull();
            orderLineQuantityAdjustedEvent.OrderId.OrderGuid.ShouldBe(_orderGuid);
            orderLineQuantityAdjustedEvent.OrderLineIdentity.LineId.ShouldBe(id);
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

            _eventStore.SaveEvents(_orderGuid.ToString(), new LoadedEvent(DateTimeOffset.Now, @event));
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
            _order = Order.Create();
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

                var orderRepository = new Repository<Order, OrderIdentity>(_eventStore);
                _order = orderRepository.Load(_orderGuid);
                _eventStore = null;
            }

            return _order;
        }
    }
}
