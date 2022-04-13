namespace Pudicitia.Identity.App.Authorization;

public class CreateRoleCommand : Command
{
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
}
