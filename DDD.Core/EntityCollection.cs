using System;
using System.Collections;
using System.Collections.Generic;

namespace DDD.Core
{
    public class EntityCollection<TEntity, TIdentifier> : IEntityCollection<TEntity, TIdentifier>
        where TIdentifier : class, IIdentifier
        where TEntity : Entity<TIdentifier>
    {
        private readonly Dictionary<TIdentifier, TEntity> _entities = new Dictionary<TIdentifier, TEntity>();

        public IReadOnlyCollection<TEntity> Entities => _entities.Values;

        public TIdentifier? LastIdentifier { get; private set; }

        public void Add(TEntity entity)
        {
            _entities.Add(entity.GetIdentifier(), entity);

            LastIdentifier = entity.GetIdentifier();
        }

        public void Remove(TIdentifier identifier)
        {
            if (_entities.TryGetValue(identifier, out var entity))
            {
                IEntityModifier changer = entity;
                changer.MarkAsRemoved();
                _entities.Remove(identifier);
            }
        }

        public bool TryGet(TIdentifier identifier, out TEntity entity)
        {
            return _entities.TryGetValue(identifier, out entity);
        }

        public TEntity? Find(TIdentifier identifier)
        {
            if (TryGet(identifier, out var entity))
            {
                return entity;
            }

            return null;
        }

        public TEntity Get(TIdentifier identifier)
        {
            if (TryGet(identifier, out var entity))
            {
                return entity;
            }

            throw new Exception($"Entity '{identifier}' not found");
        }

        public Action<HandlerEvent<TEvent>> ForwardTo<TEvent>(Func<TEvent, TIdentifier> selector)
            where TEvent : Event
        {
            return @event =>
            {
                var identifier = selector(@event.Event);
                if (TryGet(identifier, out var entity))
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