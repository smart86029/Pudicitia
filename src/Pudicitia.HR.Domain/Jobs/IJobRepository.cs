using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Pudicitia.Common.Domain;

namespace Pudicitia.HR.Domain.Jobs
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<ICollection<Job>> GetJobsAsync();

        Task<Job> GetJobAsync(Guid jobId);

        void Add(Job job);

        void Update(Job job);
    }
}