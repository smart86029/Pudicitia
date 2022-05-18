using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Roles;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.App.Authentication;

public class AuthenticationApp
{
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AuthenticationApp(
        IIdentityUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
    }

    public async Task<UserDetail?> GetUserAsync(string userName, string password)
    {
        try
        {
            var user = await _userRepository.GetUserAsync(userName, password);
            var result = new UserDetail
            {
                Id = user.Id,
                UserName = user.UserName,
            };

            return result;
        }
        catch
        {
        }

        return default;
    }

    public async Task<List<string>> GetPermissionCodesAsync(Guid userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        var roleIds = user.UserRoles.Select(x => x.RoleId);
        var roles = await _roleRepository.GetRolesAsync(r => roleIds.Contains(r.Id));
        var permissionIds = roles.SelectMany(r => r.RolePermissions).Select(x => x.PermissionId).Distinct();
        var permissions = await _permissionRepository.GetPermissionsAsync(p => permissionIds.Contains(p.Id));
        var result = permissions.Select(p => p.Code).ToList();

        return result;
    }
}
