namespace Pudicitia.HR.App.Organization;

public class JobSummary : EntityResult
{
    public string Title { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
