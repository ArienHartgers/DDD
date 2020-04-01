using DDD.Core.OrderManagement.Orders.Events;

namespace DDD.Core.OrderManagement.Orders
{
    public class OrderLine : Entity<OrderLineIdentity>
    {
        private OrderLineIdentity _identity = null!;

        public OrderLine()
        {
            RegisterEvent<OrderLineCreatedEvent>(handlerEvent =>
            {
                _identity = new OrderLineIdentity(handlerEvent.Event.LineId);
                ProductName = handlerEvent.Event.ProductName;
                Quantity = handlerEvent.Event.Quantity;
                
            });

            RegisterEvent<OrderLineQuantityAdjustedEvent>(handlerEvent =>
            {
                Quantity = handlerEvent.Event.Quantity;
            });
        }

        public override OrderLineIdentity Identity => _identity;
        public string ProductName { get; private set; } = null!;
        public int Quantity { get; private set; }

    }
}