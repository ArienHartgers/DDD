using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Events;

namespace DDD.Core.OrderManagement.Orders
{
    public class Order : AggregateRoot<OrderIdentity>
    {
        private readonly List<OrderLine> _orderLines = new List<OrderLine>();
        private OrderIdentity _identity = null!;

        private Order()
        {
            RegisterEvent<OrderCreatedEvent>(OrderCreatedEventHandler);
            RegisterEvent<OrderCustomerNameChangedEvent>(OrderCustomerNameChangedEventHandler);
            RegisterEvent<OrderLineCreatedEvent>(OrderLineCreatedEventHandler);
            RegisterEvent<OrderLineRemovedEvent>(OrderLineRemovedEventHandler);

            RegisterEvent<OrderLineQuantityAdjustedEvent>(e => e.ForwardTo(FindOrderLine(e.Event.OrderLineIdentity)));
        }

        public override OrderIdentity Identity => _identity;

        public DateTimeOffset Created { get; private set; }

        public DateTimeOffset LastUpdate { get; private set; }

        public string CustomerName { get; private set; } = null!;

        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines;

        public OrderLine FindOrderLine(OrderLineIdentity id)
        {
            return _orderLines.FirstOrDefault(ol => ol.Identity == id);
        }

        public int LastOrderLineId { get; private set; }


        public static Order Create()
        {
            var order = new Order();
            order.ApplyChange(new OrderCreatedEvent(Guid.NewGuid()));

            return order;
        }

        private void OrderCreatedEventHandler(HandlerEvent<OrderCreatedEvent> handlerEvent)
        {
            _identity = new OrderIdentity(handlerEvent.Event.Id);
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
            LastOrderLineId = handlerEvent.Event.LineId;
            var orderLine = new OrderLine();
            handlerEvent.ForwardTo(orderLine);
            _orderLines.Add(orderLine);
        }

        private void OrderLineRemovedEventHandler(HandlerEvent<OrderLineRemovedEvent> handlerEvent)
        {
            var orderLine = FindOrderLine(new OrderLineIdentity(handlerEvent.Event.LineId));
            if (orderLine != null)
            {
                _orderLines.Remove(orderLine);
            }
        }

    }
}