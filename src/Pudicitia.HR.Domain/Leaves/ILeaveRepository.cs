namespace Pudicitia.HR.Domain.Leaves;

public interface ILeaveRepository : IRepository<Leave>
{
    Task<ICollection<Leave>> GetLeavesAsync(int offset, int limit);

    Task<Leave> GetLeaveAsync(Guid leaveId);

    void Add(Leave leave);

    void Update(Leave leave);
}
