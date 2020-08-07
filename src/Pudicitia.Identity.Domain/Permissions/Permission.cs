using System.Collections.Generic;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Domain.Permissions
{
    public class Permission : AggregateRoot
    {
        private readonly List<RolePermission> rolePermissions = new List<RolePermission>();

        private Permission()
        {
        }

        public Permission(string code, string name, string description, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Code can not be null");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

            Code = code.Trim();
            Name = name.Trim();
            Description = description?.Trim();
            IsEnabled = isEnabled;
        }

        public string Code { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public bool IsEnabled { get; private set; }

        public IReadOnlyCollection<RolePermission> RolePermissions => rolePermissions;

        public void UpdateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Code can not be null");

            Code = code.Trim();
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

            Name = name.Trim();
        }

        public void UpdateDescription(string description)
        {
            Description = description?.Trim();
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}