namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class CreatePermissionInput
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
