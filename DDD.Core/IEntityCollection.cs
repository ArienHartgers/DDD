using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DDD.Core
{
    public interface IEntityCollection<TEntity, TIdentifier> : IEnumerable<TEntity>
        where TIdentifier : class
        where TEntity : Entity
    {
        public IReadOnlyCollection<TEntity> Entities { get; }

        public TIdentifier? LastIdentifier { get; }

        public bool TryGet(TIdentifier identifier, [MaybeNullWhen(false)] out TEntity entity);

        public TEntity? Find(TIdentifier identifier);

        public TEntity Get(TIdentifier identifier);
    }
}