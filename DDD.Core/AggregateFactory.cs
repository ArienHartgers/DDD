using System;
using System.Linq;
using System.Reflection;

namespace DDD.Core
{
    public static class AggregateFactory
    {
        internal static TAggregateRoot CreateAggregateRoot<TAggregateRoot>(IEventContext context, Event initialEvent)
            where TAggregateRoot : AggregateRoot
        {
            var eventType = initialEvent.GetType();
            var constructors = typeof(TAggregateRoot).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (constructors.Any(c => !c.IsPrivate))
            {
                throw new Exception($"Aggregate {typeof(TAggregateRoot).Name} may not contain not private constructors");
            }

            foreach (var constructor in constructors)
            {
                var paramTypes = constructor.GetParameters();
                if (paramTypes.Length == 2 && paramTypes[0].ParameterType == typeof(IEventContext) && paramTypes[1].ParameterType == eventType)
                {
                    return (TAggregateRoot)constructor.Invoke(new object[] { context, initialEvent });
                }
            }

            throw new Exception($"Aggregate {typeof(TAggregateRoot).Name} must have at least 1 private constructor(IEventContext context, {eventType.Name} initialEvent)");
        }

    }
}