﻿using DDD.Core.OrderManagement.Orders.Identifiers;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter.EventConverters
{
    public class OrderLineCreatedConverter : EventConverter<Core.OrderManagement.Orders.Events.OrderLineCreated, OrderLineCreated>
    {
        public override OrderLineCreated ConvertToExtern(Core.OrderManagement.Orders.Events.OrderLineCreated e)
        {
            return new OrderLineCreated(
                e.OrderIdentifier.Identifier,
                e.OrderLineIdentifier.Identifier,
                e.ProductIdentifier.Identifier,
                e.ProductName.Name,
                e.Quantity);
        }

        public override Core.OrderManagement.Orders.Events.OrderLineCreated ConvertToIntern(OrderLineCreated e)
        {
            return new Core.OrderManagement.Orders.Events.OrderLineCreated(
                OrderIdentifier.Parse(e.OrderIdentifier),
                OrderLineIdentifier.Parse(e.OrderLineIdentifier),
                ProductIdentifier.Parse(e.ProductIdentifier), 
                ProductName.Create(e.ProductName), 
                e.Quantity);
        }
    }
}