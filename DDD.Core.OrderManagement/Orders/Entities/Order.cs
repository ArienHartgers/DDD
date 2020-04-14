using System;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public class Order : AggregateRoot<OrderIdentity>
    {
        private readonly EntityCollection<OrderLine, OrderLineIdentity> _orderLines = new EntityCollection<OrderLine, OrderLineIdentity>();

        private Order(TypedEvent<OrderCreated> initialEvent)
        {
            Created = initialEvent.EventDateTime;
            LastUpdate = initialEvent.EventDateTime;

            Identity = initialEvent.Event.OrderIdentity;
            CustomerIdentity = initialEvent.Event.CustomerIdentity;
            CustomerName = initialEvent.Event.CustomerIdentity.CustomerGuid.ToString();

            RegisterEvent<OrderCustomerNameChangedEvent>(OrderCustomerNameChangedEventHandler);

            // OrderLines
            RegisterEvent<OrderLineCreatedEvent>(OrderLineCreatedEventHandler);
            RegisterEvent<OrderLineRemovedEvent>(OrderLineRemovedEventHandler);
            RegisterEvent<OrderLineQuantityAdjustedEvent>(e => e.ForwardTo(_orderLines.Get(e.Event.OrderLineIdentity)));
        }

        public override OrderIdentity Identity { get; }
    
        public DateTimeOffset Created { get; private set; }

        public CustomerIdentity CustomerIdentity { get; }

        public DateTimeOffset LastUpdate { get; private set; }

        public string CustomerName { get; private set; }

        public IEntityCollection<OrderLine, OrderLineIdentity> OrderLines => _orderLines;

        public OrderLine? FindOrderLine(OrderLineIdentity id)
        {
            return _orderLines.Find(id);
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