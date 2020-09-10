using DDD.Core.OrderManagement.Products.Events;
using DDD.Core.OrderManagement.Products.ValueObjects;

namespace DDD.Core.OrderManagement.Products.Entities
{
    public partial class Product
    {
        public void ChangeName(ProductName productName)
        {
            if (ProductName != productName)
            {
                ApplyChange(new ProductNameChanged(Identity, productName));
            }
        }
    }
}