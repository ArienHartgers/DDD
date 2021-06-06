using System;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identifiers;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class Order : AggregateRoot
    {
        private readonly OrderLineCollection _orderLines;

        private Order(IEventContext context, OrderCreated initialEvent)
        {
            _orderLines = new OrderLineCollection(this);

            Created = context.EventDateTime;
            LastUpdate = context.EventDateTime;

            OrderIdentifier = initialEvent.OrderIdentifier;
            CustomerIdentifier = initialEvent.CustomerIdentifier;
            CustomerName = initialEvent.CustomerIdentifier.ToString();

            RegisterEvent<OrderCustomerNameChanged>(OrderCustomerNameChangedEventHandler);

            // OrderLines
            RegisterEvent<OrderLineCreated>(OrderLineCreatedEventHandler);
            RegisterEvent<OrderLineRemoved>(OrderLineRemovedEventHandler);
            RegisterEvent<OrderLineQuantityAdjusted>(e => e.ForwardTo(_orderLines.Get(e.Event.OrderLineIdentifier)));
        }

        public OrderIdentifier OrderIdentifier { get; }
    
        public DateTimeOffset Created { get; private set; }

        public CustomerIdentifier CustomerIdentifier { get; }

        public DateTimeOffset LastUpdate { get; private set; }

        public string CustomerName { get; private set; }

        public IOrderLineCollection Lines => _orderLines;

        public override IIdentifier GetIdentifier() => OrderIdentifier;

        private void OrderCustomerNameChangedEventHandler(HandlerEvent<OrderCustomerNameChanged> handlerEvent)
        {
            CustomerName = handlerEvent.Event.CustomerName;
            LastUpdate = handlerEvent.EventDateTime;
        }

        private void OrderLineCreatedEventHandler(HandlerEvent<OrderLineCreated> handlerEvent)
        {
            var orderLine = new OrderLine(this, handlerEvent);
            _orderLines.Add(orderLine.Identifier, orderLine);
        }

        private void OrderLineRemovedEventHandler(HandlerEvent<OrderLineRemoved> handlerEvent)
        {
            _orderLines.Remove(handlerEvent.Event.OrderLineIdentifier);
        }
    }
}