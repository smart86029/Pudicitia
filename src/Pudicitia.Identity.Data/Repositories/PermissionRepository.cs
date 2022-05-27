using System.Linq.Expressions;

using Pudicitia.Identity.Domain.Permissions;

namespace Pudicitia.Identity.Data.Repositories;

public class PermissionRepository : IPermissionRepository
{
    private readonly IdentityContext _context;
    private readonly DbSet<Permission> _permissions;

    public PermissionRepository(IdentityContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _permissions = context.Set<Permission>();
    }

    public async Task<ICollection<Permission>> GetPermissionsAsync()
    {
        var result = await _permissions
            .ToListAsync();

        return result;
    }

    public async Task<ICollection<Permission>> GetPermissionsAsync(int offset, int limit)
    {
        var result = await _permissions
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return result;
    }

    public async Task<ICollection<Permission>> GetPermissionsAsync(Expression<Func<Permission, bool>> criteria)
    {
        var result = await _permissions
            .Where(criteria)
            .ToListAsync();

        return result;
    }

    public async Task<Permission> GetPermissionAsync(Guid permissionId)
    {
        var result = await _permissions
            .Include(x => x.RolePermissions)
            .SingleOrDefaultAsync(x => x.Id == permissionId)
            ?? throw new EntityNotFoundException<Permission>(permissionId);

        return result;
    }

    public async Task<int> GetCountAsync()
    {
        var result = await _permissions
            .CountAsync();

        return result;
    }

    public void Add(Permission permission)
    {
        _permissions.Add(permission);
    }

    public void Update(Permission permission)
    {
        _permissions.Update(permission);
    }

    public void Remove(Permission permission)
    {
        _permissions.Remove(permission);
    }
}
