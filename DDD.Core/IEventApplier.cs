namespace DDD.Core
{
    public interface IEventApplier
    {
        bool ProcessMessage(LoadedEvent loadedEvent);

    }
}