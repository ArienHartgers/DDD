using DDD.Core.OrderManagement.Orders.Events;

namespace DDD.Core.OrderManagement.Orders.Entities
{
    public partial class OrderLine
    {
        public void AdjustQuantity(
            int quantity)
        {
            if (Quantity != quantity)
            {
                ApplyChange(
                    new OrderLineQuantityAdjusted(
                        _order.Identity,
                        Identity,
                        quantity));
            }
        }

        public void Remove()
        {
            ApplyChange(new OrderLineRemoved(
                _order.Identity,
                Identity));
        }
    }
}