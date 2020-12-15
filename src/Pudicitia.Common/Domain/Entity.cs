using System;
using System.Collections.Generic;
using Pudicitia.Common.Utilities;

namespace Pudicitia.Common.Domain
{
    public abstract class Entity
    {
        private readonly List<DomainEvent> domainEvents = new List<DomainEvent>();

        protected Entity()
        {
        }

        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            Id = id;
        }

        public Guid Id { get; private set; } = GuidUtility.NewGuid();

        public IReadOnlyCollection<DomainEvent> DomainEvents => domainEvents.AsReadOnly();

        public void RaiseDomainEvent(DomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        public void ClearEvents()
        {
            domainEvents.Clear();
        }
    }
}