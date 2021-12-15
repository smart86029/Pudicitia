namespace Pudicitia.Enterprise.Gateway.Models.Identity;

public class GetUserOutput
{
    public UserDetail User { get; set; } = new UserDetail();

    public ICollection<NamedEntityResult> Roles { get; set; } = new List<NamedEntityResult>();
}
