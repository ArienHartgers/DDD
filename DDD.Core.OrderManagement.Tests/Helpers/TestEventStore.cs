﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Core.OrderManagement.Tests.Helpers
{
    public class TestEventStore : IEventStore
    {
        private readonly IEventStore.StreamEvents _noEvents = new IEventStore.StreamEvents(0, new List<LoadedEvent>(0));
        private readonly Dictionary<string, List<LoadedEvent>> _streams = new Dictionary<string, List<LoadedEvent>>();

        public void AddTestEvents(string streamName, params LoadedEvent[] changes)
        {
            if (!_streams.TryGetValue(streamName, out var loadedEvents))
            {
                loadedEvents = new List<LoadedEvent>();
                _streams.Add(streamName, loadedEvents);
            }

            loadedEvents.AddRange(changes);
        }

        Task IEventStore.SaveEventsAsync(string streamName, int expectedVersion, IEnumerable<LoadedEvent> changes)
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
            return Task.CompletedTask;
        }

        Task<IEventStore.StreamEvents> IEventStore.GetStreamEventsAsync(string streamName)
        {
            if (_streams.TryGetValue(streamName, out var loadedEvents))
            {
                return Task.FromResult(new IEventStore.StreamEvents(loadedEvents.Count, loadedEvents));
            }

            return Task.FromResult(_noEvents);
        }
    }
}