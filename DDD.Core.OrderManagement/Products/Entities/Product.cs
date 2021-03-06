﻿using System;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Entities
{
    public partial class Product : AggregateRoot
    {
        private Product(TypedEvent<ProductCreated> initialEvent)
        {
            Created = initialEvent.EventDateTime;
            LastUpdate = initialEvent.EventDateTime;
            Identifier = initialEvent.Event.ProductIdentifier;
            ProductName = initialEvent.Event.ProductName;

            RegisterEvent<ProductNameChanged>(ProductNameChangedEventHandler);
        }

        public ProductIdentifier Identifier { get; }

        public DateTimeOffset Created { get; }

        public DateTimeOffset LastUpdate { get; private set; }

        public ProductName ProductName { get; private set; }

        public override IIdentifier GetIdentifier() => Identifier;


        private void ProductNameChangedEventHandler(HandlerEvent<ProductNameChanged> handlerEvent)
        {
            LastUpdate = handlerEvent.EventDateTime;
            ProductName = handlerEvent.Event.ProductName;
        }
    }
}