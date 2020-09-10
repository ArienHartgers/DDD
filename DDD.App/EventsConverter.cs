using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DDD.App.Events;
using DDD.Core;

namespace DDD.App
{
    public class EventsConverter
    {
        private readonly Dictionary<Type, EventConverter> _externalConverters;
        private readonly Dictionary<Type, EventConverter> _internalConverters;

        public EventsConverter()
        {
            var converterTypes = typeof(EventsConverter).Assembly.GetExportedTypes()
                .Where(t => t.BaseType?.BaseType != null && t.BaseType.BaseType == typeof(EventConverter))
                .ToList();

            var constructorTypes = new Type[0];
            var converters = converterTypes.Select(t => t.GetConstructor(constructorTypes)?.Invoke(null))
                .OfType<EventConverter>()
                 .ToList();

            _externalConverters = converters.ToDictionary(c => c.ExternalType);
            _internalConverters = converters.ToDictionary(c => c.InternalType);
        }

        public bool TryConvert(Event @event, [NotNullWhen(true)] out IDomainEvent domainEvent)
        {
            if (_internalConverters.TryGetValue(@event.GetType(), out var converter))
            {
                domainEvent= converter.Convert(@event);
                return true;
            }

            domainEvent = null!;
            return false;
        }

        public bool TryConvert(IDomainEvent domainEvent, [NotNullWhen(true)] out Event @event)
        {
            if (_externalConverters.TryGetValue(domainEvent.GetType(), out var converter))
            {
                @event = converter.Convert(domainEvent);
                return true;
            }

            @event = null!;
            return false;
        }
    }
}