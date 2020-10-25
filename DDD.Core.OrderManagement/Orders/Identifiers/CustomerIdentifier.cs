using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Orders.Identitfiers
{
    public class CustomerIdentifier : IdentifierValueObject
    {
        internal CustomerIdentifier(Guid guid)
        {
            CustomerGuid = guid;
        }

        public Guid CustomerGuid { get; }

        public override string Identifier => $"Customer_{CustomerGuid}";

        public static CustomerIdentifier Create(string s)
        {
            if (s.StartsWith("Customer_"))
            {
                return new CustomerIdentifier(new Guid(s.Substring(6)));
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