using System;
using System.Runtime.Serialization;

namespace Pudicitia.Common.Domain
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityNotFoundException(Type entityType, Guid id) :
            this($"There is no such an entity. Entity type: {entityType.FullName}, id: {id}")
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}