using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Identitfiers;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public interface IOrderLineCollection : IEntityCollection<OrderLine, OrderLineIdentifier>
    {
        OrderLine Get(ProductIdentifier productIdentifier);
    }

    public class OrderLineCollection : EntityCollection<OrderLine, OrderLineIdentifier>, IOrderLineCollection
    {
        public OrderLine Get(ProductIdentifier productIdentifier)
        {
            var orderLine = Entities.FirstOrDefault(ol => ol.ProductIdentifier == productIdentifier);
            return orderLine ?? throw new Exception($"OrderLine with productIdentifier {productIdentifier} not found");
        }
    }
}