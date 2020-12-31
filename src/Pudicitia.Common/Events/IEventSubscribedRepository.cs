using System;
using System.Threading.Tasks;

namespace Pudicitia.Common.Events
{
    public interface IEventSubscribedRepository
    {
        Task<bool> Contains(Guid eventId);

        void Add(EventSubscribed eventSubscribed);
    }
}