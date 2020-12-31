using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore
{
    public class EventSubscribedRepository<TContext> : IEventSubscribedRepository
        where TContext : DbContext
    {
        private readonly TContext context;

        public EventSubscribedRepository(TContext context)
        {
            this.context = context;
        }

        public Task<bool> Contains(Guid eventId)
        {
            var result = context
                .Set<EventSubscribed>()
                .AnyAsync(x => x.EventId == eventId);

            return result;
        }

        public void Add(EventSubscribed eventSubscribed)
        {
            context
                .Set<EventSubscribed>()
                .Add(eventSubscribed);
        }
    }
}