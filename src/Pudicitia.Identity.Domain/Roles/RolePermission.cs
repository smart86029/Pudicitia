using System;
using Pudicitia.Common.Domain;

namespace Pudicitia.Identity.Domain.Roles
{
    public class RolePermission : Entity
    {
        private RolePermission()
        {
        }

        public RolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public Guid RoleId { get; private set; }

        public Guid PermissionId { get; private set; }
    }
}