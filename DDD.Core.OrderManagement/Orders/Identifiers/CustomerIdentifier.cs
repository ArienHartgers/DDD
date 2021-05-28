using System;
using System.Collections.Generic;
using DDD.SharedKernel.Identifiers;

namespace DDD.Core.OrderManagement.Orders.Identifiers
{
    public class CustomerIdentifier : IdentifierValueObject
    {
        public static readonly Prefix Prefix = new Prefix("cus");

        internal CustomerIdentifier(Guid guid)
        {
            CustomerGuid = guid;
        }

        public Guid CustomerGuid { get; }

        public override string Identifier => CustomerGuid.ToSolidCode(Prefix);

        public static CustomerIdentifier Parse(string s)
        {
            if (SolidCode.TryParseGuid(s, Prefix, out var guid))
            {
                return new CustomerIdentifier(guid);
            }

            throw new Exception("Invalid id");
        }

        public static CustomerIdentifier Create(Guid customerGuid)
        {
            return new CustomerIdentifier(customerGuid);
        }

        public static CustomerIdentifier New()
        {
            return new CustomerIdentifier(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerGuid;
        }
    }
}