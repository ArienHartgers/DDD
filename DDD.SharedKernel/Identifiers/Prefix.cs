using System;

namespace DDD.SharedKernel.Identifiers
{
    public class Prefix
    {
        public const int MaxLength = 4;

        public Prefix(string value)
        {
            if (value.Length > MaxLength)
            {
                throw new InvalidOperationException($"Prefix may not contain more then {MaxLength} characters");
            }

            foreach (var c in value)
            {
                if (!char.IsLower(c) || !char.IsLetter(c))
                {
                    throw new InvalidOperationException("Prefix may only contain lowercase characters");
                }
            }

            Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }
    }
}