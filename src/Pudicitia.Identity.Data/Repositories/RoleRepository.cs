using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly IdentityContext _context;
    private readonly DbSet<Role> _roles;

    public RoleRepository(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _roles = context.Set<Role>();
    }

    public async Task<ICollection<Role>> GetRolesAsync()
    {
        var results = await _roles
            .ToListAsync();

        return results;
    }

    public async Task<ICollection<Role>> GetRolesAsync(IEnumerable<Guid> roleIds)
    {
        var results = await _roles
            .Include(x => x.RolePermissions)
            .Where(x => roleIds.Contains(x.Id))
            .ToListAsync();

        return results;
    }

    public async Task<Role> GetRoleAsync(Guid roleId)
    {
        var result = await _roles
            .Include(x => x.UserRoles)
            .Include(x => x.RolePermissions)
            .SingleOrDefaultAsync(x => x.Id == roleId)
            ?? throw new EntityNotFoundException<Role>(roleId);

        return result;
    }

    public void Add(Role role)
    {
        _roles.Add(role);
    }

    public void Update(Role role)
    {
        _roles.Update(role);
    }

    public void Remove(Role role)
    {
        _roles.Remove(role);
    }
}
