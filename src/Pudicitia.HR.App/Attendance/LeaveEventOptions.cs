namespace Pudicitia.HR.App.Attendance;

public class LeaveEventOptions
{
    public Guid UserId { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public ApprovalStatus ApprovalStatus { get; private set; } = ApprovalStatus.Rejected;
}
