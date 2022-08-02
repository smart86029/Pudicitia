namespace Pudicitia.Enterprise.Gateway.Models.Attendance;

public class LeaveDetail : EntityResult
{
    public int Type { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public string Reason { get; set; } = string.Empty;

    public int ApprovalStatus { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;
}
