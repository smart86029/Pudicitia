using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Data.Repositories
{
    public class JobTitleRepository : IJobRepository
    {
        private readonly HRContext context;
        private readonly DbSet<Job> jobs;

        public JobTitleRepository(HRContext context)
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

        public async Task<Job> GetJobAsync(Guid jobTitleId)
        {
            var result = await jobs
                .SingleOrDefaultAsync(j => j.Id == jobTitleId) ??
                throw new EntityNotFoundException(typeof(Job), jobTitleId);

            return result;
        }

        public void Add(Job jobTitle)
        {
            jobs.Add(jobTitle);
        }

        public void Update(Job jobTitle)
        {
            jobs.Update(jobTitle);
        }
    }
}