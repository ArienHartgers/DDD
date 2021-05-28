using System;
using System.Collections.Generic;

namespace DDD.Core.OrderManagement.Products.ValueObjects
{
    public class ProductName : ValueObject
    {
        private ProductName(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static ProductName Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Invalid name");
            }
            return new ProductName(name);
        }

        public override string ToString()
        {
            return Name;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}