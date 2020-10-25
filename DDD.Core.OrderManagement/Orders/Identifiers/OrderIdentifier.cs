using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Orders.Identifiers
{
    public class OrderIdentifier : IdentifierValueObject
    {
        internal OrderIdentifier(Guid guid)
        {
            OrderGuid = guid;
        }

        public Guid OrderGuid { get; }

        public override string Identifier => $"Order_{OrderGuid}";

        public static OrderIdentifier Create(string s)
        {
            if (s.StartsWith("Order_"))
            {
                return new OrderIdentifier(new Guid(s.Substring(6)));
            }

            throw new Exception("Invalid id");
        }

        public static OrderIdentifier Create(Guid orderGuid)
        {
            return new OrderIdentifier(orderGuid);
        }

        public static OrderIdentifier New()
        {
            return new OrderIdentifier(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return OrderGuid;
        }
    }
}