using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DDD.Core
{
    public class EventStore
    {
        private readonly IReadOnlyCollection<LoadedEvent> _noEvents = new List<LoadedEvent>(0);
        private readonly Dictionary<string, List<LoadedEvent>> _streams = new Dictionary<string, List<LoadedEvent>>();

        public void SaveEvents(string streamName, int expectedVersion, params LoadedEvent[] changes)
        {
            //Console.WriteLine($"=== Save ==={streamName}===");
            //Console.WriteLine(JsonSerializer.Serialize(changes, new JsonSerializerOptions { WriteIndented = true }));


            if (!_streams.TryGetValue(streamName, out var loadedEvents))
            {
                loadedEvents = new List<LoadedEvent>();
                _streams.Add(streamName, loadedEvents);
            }

            if (expectedVersion != loadedEvents.Count)
            {
                throw new Exception($"Expected version is different from actual version");
            }

            loadedEvents.AddRange(changes);
        }

        public IReadOnlyCollection<LoadedEvent> GetStreamEvents(string streamName)
        {
            if (_streams.TryGetValue(streamName, out var loadedEvents))
            {
                return loadedEvents;
            }

            return _noEvents;
        }
    }
}