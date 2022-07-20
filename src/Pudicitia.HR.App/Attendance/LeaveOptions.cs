namespace Pudicitia.HR.App.Attendance;

public class LeaveOptions : PaginationOptions
{
    public DateTime? StartedOn { get; set; }

    public DateTime? EndedOn { get; set; }

    public  ApprovalStatus? ApprovalStatus { get; set; }
}
