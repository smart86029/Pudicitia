namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class GetUsersInput : Pagination
{
    public string? UserName { get; set; }

    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }
}
