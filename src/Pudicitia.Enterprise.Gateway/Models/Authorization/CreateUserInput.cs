namespace Pudicitia.Enterprise.Gateway.Models.Authorization;

public class CreateUserInput
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public ICollection<Guid> RoleIds { get; set; } = new List<Guid>();
}