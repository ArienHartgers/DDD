using System;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identitfiers;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class Order : AggregateRoot<OrderIdentifier>
    {
        private readonly OrderLineCollection _orderLines = new OrderLineCollection();

        private Order(TypedEvent<OrderCreated> initialEvent)
        {
            Created = initialEvent.EventDateTime;
            LastUpdate = initialEvent.EventDateTime;

            OrderIdentifier = initialEvent.Event.OrderIdentifier;
            CustomerIdentifier = initialEvent.Event.CustomerIdentifier;
            CustomerName = initialEvent.Event.CustomerIdentifier.ToString();

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

        public override OrderIdentifier GetIdentifier() => OrderIdentifier;

        private void OrderCustomerNameChangedEventHandler(HandlerEvent<OrderCustomerNameChanged> handlerEvent)
        {
            CustomerName = handlerEvent.Event.CustomerName;
            LastUpdate = handlerEvent.EventDateTime;
        }

        private void OrderLineCreatedEventHandler(HandlerEvent<OrderLineCreated> handlerEvent)
        {
            var orderLine = new OrderLine(this, handlerEvent);
            _orderLines.Add(orderLine);
        }

        private void OrderLineRemovedEventHandler(HandlerEvent<OrderLineRemoved> handlerEvent)
        {
            _orderLines.Remove(handlerEvent.Event.OrderLineIdentifier);
        }
    }
}