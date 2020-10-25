using System;
using DDD.Core;

namespace DDD.Adapters.Store
{
    public class AggregateContext : IAggregateContext
    {
        public DateTimeOffset GetDateTime()
        {
            return DateTimeOffset.Now;
        }
    }
}