namespace DDD.Core
{
    public abstract class IdentityValueObject : ValueObject, IIdentity
    {
        public abstract string Identity { get; }

        public override string ToString()
        {
            return Identity;
        }
    }
}