using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Data.Repositories;

public class JobRepository : IJobRepository
{
    private readonly HRContext _context;
    private readonly DbSet<Job> _jobs;

    public JobRepository(HRContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _jobs = context.Set<Job>();
    }

    public async Task<ICollection<Job>> GetJobsAsync()
    {
        var results = await _jobs
            .Where(x => x.IsEnabled)
            .ToListAsync();

        return results;
    }

    public async Task<Job> GetJobAsync(Guid jobId)
    {
        var result = await _jobs
            .SingleOrDefaultAsync(x => x.Id == jobId)
            ?? throw new EntityNotFoundException<Job>(jobId);

        return result;
    }

    public void Add(Job job)
    {
        _jobs.Add(job);
    }

    public void Update(Job job)
    {
        _jobs.Update(job);
    }

    public void Remove(Job job)
    {
        _jobs.Remove(job);
    }
}
