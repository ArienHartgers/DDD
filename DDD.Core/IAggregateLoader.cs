using System.Collections.Generic;

namespace DDD.Core
{
    public interface IAggregateLoader
    {
        void LoadFromHistory(IEnumerable<LoadedEvent> history);
        IEnumerable<LoadedEvent> GetUncommittedChanges();
        void MarkChangesAsCommitted();
    }
}