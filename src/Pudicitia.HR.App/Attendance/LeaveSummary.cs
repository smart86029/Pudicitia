using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.App.Attendance;

public class LeaveSummary
{
    public Guid Id { get; set; }

    public LeaveType Type { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;
}
