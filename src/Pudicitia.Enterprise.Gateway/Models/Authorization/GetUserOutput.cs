namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class GetUserOutput
{
    public UserDetail User { get; set; } = new UserDetail();

    public ICollection<NamedEntityResult> Roles { get; set; } = new List<NamedEntityResult>();
}
