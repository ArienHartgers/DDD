using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Products.Identities
{
    public class ProductIdentity : IdentityValueObject
    {
        internal ProductIdentity(Guid guid)
        {
            ProductGuid = guid;
        }

        public Guid ProductGuid { get; }

        public override string Identity => $"Product_{ProductGuid}";

        public static ProductIdentity Parse(string s)
        {
            if (s.StartsWith("Product_"))
            {
                return new ProductIdentity(new Guid(s.Substring(6)));
            }

            throw new Exception("Invalid id");
        }

        public static ProductIdentity Create(Guid orderGuid)
        {
            return new ProductIdentity(orderGuid);
        }

        public static ProductIdentity New()
        {
            return new ProductIdentity(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProductGuid;
        }
    }
}