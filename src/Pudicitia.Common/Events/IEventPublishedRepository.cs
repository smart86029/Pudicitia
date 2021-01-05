using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pudicitia.Common.Events
{
    public interface IEventPublishedRepository
    {
        public Task<ICollection<EventPublished>> GetUnpublishedEventsAsync();

        Task UpdateAndCommit(EventPublished eventPublished);
    }
}