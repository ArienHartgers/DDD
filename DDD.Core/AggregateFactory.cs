using System;
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
            if (constructors.Length == 1)
            {
                var constructor = constructors[0];
                if (constructor.IsPrivate)
                {

                    var paramTypes = constructor.GetParameters();
                    if (paramTypes.Length == 2 && paramTypes[0].ParameterType == typeof(IEventContext) && paramTypes[1].ParameterType == eventType)
                    {
                        return (TAggregateRoot)constructor.Invoke(new object[] { context, initialEvent });
                    }
                }
            }

            throw new Exception($"Aggregate {typeof(TAggregateRoot).Name} must have 1 private constructor(IEventContext context, {eventType.Name} initialEvent)");
        }

    }
}