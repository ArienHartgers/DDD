using System;
using System.Collections;
using System.Collections.Generic;

namespace DDD.Core
{
    public class EntityCollection<TEntity, TIdentity> : IEntityCollection<TEntity, TIdentity>
        where TIdentity : class, IIdentity
        where TEntity : Entity<TIdentity>
    {
        private readonly Dictionary<TIdentity, TEntity> _entities = new Dictionary<TIdentity, TEntity>();

        public IReadOnlyCollection<TEntity> Entities => _entities.Values;

        public TIdentity? LastIdentity { get; private set; }

        public void Add(TEntity entity)
        {
            _entities.Add(entity.GetIdentity(), entity);

            LastIdentity = entity.GetIdentity();
        }

        public void Remove(TIdentity identity)
        {
            if (_entities.TryGetValue(identity, out var entity))
            {
                IEntityModifier changer = entity;
                changer.MarkAsRemoved();
                _entities.Remove(identity);
            }
        }

        public bool TryGet(TIdentity identity, out TEntity entity)
        {
            return _entities.TryGetValue(identity, out entity);
        }

        public TEntity? Find(TIdentity identity)
        {
            if (TryGet(identity, out var entity))
            {
                return entity;
            }

            return null;
        }

        public TEntity Get(TIdentity identity)
        {
            if (TryGet(identity, out var entity))
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
                if (TryGet(identity, out var entity))
                {
                    @event.ForwardTo(entity);
                }
            };
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.Values.GetEnumerator();
        }
    }
}