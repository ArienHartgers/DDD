using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public class OrderLine : Entity<OrderLineIdentity>
    {
        private OrderLineIdentity? _identity;

        public OrderLine()
        {
            RegisterEvent<OrderLineCreatedEvent>(handlerEvent =>
            {
                _identity = handlerEvent.Event.OrderLineIdentity;
                ProductIdentity = handlerEvent.Event.ProductIdentity;
                Quantity = handlerEvent.Event.Quantity;
            });

            RegisterEvent<OrderLineQuantityAdjustedEvent>(handlerEvent =>
            {
                Quantity = handlerEvent.Event.Quantity;
            });
        }

        public override OrderLineIdentity Identity => _identity ?? throw new EntityNotInitializedException(nameof(OrderLine));
        public ProductIdentity ProductIdentity { get; private set; } = null!;
        public int Quantity { get; private set; }

    }
}