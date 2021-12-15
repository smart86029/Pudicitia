using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization;

public class UpdateRoleCommand : Command
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
}
