namespace Pudicitia.Identity.App.Authorization;

public class RoleSummary : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
