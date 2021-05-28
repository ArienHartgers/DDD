using System.Diagnostics.CodeAnalysis;
using DDD.Core;
using DDD.SharedKernel.Events;

namespace DDD.EventConverter
{
    public interface IEventsConverter
    {
        bool TryConvert(Event @event, [NotNullWhen(true)] out IDomainEvent domainEvent);

        bool TryConvert(IDomainEvent domainEvent, [NotNullWhen(true)] out Event @event);
    }
}