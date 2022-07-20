namespace Pudicitia.Enterprise.Gateway.Models.Attendance;

public class GetLeavesInput : Pagination
{
    public DateTime? StartedOn { get; set; }

    public DateTime? EndedOn { get; set; }

    public int? ApprovalStatus { get; set; }
}
