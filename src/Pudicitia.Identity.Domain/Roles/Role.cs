using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Domain.Roles;

public class Role : AggregateRoot
{
    private readonly List<UserRole> _userRoles = new();
    private readonly List<RolePermission> _rolePermissions = new();

    private Role()
    {
    }

    public Role(string name, bool isEnabled)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not be null");
        }

        Name = name;
        IsEnabled = isEnabled;
    }

    public string Name { get; private set; } = string.Empty;

    public bool IsEnabled { get; private set; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions.AsReadOnly();

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Name can not be null");
        }

        Name = name;
    }

    public void Enable()
    {
        IsEnabled = true;
    }

    public void Disable()
    {
        IsEnabled = false;
    }

    public void AssignPermission(Permission permission)
    {
        if (!_rolePermissions.Any(x => x.PermissionId == permission.Id))
        {
            _rolePermissions.Add(new RolePermission(Id, permission.Id));
        }
    }

    public void UnassignPermission(Permission permission)
    {
        var rolePermission = _rolePermissions.FirstOrDefault(x => x.PermissionId == permission.Id);
        if (rolePermission != default(RolePermission))
        {
            _rolePermissions.Remove(rolePermission);
        }
    }
}
