namespace Pudicitia.HR.Domain.Leaves;

public interface ILeaveRepository : IRepository<Leave>
{
    Task<ICollection<Leave>> GetLeavesAsync(int offset, int limit);
}
