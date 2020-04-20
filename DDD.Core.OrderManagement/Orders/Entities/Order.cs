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

            RegisterEvent<OrderCustomerNameChanged>(OrderCustomerNameChangedEventHandler);

            // OrderLines
            RegisterEvent<OrderLineCreated>(OrderLineCreatedEventHandler);
            RegisterEvent<OrderLineRemoved>(OrderLineRemovedEventHandler);
            RegisterEvent<OrderLineQuantityAdjusted>(e => e.ForwardTo(_orderLines.Get(e.Event.OrderLineIdentity)));
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

        private void OrderCustomerNameChangedEventHandler(HandlerEvent<OrderCustomerNameChanged> handlerEvent)
        {
            CustomerName = handlerEvent.Event.CustomerName;
            LastUpdate = handlerEvent.EventDateTime;
        }

        private void OrderLineCreatedEventHandler(HandlerEvent<OrderLineCreated> handlerEvent)
        {
            var orderLine = new OrderLine();
            handlerEvent.ForwardTo(orderLine);
            _orderLines.Add(orderLine);
        }

        private void OrderLineRemovedEventHandler(HandlerEvent<OrderLineRemoved> handlerEvent)
        {
            _orderLines.Remove(handlerEvent.Event.OrderLineIdentity);
        }

    }
}