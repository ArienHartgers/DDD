using System;

namespace DDD.Core.OrderManagement.Orders
{
    public class OrderIdentity
    {
        internal OrderIdentity(Guid guid)
        {
            OrderGuid = guid;
        }

        public Guid OrderGuid { get; }


        public static OrderIdentity Parse(string s)
        {
            return new OrderIdentity(new Guid(s));
        }

        public override string ToString()
        {
            return $"Order-{OrderGuid}";
        }
    }
}