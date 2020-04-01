using System;
using DDD.Core.OrderManagement.Orders.Events;

namespace DDD.Core.OrderManagement.Orders.Commands
{
    public static class ChangeOrderCustomerNameCommand
    {
        public static void ChangeOrderCustomerName(this Order order, string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                throw new Exception("Invalid name");
            }

            if (order.CustomerName != customerName)
            {
                order.ApplyChange(new OrderCustomerNameChangedEvent
                {
                    CustomerName = customerName,
                });
            }
        }
    }
}