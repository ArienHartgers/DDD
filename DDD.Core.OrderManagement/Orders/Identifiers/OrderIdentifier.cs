using System;
using System.Collections.Generic;
using DDD.SharedKernel.Identifiers;

namespace DDD.Core.OrderManagement.Orders.Identifiers
{
    public class OrderIdentifier : IdentifierValueObject
    {
        public static readonly Prefix Prefix = new Prefix("odr");

        internal OrderIdentifier(Guid guid)
        {
            OrderGuid = guid;
        }

        public Guid OrderGuid { get; }

        public override string Identifier => OrderGuid.ToSolidCode(Prefix);

        public static OrderIdentifier Parse(string s)
        {
            if (SolidCode.TryParseGuid(s, Prefix, out var guid))
            {
                return new OrderIdentifier(guid);
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