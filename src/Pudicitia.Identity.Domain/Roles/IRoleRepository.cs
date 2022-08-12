namespace Pudicitia.Identity.Domain.Roles;

public interface IRoleRepository : IRepository<Role>
{
    Task<ICollection<Role>> GetRolesAsync();

    Task<ICollection<Role>> GetRolesAsync(IEnumerable<Guid> roleIds);

    Task<ICollection<Role>> GetRolesAsync(int offset, int limit);

    Task<Role> GetRoleAsync(Guid roleId);

    Task<int> GetCountAsync();

    void Add(Role role);

    void Update(Role role);

    void Remove(Role role);
}
