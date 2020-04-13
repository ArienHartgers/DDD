using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Orders.Identities
{
    public class OrderIdentity : IdentityValueObject
    {
        internal OrderIdentity(Guid guid)
        {
            OrderGuid = guid;
        }

        public Guid OrderGuid { get; }

        public override string Identity => $"Order_{OrderGuid}";

        public static OrderIdentity Parse(string s)
        {
            if (s.StartsWith("Order_"))
            {
                return new OrderIdentity(new Guid(s.Substring(6)));
            }

            throw new Exception("Invalid id");
        }

        public static OrderIdentity Create(Guid orderGuid)
        {
            return new OrderIdentity(orderGuid);
        }

        public static OrderIdentity New()
        {
            return new OrderIdentity(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OrderGuid;
        }
    }
}