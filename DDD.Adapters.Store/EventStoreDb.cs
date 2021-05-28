using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DDD.Core;
using DDD.EventConverter;
using DDD.SharedKernel.Events;
using EventStore.Client;
using Google.Protobuf;

namespace DDD.Adapters.Store
{
    public class EventStoreDb : IEventStore
    {
        private EventStoreClient _client;
        private readonly IEventsConverter _eventsConverter;
        private Dictionary<string, Type> _externalEventTypes;


        public EventStoreDb(IEventsConverter eventsConverter, Uri uri)
        {
            _externalEventTypes = typeof(OrderCreated).Assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i=>i == typeof(IDomainEvent)))
                .ToDictionary(t=>t.Name);


            EventStoreClientSettings settings = new EventStoreClientSettings
            {
                ConnectivitySettings = new EventStoreClientConnectivitySettings
                {
                    Address = new Uri("http://localhost:2113"),
                }
            };

            _client = new EventStoreClient(settings);
            _eventsConverter = eventsConverter;
        }

        async Task IEventStore.SaveEventsAsync(string streamName, int expectedVersion, IEnumerable<LoadedEvent> changes)
        {
            var eventList = new List<EventData>();

            foreach (var loadedEvent in changes)
            {
                if (!_eventsConverter.TryConvert(loadedEvent.Data, out var externalEvent))
                {
                    throw new InvalidOperationException($"Cannot convert event {loadedEvent.Data.GetType().Name}");
                }

                eventList.Add(new EventData(
                    Uuid.NewUuid(),
                    externalEvent.GetType().Name,
                    JsonSerializer.SerializeToUtf8Bytes(externalEvent, externalEvent.GetType()),
                    null));
            }

            var streamRevision = expectedVersion == 0 ? StreamRevision.None : StreamRevision.FromInt64(expectedVersion-1);

            var result = await _client.AppendToStreamAsync(streamName, streamRevision, eventList);
        }

        async Task<IEventStore.StreamEvents> IEventStore.GetStreamEventsAsync(string streamName)
        {
            var stream = _client.ReadStreamAsync(
                Direction.Forwards,
                streamName,
                StreamPosition.Start);

            var loadedEvents = new List<LoadedEvent>();

            await foreach (var eventObject in stream)
            {
                if (!_externalEventTypes.TryGetValue(eventObject.Event.EventType, out var eventType))
                {
                    throw new InvalidOperationException("Unknown event type found");
                }

                var externalEvent = (IDomainEvent?)JsonSerializer.Deserialize(eventObject.Event.Data.Span, eventType);
                if (externalEvent == null)
                {
                    throw new InvalidOperationException($"Event cannot be null {eventObject.Event.EventType}");
                }

                if (!_eventsConverter.TryConvert(externalEvent, out var internalEvent))
                {
                    throw new InvalidOperationException($"Cannot convert event {eventObject.Event.EventType}");
                }

                loadedEvents.Add(new LoadedEvent(
                    eventObject.Event.Created,
                    internalEvent));
            }

            return new IEventStore.StreamEvents(loadedEvents.Count, loadedEvents);
        }
    }
}