using System;
using System.Linq;
using DDD.Core.OrderManagement.Orders.Identities;
using DDD.Core.OrderManagement.Products.Identities;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public interface IOrderLineCollection : IEntityCollection<OrderLine, OrderLineIdentity>
    {
        OrderLine Get(ProductIdentity productIdentity);
    }

    public class OrderLineCollection : EntityCollection<OrderLine, OrderLineIdentity>, IOrderLineCollection
    {
        public OrderLine Get(ProductIdentity productIdentity)
        {
            var orderLine = Entities.FirstOrDefault(ol => ol.ProductIdentity == productIdentity);
            return orderLine ?? throw new Exception($"OrderLine with productIdentity {productIdentity} not found");
        }
    }
}