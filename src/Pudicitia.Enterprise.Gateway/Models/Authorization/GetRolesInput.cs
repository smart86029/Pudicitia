namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class GetRolesInput : Pagination
{
    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }
}
