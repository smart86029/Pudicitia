using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.Data.Repositories;

public class LeaveRepository : ILeaveRepository
{
    private readonly HRContext _context;
    private readonly DbSet<Leave> _leaves;

    public LeaveRepository(HRContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _leaves = context.Set<Leave>();
    }

    public async Task<ICollection<Leave>> GetLeavesAsync(int offset, int limit)
    {
        var results = await _leaves
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return results;
    }
}
