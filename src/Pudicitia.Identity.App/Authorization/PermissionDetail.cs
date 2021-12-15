using Pudicitia.Common.Models;

namespace Pudicitia.Identity.App.Authorization;

public class PermissionDetail : EntityResult
{
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }
}
