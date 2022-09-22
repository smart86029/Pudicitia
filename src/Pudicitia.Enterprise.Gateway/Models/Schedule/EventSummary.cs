namespace Pudicitia.Enterprise.Gateway.Models.Schedule;

public class EventSummary : EntityResult
{
    public string Title { get; set; } = string.Empty;

    public DateTime StartedOn { get; set; }

    public DateTime EndedOn { get; set; }

    public bool IsAllDay { get; set; }
}
