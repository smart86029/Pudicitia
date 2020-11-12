using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pudicitia.Common.Domain;
using Pudicitia.Identity.Domain.Permissions;

namespace Pudicitia.Identity.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IdentityContext context;
        private readonly DbSet<Permission> permissions;

        public PermissionRepository(IdentityContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            permissions = context.Set<Permission>();
        }

        public async Task<ICollection<Permission>> GetPermissionsAsync()
        {
            var result = await permissions
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Permission>> GetPermissionsAsync(int offset, int limit)
        {
            var result = await permissions
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return result;
        }

        public async Task<ICollection<Permission>> GetPermissionsAsync(Expression<Func<Permission, bool>> criteria)
        {
            var result = await permissions
                .Where(criteria)
                .ToListAsync();

            return result;
        }

        public async Task<Permission> GetPermissionAsync(Guid permissionId)
        {
            var result = await permissions
                .Include(x => x.RolePermissions)
                .SingleOrDefaultAsync(x => x.Id == permissionId) ??
                throw new EntityNotFoundException(typeof(Permission), permissionId);

            return result;
        }

        public async Task<int> GetCountAsync()
        {
            var result = await permissions
                .CountAsync();

            return result;
        }

        public void Add(Permission permission)
        {
            permissions.Add(permission);
        }

        public void Update(Permission permission)
        {
            permissions.Update(permission);
        }

        public void Remove(Permission permission)
        {
            permissions.Remove(permission);
        }
    }
}