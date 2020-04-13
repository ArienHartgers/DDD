using System;
using System.Collections.Generic;

namespace DDD.Core
{
    public class EntityCollection<TEntity, TIdentity> : IEntityCollection<TEntity, TIdentity>
        where TIdentity : class, IIdentity
        where TEntity : Entity<TIdentity>
    {

        private readonly Dictionary<TIdentity, TEntity> _entities = new Dictionary<TIdentity, TEntity>();
        private TIdentity? _maxIdentity = null;

        public IReadOnlyCollection<TEntity> Entities => _entities.Values;

        public TIdentity? MaxIdentity => _maxIdentity;

        public void Add(TEntity entity)
        {
            _entities.Add(entity.Identity, entity);

            _maxIdentity = entity.Identity;
        }

        public void Remove(TIdentity identity)
        {
            _entities.Remove(identity);
        }

        public bool TryFind(TIdentity identity, out TEntity entity)
        {
            return _entities.TryGetValue(identity, out entity);
        }

        public TEntity? Find(TIdentity identity)
        {
            if (TryFind(identity, out var entity))
            {
                return entity;
            }

            return null;
        }

        public TEntity Get(TIdentity identity)
        {
            if (TryFind(identity, out var entity))
            {
                return entity;
            }

            throw new Exception($"Entity '{identity}' not found");
        }

        public Action<HandlerEvent<TEvent>> ForwardTo<TEvent>(Func<TEvent, TIdentity> selector)
            where TEvent : Event
        {
            return @event =>
            {
                var identity = selector(@event.Event);
                if (TryFind(identity, out var entity))
                {
                    @event.ForwardTo(entity);
                }
            };
        }
    }
}