namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class UpdateRoleInput
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
}
