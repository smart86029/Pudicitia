using System;
using Pudicitia.Common.Domain;

namespace Pudicitia.Identity.Domain.Users
{
    public class UserRole : Entity
    {
        private UserRole()
        {
        }

        public UserRole(Guid userId, Guid roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public Guid UserId { get; private set; }

        public Guid RoleId { get; private set; }
    }
}