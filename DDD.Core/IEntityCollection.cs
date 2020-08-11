using System.Collections.Generic;

namespace DDD.Core
{
    public interface IEntityCollection<TEntity, TIdentity>
        where TEntity : Entity<TIdentity>
    {
        public IReadOnlyCollection<TEntity> Entities { get; }

        public TIdentity LastIdentity { get; }

        public bool TryFind(TIdentity identity, out TEntity entity);

        public TEntity? Find(TIdentity identity);

        public TEntity Get(TIdentity identity);
    }
}