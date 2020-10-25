using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identitfiers;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class OrderLine : Entity<OrderLineIdentifier>
    {
        private readonly Order _order;

        public OrderLine(Order order, HandlerEvent<OrderLineCreated> initialEvent)
            : base(order)
        {
            _order = order;
            Identifier = initialEvent.Event.OrderLineIdentifier;
            ProductIdentifier = initialEvent.Event.ProductIdentifier;
            Quantity = initialEvent.Event.Quantity;

            RegisterEvent<OrderLineQuantityAdjusted>(ProcessOrderLineQuantityAdjusted);
        }

        public OrderLineIdentifier Identifier { get; }
        public ProductIdentifier ProductIdentifier { get; }
        public int Quantity { get; private set; }

        public override OrderLineIdentifier GetIdentifier() => Identifier;

        private void ProcessOrderLineQuantityAdjusted(HandlerEvent<OrderLineQuantityAdjusted> handlerEvent)
        {
            Quantity = handlerEvent.Event.Quantity;
        }
    }
}