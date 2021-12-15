namespace Pudicitia.Enterprise.Gateway.Models.Identity;

public class GetRoleOutput
{
    public RoleDetail Role { get; set; } = new RoleDetail();

    public ICollection<NamedEntityResult> Permissions { get; set; } = new List<NamedEntityResult>();
}
