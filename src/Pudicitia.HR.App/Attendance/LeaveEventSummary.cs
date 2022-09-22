using Pudicitia.HR.Domain.Leaves;

namespace Pudicitia.HR.App.Attendance;

public class LeaveEventSummary : EntityResult
{
    public LeaveType Type { get; set; }

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; }

    public string Title => ApprovalStatus == ApprovalStatus.Pending ? $"{Type} (Pending)" : Type.ToString();

    public bool IsAllDay => (EndedOn - StartedOn) > TimeSpan.FromHours(8);
}
