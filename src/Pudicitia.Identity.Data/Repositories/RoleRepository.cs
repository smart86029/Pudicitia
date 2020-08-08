using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public Task<ICollection<Role>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Role>> GetRolesAsync(int offset, int limit)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Role>> GetRolesAsync(Expression<Func<Role, bool>> criteria)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleAsync(Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync()
        {
            throw new NotImplementedException();
        }

        public void Add(Role role)
        {
            throw new NotImplementedException();
        }

        public void Update(Role role)
        {
            throw new NotImplementedException();
        }
    }
}