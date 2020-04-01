using System;
using System.Collections.Generic;

namespace DDD.Core
{
    public abstract class Entity<TIdentity> : IEventApplier
        where TIdentity : class
    {

        private readonly Dictionary<Type, MessageHandler>
            _messageHandlerDictionary = new Dictionary<Type, MessageHandler>();

        public abstract TIdentity Identity { get; }

        bool IEventApplier.ProcessMessage(LoadedEvent loadedEvent)
        {
            if (_messageHandlerDictionary.TryGetValue(loadedEvent.Data.GetType(), out var handler))
            {
                handler.Execute(loadedEvent);
                return true;
            }

            return false;
        }

        protected void RegisterEvent<TEvent>(Action<HandlerEvent<TEvent>> callback)
            where TEvent : Event
        {
            var handler = new MessageHandler<TEvent>(callback);

            _messageHandlerDictionary.Add(typeof(TEvent), handler);
        }

        private abstract class MessageHandler
        {
            internal abstract void Execute(LoadedEvent loadedEvent);
        }

        private class MessageHandler<TEvent> : MessageHandler
            where TEvent : Event
        {
            private readonly Action<HandlerEvent<TEvent>> _callback;

            public MessageHandler(Action<HandlerEvent<TEvent>> callback)
            {
                _callback = callback;
            }

            internal override void Execute(LoadedEvent loadedEvent)
            {
                if (loadedEvent is HandlerEvent<TEvent> he)
                {
                    _callback(he);
                }
                else if (loadedEvent.Data is TEvent @event)
                {
                    _callback(new HandlerEvent<TEvent>(loadedEvent.EventDateTime, @event));
                }
            }
        }
    }
}