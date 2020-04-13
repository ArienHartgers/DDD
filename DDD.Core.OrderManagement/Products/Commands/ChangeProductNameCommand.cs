using System;
using DDD.Core.OrderManagement.Orders.Events;
using DDD.Core.OrderManagement.Products.Entities;
using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Commands
{
    public static class ChangeProductNameCommand
    {
        public static void ChangeProductName(this Product product, ProductName productName)
        {

            if (product.ProductName != productName)
            {
                product.ApplyChange(new ProductNameChanged(product.Identity, productName));
            }
        }

    }
}