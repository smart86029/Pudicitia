namespace Pudicitia.Enterprise.Gateway.Models.Attendance;

public class LeaveSummary
{
    public Guid Id { get; set; }

    public int Type { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public int ApprovalStatus { get; set; }

    public Guid EmployeeId { get; set; }

    public string EmployeeName { get; set; } = string.Empty;
}
