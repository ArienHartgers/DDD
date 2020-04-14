using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Products.Identities
{
    public class ProductIdentity : IdentityValueObject
    {
        internal ProductIdentity(string guid)
        {
            Name = guid;
        }

        public string Name { get; }

        public override string Identity => Name;//$"Product_{ProductGuid}";

        public static ProductIdentity Parse(string s)
        {
            //if (s.StartsWith("Product_"))
            {
                return new ProductIdentity(s);// new Guid(s.Substring(6)));
            }

            throw new Exception("Invalid id");
        }

        public static ProductIdentity Create(string orderGuid)
        {
            return new ProductIdentity(orderGuid);
        }

        public static ProductIdentity New()
        {
            return new ProductIdentity(Guid.NewGuid().ToString());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;//ProductGuid;
        }
    }
}