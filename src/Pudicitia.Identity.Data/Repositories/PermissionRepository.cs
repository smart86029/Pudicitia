using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pudicitia.Identity.Domain.Permissions;

namespace Pudicitia.Identity.Data.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        public Task<ICollection<Permission>> GetPermissionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Permission>> GetPermissionsAsync(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Permission>> GetPermissionsAsync(Expression<Func<Permission, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<Permission> GetPermissionAsync(Guid permissionId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public void Add(Permission permission)
        {
            throw new NotImplementedException();
        }

        public void Update(Permission permission)
        {
            throw new NotImplementedException();
        }
    }
}