using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Events;

namespace Pudicitia.Common.EntityFrameworkCore
{
    public class EventLogRepository<TContext> : IEventLogRepository
        where TContext : DbContext
    {
        private readonly TContext context;

        public EventLogRepository(TContext context)
        {
            this.context = context;
        }

        public void Add(EventLog eventLog)
        {
            context.Set<EventLog>().Add(eventLog);
        }
    }
}