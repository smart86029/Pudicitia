using System;

namespace Pudicitia.Common.RabbitMQ
{
    internal class Subscription
    {
        public Subscription(Type eventType, Type eventHandlerType)
        {
            EventType = eventType;
            EventHandlerType = eventHandlerType;
        }

        public Type EventType { get; private init; }

        public Type EventHandlerType { get; private init; }
    }
}