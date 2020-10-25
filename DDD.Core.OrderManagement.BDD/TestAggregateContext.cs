using System;

namespace DDD.Core.OrderManagement.BDD
{
    public class TestAggregateContext : IAggregateContext
    {
        public DateTimeOffset GetDateTime()
        {
            return DateTimeOffset.Now;
        }
    }
}