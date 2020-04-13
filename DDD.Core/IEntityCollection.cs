using System.Collections.Generic;

namespace DDD.Core
{
    public interface IEntityCollection<TEntity, TIdentity>
        where TIdentity : class, IIdentity
        where TEntity : Entity<TIdentity>
    {
        public IReadOnlyCollection<TEntity> Entities { get; }

        public TIdentity? MaxIdentity { get; }

        public bool TryFind(TIdentity identity, out TEntity entity);

        public TEntity? Find(TIdentity identity);

        public TEntity Get(TIdentity identity);
    }
}