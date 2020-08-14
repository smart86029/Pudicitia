using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Pudicitia.Common.Domain;

namespace Pudicitia.HR.Domain.JobTitles
{
    public interface IJobTitleRepository : IRepository<JobTitle>
    {
        Task<ICollection<JobTitle>> GetJobTitletsAsync();

        Task<JobTitle> GetJobTitleAsync(Guid jobTitleId);

        void Add(JobTitle jobTitle);

        void Update(JobTitle jobTitle);
    }
}