using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class OrderLine : Entity<OrderLineIdentity>
    {
        private readonly Order _order;

        public OrderLine(Order order, HandlerEvent<OrderLineCreated> initialEvent)
            : base(order)
        {
            _order = order;
            Identity = initialEvent.Event.OrderLineIdentity;
            ProductIdentity = initialEvent.Event.ProductIdentity;
            Quantity = initialEvent.Event.Quantity;

            RegisterEvent<OrderLineQuantityAdjusted>(ProcessOrderLineQuantityAdjusted);
        }

        public OrderLineIdentity Identity { get; }
        public ProductIdentity ProductIdentity { get; }
        public int Quantity { get; private set; }

        public override OrderLineIdentity GetIdentity() => Identity;

        private void ProcessOrderLineQuantityAdjusted(HandlerEvent<OrderLineQuantityAdjusted> handlerEvent)
        {
            Quantity = handlerEvent.Event.Quantity;
        }
    }
}