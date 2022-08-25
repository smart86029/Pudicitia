namespace Pudicitia.HR.Domain.Jobs
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<ICollection<Job>> GetJobsAsync();

        Task<Job> GetJobAsync(Guid jobId);

        void Add(Job job);

        void Update(Job job);

        void Remove(Job job);
    }
}
