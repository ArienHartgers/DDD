﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DDD.Core
{
    public class EntityCollection<TEntity, TIdentifier> : IEntityCollection<TEntity, TIdentifier>
        where TIdentifier : class, IIdentifier
        where TEntity : Entity
    {
        private readonly IEntityModifier _root;
        private readonly Dictionary<TIdentifier, TEntity> _entities = new Dictionary<TIdentifier, TEntity>();

        public EntityCollection(IEntityModifier root)
        {
            _root = root;
        }

        public IReadOnlyCollection<TEntity> Entities => _entities.Values;

        public TIdentifier? LastIdentifier { get; private set; }

        public void Add(TIdentifier identifier, TEntity entity)
        {
            _entities.Add(identifier, entity);

            LastIdentifier = identifier;
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

        public bool TryGet(TIdentifier identifier, [MaybeNullWhen(false)]out TEntity entity)
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

            throw new Exception($"{_root.GetPath()}/{typeof(TEntity).Name}/{identifier}' not found");
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