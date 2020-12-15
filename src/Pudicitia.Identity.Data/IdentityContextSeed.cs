using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Roles;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.Data
{
    public class IdentityContextSeed
    {
        private readonly IdentityContext context;

        public IdentityContextSeed(IdentityContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SeedAsync()
        {
            if (context.Set<User>().Any())
                return;

            var users = GetUsers();
            var roles = GetRoles();
            var permissions = GetPermissions();

            foreach (var user in users)
                foreach (var role in roles)
                    user.AssignRole(role);

            foreach (var role in roles)
                foreach (var permission in permissions)
                    role.AssignPermission(permission);

            context.Set<User>().AddRange(users);
            context.Set<Role>().AddRange(roles);
            context.Set<Permission>().AddRange(permissions);

            context.LogEvents();
            await context.SaveChangesAsync();
        }

        private IEnumerable<User> GetUsers()
        {
            var result = new User[]
            {
                new User("Admin", "123fff", "Admin", "Admin", true)
            };

            return result;
        }

        private IEnumerable<Role> GetRoles()
        {
            var result = new Role[]
            {
                new Role("Administrator", true),
                new Role("Human Resources", true)
            };

            return result;
        }

        private IEnumerable<Permission> GetPermissions()
        {
            var result = new Permission[]
            {
                new Permission("SignIn", "Sign In", default, true),
                new Permission("HumanResources", "Human Resources", default, true),
            };

            return result;
        }
    }
}