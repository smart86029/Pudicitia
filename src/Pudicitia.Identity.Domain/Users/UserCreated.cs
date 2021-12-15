namespace Pudicitia.Identity.Domain.Users;

public class UserCreated : DomainEvent
{
    public UserCreated(Guid userId, string name, string displayName)
    {
        UserId = userId;
        Name = name;
        DisplayName = displayName;
    }

    public Guid UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;
}
