using System;

namespace DDD.Core
{
    public class EntityNotInitializedException: Exception
    {
        public EntityNotInitializedException(string entityName) 
            : base($"Entity '{entityName}' not initialized")
        {
        }
    }
}