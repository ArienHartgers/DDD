using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.Identitfiers;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Entities
{
    public partial class Product
    {
        public static Product Create(
            IAggregateContext context,
            ProductName productName)
        {
            return Product.CreateWithEvent<Product, ProductCreated>(
                context,
                new ProductCreated(
                    ProductIdentifier.New(), 
                    productName));
        }

        public void ChangeName(ProductName productName)
        {
            if (ProductName != productName)
            {
                ApplyChange(new ProductNameChanged(Identifier, productName));
            }
        }
    }
}