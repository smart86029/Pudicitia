namespace Pudicitia.HR.Domain.Leaves;

public class Leave : AggregateRoot
{
    private Leave()
    {
    }

    public Leave(LeaveType type, DateTime startedOn, DateTime endedOn, string reason, Guid employeeId)
    {
        Type = type;
        StartedOn = startedOn;
        EndedOn = endedOn;
        Reason = reason;
        EmployeeId = employeeId;
    }

    public LeaveType Type { get; private set; }

    public DateTime StartedOn { get; private set; }

    public DateTime EndedOn { get; private set; }

    public string Reason { get; private set; } = string.Empty;

    public ApprovalStatus ApprovalStatus { get; private set; }

    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

    public Guid EmployeeId { get; private set; }

    public void Approve()
    {
        if (ApprovalStatus != ApprovalStatus.Pending)
        {
            return;
        }

        ApprovalStatus = ApprovalStatus.Approved;
    }

    public void Reject()
    {
        if (ApprovalStatus != ApprovalStatus.Pending)
        {
            return;
        }

        ApprovalStatus = ApprovalStatus.Rejected;
    }
}
