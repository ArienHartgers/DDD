using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class OrderLine : Entity
    {
        private readonly Order _order;

        public OrderLine(Order order, HandlerEvent<OrderLineCreated> initialEvent)
            : base(order)
        {
            _order = order;
            Identifier = initialEvent.Event.OrderLineIdentifier;
            ProductIdentifier = initialEvent.Event.ProductIdentifier;
            ProductName = initialEvent.Event.ProductName;
            Quantity = initialEvent.Event.Quantity;

            RegisterEvent<OrderLineQuantityAdjusted>(ProcessOrderLineQuantityAdjusted);
        }

        public OrderLineIdentifier Identifier { get; }
        public ProductIdentifier ProductIdentifier { get; }
        public ProductName ProductName { get; }
        public int Quantity { get; private set; }

        public override IIdentifier GetIdentifier() => Identifier;

        private void ProcessOrderLineQuantityAdjusted(HandlerEvent<OrderLineQuantityAdjusted> handlerEvent)
        {
            Quantity = handlerEvent.Event.Quantity;
        }
    }
}