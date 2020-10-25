namespace DDD.Core
{
    public interface IEntityModifier
    {
        void ApplyChange(Event @event);
        void MarkAsRemoved();
        string GetPath();
    }
}