namespace Pudicitia.Enterprise.Gateway.Models.Identity;

public class RoleSummary : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
