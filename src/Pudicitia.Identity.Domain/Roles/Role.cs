using System.Collections.Generic;
using System.Linq;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;
using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Domain.Roles
{
    public class Role : AggregateRoot
    {
        private readonly List<UserRole> userRoles = new List<UserRole>();
        private readonly List<RolePermission> rolePermissions = new List<RolePermission>();

        private Role()
        {
        }

        public Role(string name, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

            Name = name;
            IsEnabled = isEnabled;
        }

        public string Name { get; private set; }

        public bool IsEnabled { get; private set; }

        public IReadOnlyCollection<UserRole> UserRoles => userRoles.AsReadOnly();

        public IReadOnlyCollection<RolePermission> RolePermissions => rolePermissions.AsReadOnly();

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

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
            if (!rolePermissions.Any(x => x.PermissionId == permission.Id))
                rolePermissions.Add(new RolePermission(Id, permission.Id));
        }

        public void UnassignPermission(Permission permission)
        {
            var rolePermission = rolePermissions.FirstOrDefault(x => x.PermissionId == permission.Id);
            if (rolePermission != default(RolePermission))
                rolePermissions.Remove(rolePermission);
        }
    }
}