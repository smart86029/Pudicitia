namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class JobDetail : EntityResult
{
    public string Title { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
