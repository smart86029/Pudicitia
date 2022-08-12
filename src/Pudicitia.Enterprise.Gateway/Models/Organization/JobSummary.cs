namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class JobSummary : EntityResult
{
    public string Title { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public int EmployeeCount { get; set; }
}
