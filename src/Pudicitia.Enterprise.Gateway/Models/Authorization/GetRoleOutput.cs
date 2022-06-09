namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class GetRoleOutput
{
    public RoleDetail Role { get; set; } = new RoleDetail();

    public ICollection<NamedEntityResult> Permissions { get; set; } = new List<NamedEntityResult>();
}
