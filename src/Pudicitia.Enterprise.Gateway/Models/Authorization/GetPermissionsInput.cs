namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class GetPermissionsInput : Pagination
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }
}
