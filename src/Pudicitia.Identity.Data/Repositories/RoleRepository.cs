using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityContext context;
        private readonly DbSet<Role> roles;

        public RoleRepository(IdentityContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            roles = context.Set<Role>();
        }

        public async Task<ICollection<Role>> GetRolesAsync()
        {
            var result = await roles
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Role>> GetRolesAsync(int offset, int limit)
        {
            var result = await roles
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Role>> GetRolesAsync(Expression<Func<Role, bool>> criteria)
        {
            var result = await roles
                .Include(x => x.RolePermissions)
                .Where(criteria)
                .ToListAsync();

            return result;
        }

        public async Task<Role> GetRoleAsync(Guid roleId)
        {
            var result = await roles
                .Include(x => x.RolePermissions)
                .SingleOrDefaultAsync(x => x.Id == roleId) ??
                throw new EntityNotFoundException(typeof(Role), roleId);

            return result;
        }

        public async Task<int> GetCountAsync()
        {
            var result = await roles
                .CountAsync();

            return result;
        }

        public void Add(Role role)
        {
            roles.Add(role);
        }

        public void Update(Role role)
        {
            roles.Update(role);
        }
    }
}