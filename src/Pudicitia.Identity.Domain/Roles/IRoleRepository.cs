using System.Linq.Expressions;

namespace Pudicitia.Identity.Domain.Roles;

public interface IRoleRepository : IRepository<Role>
{
    Task<ICollection<Role>> GetRolesAsync();

    Task<ICollection<Role>> GetRolesAsync(int offset, int limit);

    Task<ICollection<Role>> GetRolesAsync(Expression<Func<Role, bool>> criteria);

    Task<Role> GetRoleAsync(Guid roleId);

    Task<int> GetCountAsync();

    void Add(Role role);

    void Update(Role role);

    void Remove(Role role);
}
