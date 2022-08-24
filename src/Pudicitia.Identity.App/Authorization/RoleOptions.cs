namespace Pudicitia.Identity.App.Authorization;

public class RoleOptions : PaginationOptions
{
    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }
}
