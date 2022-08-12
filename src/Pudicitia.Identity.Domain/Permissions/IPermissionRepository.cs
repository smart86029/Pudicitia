using System.Linq.Expressions;

namespace Pudicitia.Identity.Domain.Permissions;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<ICollection<Permission>> GetPermissionsAsync();

    Task<ICollection<Permission>> GetPermissionsAsync(IEnumerable<Guid> permissionIds);

    Task<ICollection<Permission>> GetPermissionsAsync(int offset, int limit);

    Task<Permission> GetPermissionAsync(Guid permissionId);

    Task<int> GetCountAsync();

    void Add(Permission permission);

    void Update(Permission permission);

    void Remove(Permission permission);
}
