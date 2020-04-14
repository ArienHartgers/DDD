using System;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public class Order : AggregateRoot<OrderIdentity>
    {
        private readonly EntityCollection<OrderLine, OrderLineIdentity> _orderLines = new EntityCollection<OrderLine, OrderLineIdentity>();
        private OrderIdentity _identity;

        private Order()
        {
            RegisterEvent<OrderCreated>(OrderCreatedEventHandler);
            RegisterEvent<OrderCustomerNameChangedEvent>(OrderCustomerNameChangedEventHandler);

            // OrderLines
            RegisterEvent<OrderLineCreatedEvent>(OrderLineCreatedEventHandler);
            RegisterEvent<OrderLineRemovedEvent>(OrderLineRemovedEventHandler);
            RegisterEvent<OrderLineQuantityAdjustedEvent>(e => e.ForwardTo(_orderLines.Get(e.Event.OrderLineIdentity)));
        }

        public override OrderIdentity Identity => _identity ?? throw new EntityNotInitializedException(nameof(Order));

        public DateTimeOffset Created { get; private set; }

        public DateTimeOffset LastUpdate { get; private set; }

        public string CustomerName { get; private set; } = null!;

        public IEntityCollection<OrderLine, OrderLineIdentity> OrderLines => _orderLines;

        public OrderLine? FindOrderLine(OrderLineIdentity id)
        {
            return _orderLines.Find(id);
        }


        public static Order Create()
        {
            var order = new Order();
            order.ApplyChange(new OrderCreated(OrderIdentity.New()));

            return order;
        }

        private void OrderCreatedEventHandler(HandlerEvent<OrderCreated> handlerEvent)
        {
            _identity = handlerEvent.Event.OrderIdentity;
            Created = handlerEvent.EventDateTime;
            LastUpdate = handlerEvent.EventDateTime;
        }

        private void OrderCustomerNameChangedEventHandler(HandlerEvent<OrderCustomerNameChangedEvent> handlerEvent)
        {
            CustomerName = handlerEvent.Event.CustomerName;
            LastUpdate = handlerEvent.EventDateTime;
        }

        private void OrderLineCreatedEventHandler(HandlerEvent<OrderLineCreatedEvent> handlerEvent)
        {
            var orderLine = new OrderLine();
            handlerEvent.ForwardTo(orderLine);
            _orderLines.Add(orderLine);
        }

        private void OrderLineRemovedEventHandler(HandlerEvent<OrderLineRemovedEvent> handlerEvent)
        {
            _orderLines.Remove(handlerEvent.Event.OrderLineIdentity);
        }

    }
}