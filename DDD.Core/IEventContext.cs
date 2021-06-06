using System;

namespace DDD.Core
{
    public interface IEventContext
    {
        DateTimeOffset EventDateTime { get; }
    }
}