using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Data.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly HRContext context;
        private readonly DbSet<Job> jobs;

        public JobRepository(HRContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            jobs = context.Set<Job>();
        }

        public async Task<ICollection<Job>> GetJobsAsync()
        {
            var result = await jobs
                .ToListAsync();

            return result;
        }

        public async Task<Job> GetJobAsync(Guid jobId)
        {
            var result = await jobs
                .SingleOrDefaultAsync(x => x.Id == jobId) ??
                throw new EntityNotFoundException(typeof(Job), jobId);

            return result;
        }

        public void Add(Job job)
        {
            jobs.Add(job);
        }

        public void Update(Job job)
        {
            jobs.Update(job);
        }
    }
}