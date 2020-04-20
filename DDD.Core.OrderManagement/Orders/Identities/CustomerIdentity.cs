using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Orders.Identities
{
    public class CustomerIdentity : IdentityValueObject
    {
        internal CustomerIdentity(Guid guid)
        {
            CustomerGuid = guid;
        }

        public Guid CustomerGuid { get; }

        public override string Identity => $"Customer_{CustomerGuid}";

        public static CustomerIdentity Create(string s)
        {
            if (s.StartsWith("Customer_"))
            {
                return new CustomerIdentity(new Guid(s.Substring(6)));
            }

            throw new Exception("Invalid id");
        }

        public static CustomerIdentity Create(Guid customerGuid)
        {
            return new CustomerIdentity(customerGuid);
        }

        public static CustomerIdentity New()
        {
            return new CustomerIdentity(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerGuid;
        }
    }
}