using System.Collections.Generic;

namespace DDD.Core
{
    public interface IAggregateLoader
    {
        void SetAggregateContext(IAggregateContext context);
        void LoadFromHistory(int version, IEnumerable<LoadedEvent> history);
        IReadOnlyCollection<LoadedEvent> GetUncommittedChanges();
        void MarkChangesAsCommitted();
    }
}