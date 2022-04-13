namespace Pudicitia.Identity.App.Authorization;

public class CreatePermissionCommand : Command
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
