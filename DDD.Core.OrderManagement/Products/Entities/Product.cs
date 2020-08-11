using System;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Entities
{
    public class Product : AggregateRoot<ProductIdentity>
    {
        private Product(HandlerEvent<ProductCreated> initialEvent)
        {
            Created = initialEvent.EventDateTime;
            LastUpdate = initialEvent.EventDateTime;
            Identity = initialEvent.Event.ProductIdentity;
            ProductName = initialEvent.Event.ProductName;

            RegisterEvent<ProductNameChanged>(ProductNameChangedEventHandler);
        }

        public ProductIdentity Identity { get; }

        public DateTimeOffset Created { get; }

        public DateTimeOffset LastUpdate { get; private set; }
        
        public ProductName ProductName { get; private set; }

        public override ProductIdentity GetIdentity() => Identity;

        private void ProductNameChangedEventHandler(HandlerEvent<ProductNameChanged> handlerEvent)
        {
            LastUpdate = handlerEvent.EventDateTime;
            ProductName = handlerEvent.Event.ProductName;
        }
    }
}