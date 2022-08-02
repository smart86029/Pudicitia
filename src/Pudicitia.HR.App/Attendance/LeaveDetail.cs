using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.App.Attendance;

public class LeaveDetail : EntityResult
{
    public LeaveType Type { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public string Reason { get; set; } = string.Empty;

    public ApprovalStatus ApprovalStatus { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;
}
