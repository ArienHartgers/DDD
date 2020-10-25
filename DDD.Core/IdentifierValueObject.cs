namespace DDD.Core
{
    public abstract class IdentifierValueObject : ValueObject, IIdentifier
    {
        public abstract string Identifier { get; }

        public override string ToString()
        {
            return Identifier;
        }
    }
}