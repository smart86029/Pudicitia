namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class GetUserOutput
{
    public UserDetail User { get; set; } = new();

    public ICollection<NamedEntityResult> Roles { get; set; } = new List<NamedEntityResult>();
}
