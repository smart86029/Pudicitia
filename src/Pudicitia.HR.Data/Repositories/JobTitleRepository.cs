using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.HR.Domain.JobTitles;

namespace Pudicitia.HR.Data.Repositories
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly HRContext context;

        public JobTitleRepository(HRContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<JobTitle>> GetJobTitletsAsync()
        {
            var result = await context
                .Set<JobTitle>()
                .ToListAsync();

            return result;
        }

        public async Task<JobTitle> GetJobTitleAsync(Guid jobTitleId)
        {
            var result = await context
                .Set<JobTitle>()
                .SingleOrDefaultAsync(j => j.Id == jobTitleId) ??
                throw new EntityNotFoundException(typeof(JobTitle), jobTitleId);

            return result;
        }

        public void Add(JobTitle jobTitle)
        {
            context.Set<JobTitle>().Add(jobTitle);
        }

        public void Update(JobTitle jobTitle)
        {
            context.Set<JobTitle>().Update(jobTitle);
        }
    }
}