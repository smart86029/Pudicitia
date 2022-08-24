namespace Pudicitia.Identity.Domain.Permissions;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<ICollection<Permission>> GetPermissionsAsync();

    Task<ICollection<Permission>> GetPermissionsAsync(IEnumerable<Guid> permissionIds);

    Task<Permission> GetPermissionAsync(Guid permissionId);

    void Add(Permission permission);

    void Update(Permission permission);

    void Remove(Permission permission);
}
