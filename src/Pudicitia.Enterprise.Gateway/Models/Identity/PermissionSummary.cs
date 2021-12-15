namespace Pudicitia.Enterprise.Gateway.Models.Identity;

public class PermissionSummary : EntityResult
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
