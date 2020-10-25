using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public interface IOrderLineCollection : IEntityCollection<OrderLine, OrderLineIdentifier>
    {
        OrderLine Get(ProductIdentifier productIdentifier);
    }

    public class OrderLineCollection : EntityCollection<OrderLine, OrderLineIdentifier>, IOrderLineCollection
    {
        public OrderLineCollection(IEntityModifier root) : base(root)
        {
        }

        public OrderLine Get(ProductIdentifier productIdentifier)
        {
            var orderLine = Entities.FirstOrDefault(ol => ol.ProductIdentifier == productIdentifier);
            return orderLine ?? throw new Exception($"OrderLine with productIdentifier {productIdentifier} not found");
        }
    }
}