using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Core
{
    public interface IEventStore
    {
        Task SaveEventsAsync(string streamName, int expectedVersion, IEnumerable<LoadedEvent> changes);

        Task<StreamEvents> GetStreamEventsAsync(string streamName);

        public class StreamEvents
        {
            public StreamEvents(int version, IReadOnlyCollection<LoadedEvent> events)
            {
                Version = version;
                Events = events;
            }

            public int Version { get; }

            public IReadOnlyCollection<LoadedEvent> Events { get; }
        }
    }
}