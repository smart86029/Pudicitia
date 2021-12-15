namespace Pudicitia.Enterprise.Gateway.Models.Identity;

public class RoleDetail : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
}
