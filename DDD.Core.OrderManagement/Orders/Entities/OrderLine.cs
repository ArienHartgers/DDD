using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public class OrderLine : Entity<OrderLineIdentity>
    {
        public OrderLine(HandlerEvent<OrderLineCreated> handlerEvent)
        {
            Identity = handlerEvent.Event.OrderLineIdentity;
            ProductIdentity = handlerEvent.Event.ProductIdentity;
            Quantity = handlerEvent.Event.Quantity;

            RegisterEvent<OrderLineQuantityAdjusted>(handlerEvent =>
            {
                Quantity = handlerEvent.Event.Quantity;
            });
        }

        public OrderLineIdentity Identity { get; }
        public ProductIdentity ProductIdentity { get; }
        public int Quantity { get; private set; }

        public override OrderLineIdentity GetIdentity() => Identity;
    }
}