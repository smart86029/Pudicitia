namespace Pudicitia.Identity.Domain.Roles;

public interface IRoleRepository : IRepository<Role>
{
    Task<ICollection<Role>> GetRolesAsync();

    Task<ICollection<Role>> GetRolesAsync(IEnumerable<Guid> roleIds);

    Task<Role> GetRoleAsync(Guid roleId);

    void Add(Role role);

    void Update(Role role);

    void Remove(Role role);
}
