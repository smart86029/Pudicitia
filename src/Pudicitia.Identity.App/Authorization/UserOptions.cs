namespace Pudicitia.Identity.App.Authorization;

public class UserOptions : PaginationOptions
{
    public string? UserName { get; set; }

    public string? Name { get; set; }

    public bool? IsEnabled { get; set; }
}
