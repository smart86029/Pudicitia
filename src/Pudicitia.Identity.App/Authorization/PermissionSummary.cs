namespace Pudicitia.Identity.App.Authorization;

public class PermissionSummary : EntityResult
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
