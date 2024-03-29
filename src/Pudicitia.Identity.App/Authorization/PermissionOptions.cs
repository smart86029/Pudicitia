namespace Pudicitia.Identity.App.Authorization;

public class PermissionOptions : PaginationOptions
{
    public string? Code { get; set; }

    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }
}
