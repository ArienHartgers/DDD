using System;

namespace DDD.Core
{
    public interface IAggregateContext
    {
        DateTimeOffset GetDateTime();
    }
}