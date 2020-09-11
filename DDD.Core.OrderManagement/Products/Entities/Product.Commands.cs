using System;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identities;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Entities
{
    public partial class Product
    {
        public static Product Create(DateTimeOffset now, ProductName productName)
        {
            return Product.CreateWithEvent<Product, ProductCreated>(
                now,
                new ProductCreated(ProductIdentity.New(), productName));
        }

        public void ChangeName(ProductName productName)
        {
            if (ProductName != productName)
            {
                ApplyChange(new ProductNameChanged(Identity, productName));
            }
        }
    }
}