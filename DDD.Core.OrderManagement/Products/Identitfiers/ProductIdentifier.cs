using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Products.Identitfiers
{
    public class ProductIdentifier : IdentifierValueObject
    {
        internal ProductIdentifier(string value)
        {
            Identifier = value;
        }

        public override string Identifier { get; }

        public static ProductIdentifier Parse(string s)
        {
            //if (s.StartsWith("Product_"))
            {
                return new ProductIdentifier(s);// new Guid(s.Substring(6)));
            }

            throw new Exception("Invalid id");
        }

        public static ProductIdentifier Create(string orderGuid)
        {
            return new ProductIdentifier(orderGuid);
        }

        public static ProductIdentifier New()
        {
            return new ProductIdentifier(Guid.NewGuid().ToString());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identifier;
        }
    }
}