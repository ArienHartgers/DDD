using System.Collections.Generic;

namespace DDD.Core
{
    public interface IEntityCollection<TEntity, TIdentifier> : IEnumerable<TEntity>
        where TIdentifier : class
        where TEntity : Entity<TIdentifier>
    {
        public IReadOnlyCollection<TEntity> Entities { get; }

        public TIdentifier? LastIdentifier { get; }

        public bool TryGet(TIdentifier identifier, out TEntity entity);

        public TEntity? Find(TIdentifier identifier);

        public TEntity Get(TIdentifier identifier);
    }
}