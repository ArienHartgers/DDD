using System.Collections.Generic;

namespace DDD.Core
{
    public interface IEventStore
    {
        void SaveEvents(string streamName, int expectedVersion, IEnumerable<LoadedEvent> changes);

        StreamEvents GetStreamEvents(string streamName);

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