using System.Collections.Generic;

namespace DDD.Core
{
    public interface IAggregateLoader
    {
        void LoadFromHistory(int version, IEnumerable<LoadedEvent> history);
        IReadOnlyCollection<LoadedEvent> GetUncommittedChanges();
        void MarkChangesAsCommitted();
    }
}