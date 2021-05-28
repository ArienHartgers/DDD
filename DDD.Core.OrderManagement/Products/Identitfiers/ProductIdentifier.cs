using System;
using System.Collections.Generic;
using DDD.SharedKernel.Identifiers;

namespace DDD.Core.OrderManagement.Products.Identitfiers
{
    public class ProductIdentifier : IdentifierValueObject
    {
        public static readonly Prefix Prefix = new Prefix("prd");

        internal ProductIdentifier(Guid guid)
        {
            ProductGuid = guid;
        }

        public Guid ProductGuid { get; }

        public override string Identifier => ProductGuid.ToSolidCode(Prefix);

        public static ProductIdentifier Parse(string s)
        {
            if (SolidCode.TryParseGuid(s, Prefix, out var guid))
            {
                return new ProductIdentifier(guid);
            }

            throw new Exception("Invalid id");
        }

        public static ProductIdentifier Create(Guid guid)
        {
            return new ProductIdentifier(guid);
        }

        public static ProductIdentifier New()
        {
            return new ProductIdentifier(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Identifier;
        }
    }
}