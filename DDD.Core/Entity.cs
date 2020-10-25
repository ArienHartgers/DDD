using System;

namespace DDD.Core
{
    public abstract class Entity<TIdentifier> : EntityBase<TIdentifier>, IEntityModifier
    {
        private IEntityModifier? _root;

        protected Entity(IEntityModifier root)
        {
            _root = root;
        }

        protected void ApplyChange(Event @event)
        {
            if (_root == null)
            {
                throw new Exception("Entity is marked as removed");
            }

            _root.ApplyChange(@event);
        }

        void IEntityModifier.ApplyChange(Event @event)
        {
            ApplyChange(@event);
        }

        void IEntityModifier.MarkAsRemoved()
        {
            _root = null;
        }

        protected string GetPath()
        {
            return $"{_root?.GetPath()}/{GetType().Name}/{GetIdentifier()}";
        }

        string IEntityModifier.GetPath()
        {
            return GetPath();
        }
    }
}