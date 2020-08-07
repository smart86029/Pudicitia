using System;

namespace Pudicitia.Common.Domain
{
    public class AggregateRoot : Entity
    {
        protected AggregateRoot()
        {
        }

        protected AggregateRoot(Guid id) : base(id)
        {
        }
    }
}