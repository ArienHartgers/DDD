using System;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Entities
{
    public class Product : AggregateRoot<ProductIdentity>
    {
        private ProductIdentity? _identity;

        private Product()
        {
            RegisterEvent<ProductCreated>(ProductCreatedEventHandler);
            RegisterEvent<ProductNameChanged>(ProductNameChangedEventHandler);
        }

        public override ProductIdentity Identity => _identity ?? throw new EntityNotInitializedException(nameof(Product));

        public DateTimeOffset Created { get; private set; }

        public DateTimeOffset LastUpdate { get; private set; }
        
        public ProductName ProductName { get; private set; }

        public static Product Create(ProductName productName)
        {
            var order = new Product();
            order.ApplyChange(new ProductCreated(ProductIdentity.New(), productName));

            return order;
        }

        private void ProductCreatedEventHandler(HandlerEvent<ProductCreated> handlerEvent)
        {
            _identity = handlerEvent.Event.ProductIdentity;
            Created = handlerEvent.EventDateTime;
            LastUpdate = handlerEvent.EventDateTime;
            ProductName = handlerEvent.Event.ProductName;
        }

        private void ProductNameChangedEventHandler(HandlerEvent<ProductNameChanged> handlerEvent)
        {
            LastUpdate = handlerEvent.EventDateTime;
            ProductName = handlerEvent.Event.ProductName;
        }
    }
}