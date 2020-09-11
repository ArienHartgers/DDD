using System;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class Order : AggregateRoot<OrderIdentity>
    {
        private readonly OrderLineCollection _orderLines = new OrderLineCollection();

        private Order(TypedEvent<OrderCreated> initialEvent)
        {
            Created = initialEvent.EventDateTime;
            LastUpdate = initialEvent.EventDateTime;

            Identity = initialEvent.Event.OrderIdentity;
            CustomerIdentity = initialEvent.Event.CustomerIdentity;
            CustomerName = initialEvent.Event.CustomerIdentity.CustomerGuid.ToString();

            RegisterEvent<OrderCustomerNameChanged>(OrderCustomerNameChangedEventHandler);

            // OrderLines
            RegisterEvent<OrderLineCreated>(OrderLineCreatedEventHandler);
            RegisterEvent<OrderLineRemoved>(OrderLineRemovedEventHandler);
            RegisterEvent<OrderLineQuantityAdjusted>(e => e.ForwardTo(_orderLines.Get(e.Event.OrderLineIdentity)));
        }

        public OrderIdentity Identity { get; }
    
        public DateTimeOffset Created { get; }

        public CustomerIdentity CustomerIdentity { get; }

        public DateTimeOffset LastUpdate { get; private set; }

        public string CustomerName { get; private set; }

        public IOrderLineCollection Lines => _orderLines;

        public override OrderIdentity GetIdentity() => Identity;

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
            _orderLines.Remove(handlerEvent.Event.OrderLineIdentity);
        }
    }
}