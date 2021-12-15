namespace Pudicitia.Enterprise.Gateway.Models.Identity;

public class UpdatePermissionInput
{
    public Guid Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsEnabled { get; set; }
}
