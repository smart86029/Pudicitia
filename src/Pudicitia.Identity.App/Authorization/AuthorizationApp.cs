using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Roles;
using Pudicitia.Identity.Domain.Users;

namespace Pudicitia.Identity.App.Authorization;

public class AuthorizationApp
{
    private readonly string _connectionString;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AuthorizationApp(
        IOptions<DapperOptions> options,
        IIdentityUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository)
    {
        _connectionString = options.Value.ConnectionString;
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
    }

    public async Task<ICollection<NamedEntityResult>> GetUsersAsync(string userName)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            builder.Where("UserName LIKE @UserName", new { UserName = $"{userName}%" });
        }

        var sql = builder.AddTemplate($@"
SELECT
    Id,
    UserName AS Name
FROM [Identity].[User]
/**where**/
ORDER BY Id
");
        var users = await connection.QueryAsync<NamedEntityResult>(sql.RawSql, sql.Parameters);

        return users.ToList();
    }

    public async Task<PaginationResult<UserSummary>> GetUsersAsync(UserOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();

        if (!string.IsNullOrWhiteSpace(options.UserName))
        {
            builder.Where("UserName LIKE @UserName", new { UserName = $"{options.UserName}%" });
        }

        if (!string.IsNullOrWhiteSpace(options.Name))
        {
            builder.Where("Name LIKE @Name", new { Name = $"{options.Name}%" });
        }

        if (options.IsEnabled.HasValue)
        {
            builder.Where("IsEnabled = @IsEnabled", new { IsEnabled = options.IsEnabled.Value });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM [Identity].[User] /**where**/");
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount.RawSql, sqlCount.Parameters);
        var result = new PaginationResult<UserSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var sql = builder.AddTemplate($@"
SELECT
    Id,
    UserName,
    Name,
    DisplayName,
    IsEnabled
FROM [Identity].[User]
/**where**/
ORDER BY Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
");
        var users = await connection.QueryAsync<UserSummary>(sql.RawSql, sql.Parameters);
        result.Items = users.ToList();

        return result;
    }

    public async Task<UserDetail> GetUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        var result = new UserDetail
        {
            Id = user.Id,
            UserName = user.UserName,
            Name = user.Name,
            DisplayName = user.DisplayName,
            IsEnabled = user.IsEnabled,
            RoleIds = user.UserRoles
                .Select(x => x.RoleId)
                .ToList(),
        };

        return result;
    }

    public async Task<bool> ContainsUserAsync(Guid userId)
    {
        var result = await _userRepository.ContainsAsync(userId);

        return result;
    }

    public async Task<Guid> CreateUserAsync(CreateUserCommand command)
    {
        var user = new User(
            command.UserName,
            command.Password,
            command.Name,
            command.DisplayName,
            command.IsEnabled);
        var roleIdsToAssign = command.RoleIds;
        var rolesToAssign = await _roleRepository.GetRolesAsync(roleIdsToAssign);
        foreach (var role in rolesToAssign)
        {
            user.AssignRole(role);
        }

        _userRepository.Add(user);
        await _unitOfWork.CommitAsync();

        return user.Id;
    }

    public async Task UpdateUserAsync(UpdateUserCommand command)
    {
        var user = await _userRepository.GetUserAsync(command.Id);

        user.UpdatePassword(command.Password);
        user.UpdateName(command.Name);
        user.UpdateDisplayName(command.DisplayName);

        if (command.IsEnabled)
        {
            user.Enable();
        }
        else
        {
            user.Disable();
        }

        var roleIds = user.UserRoles.Select(x => x.RoleId);
        var roleIdsToAssign = command.RoleIds.Except(roleIds);
        var rolesToAssign = await _roleRepository.GetRolesAsync(roleIdsToAssign);
        foreach (var role in rolesToAssign)
        {
            user.AssignRole(role);
        }

        var roleIdsToUnassign = roleIds.Except(command.RoleIds);
        var rolesToUnassign = await _roleRepository.GetRolesAsync(roleIdsToUnassign);
        foreach (var role in rolesToUnassign)
        {
            user.UnassignRole(role);
        }

        _userRepository.Update(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserAsync(userId);

        _userRepository.Remove(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ICollection<NamedEntityResult>> GetRolesAsync()
    {
        var roles = await _roleRepository.GetRolesAsync();
        var results = roles
            .Select(x => new NamedEntityResult
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();

        return results;
    }

    public async Task<PaginationResult<RoleSummary>> GetRolesAsync(RoleOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();
        if (!string.IsNullOrWhiteSpace(options.Name))
        {
            builder.Where("Name LIKE @Name", new { Name = $"{options.Name}%" });
        }

        if (options.IsEnabled.HasValue)
        {
            builder.Where("IsEnabled = @IsEnabled", new { IsEnabled = options.IsEnabled.Value });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM [Identity].Role /**where**/");
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount.RawSql, sqlCount.Parameters);
        var result = new PaginationResult<RoleSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var sql = builder.AddTemplate($@"
SELECT
    Id,
    Name,
    IsEnabled
FROM [Identity].Role
/**where**/
ORDER BY Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
");
        var roles = await connection.QueryAsync<RoleSummary>(sql.RawSql, sql.Parameters);
        result.Items = roles.ToList();

        return result;
    }

    public async Task<RoleDetail> GetRoleAsync(Guid roleId)
    {
        var role = await _roleRepository.GetRoleAsync(roleId);
        var result = new RoleDetail
        {
            Id = role.Id,
            Name = role.Name,
            IsEnabled = role.IsEnabled,
            PermissionIds = role.RolePermissions
                .Select(x => x.PermissionId)
                .ToList(),
        };

        return result;
    }

    public async Task<Guid> CreateRoleAsync(CreateRoleCommand command)
    {
        var role = new Role(command.Name, command.IsEnabled);
        var permissionIdsToAssign = command.PermissionIds;
        var permissionsToAssign = await _permissionRepository.GetPermissionsAsync(permissionIdsToAssign);
        foreach (var permission in permissionsToAssign)
        {
            role.AssignPermission(permission);
        }

        _roleRepository.Add(role);
        await _unitOfWork.CommitAsync();

        return role.Id;
    }

    public async Task UpdateRoleAsync(UpdateRoleCommand command)
    {
        var role = await _roleRepository.GetRoleAsync(command.Id);

        role.UpdateName(command.Name);

        if (command.IsEnabled)
        {
            role.Enable();
        }
        else
        {
            role.Disable();
        }

        var permissionIds = role.RolePermissions.Select(x => x.PermissionId);
        var permissionIdsToAssign = command.PermissionIds.Except(permissionIds);
        var permissionsToAssign = await _permissionRepository.GetPermissionsAsync(permissionIdsToAssign);
        foreach (var permission in permissionsToAssign)
        {
            role.AssignPermission(permission);
        }

        var permissionIdsToUnassign = permissionIds.Except(command.PermissionIds);
        var permissionsToUnassign = await _permissionRepository.GetPermissionsAsync(permissionIdsToUnassign);
        foreach (var permission in permissionsToUnassign)
        {
            role.UnassignPermission(permission);
        }

        _roleRepository.Update(role);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        var role = await _roleRepository.GetRoleAsync(roleId);
        if (role.UserRoles.Any())
        {
            throw new InvalidCommandException("Is assigned can not be deleted");
        }

        _roleRepository.Remove(role);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ICollection<NamedEntityResult>> GetPermissionsAsync()
    {
        var permissions = await _permissionRepository.GetPermissionsAsync();
        var results = permissions
            .Select(x => new NamedEntityResult
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();

        return results;
    }

    public async Task<PaginationResult<PermissionSummary>> GetPermissionsAsync(PermissionOptions options)
    {
        using var connection = new SqlConnection(_connectionString);
        var builder = new SqlBuilder();
        if (!string.IsNullOrWhiteSpace(options.Name))
        {
            builder.Where("Code LIKE @Code", new { Name = $"{options.Code}%" });
        }

        if (!string.IsNullOrWhiteSpace(options.Name))
        {
            builder.Where("Name LIKE @Name", new { Name = $"{options.Name}%" });
        }

        if (options.IsEnabled.HasValue)
        {
            builder.Where("IsEnabled = @IsEnabled", new { IsEnabled = options.IsEnabled.Value });
        }

        var sqlCount = builder.AddTemplate("SELECT COUNT(*) FROM [Identity].Permission /**where**/");
        var itemCount = await connection.ExecuteScalarAsync<int>(sqlCount.RawSql, sqlCount.Parameters);
        var result = new PaginationResult<PermissionSummary>(options, itemCount);
        if (itemCount == 0)
        {
            return result;
        }

        var sql = builder.AddTemplate($@"
SELECT
    Id,
    Code,
    Name,
    IsEnabled
FROM [Identity].Permission
/**where**/
ORDER BY Id
OFFSET {result.Offset} ROWS
FETCH NEXT {result.Limit} ROWS ONLY
");
        var permissions = await connection.QueryAsync<PermissionSummary>(sql.RawSql, sql.Parameters);
        result.Items = permissions.ToList();

        return result;
    }

    public async Task<PermissionDetail> GetPermissionAsync(Guid permissionId)
    {
        var permission = await _permissionRepository.GetPermissionAsync(permissionId);
        var result = new PermissionDetail
        {
            Id = permission.Id,
            Code = permission.Code,
            Name = permission.Name,
            Description = permission.Description,
            IsEnabled = permission.IsEnabled,
        };

        return result;
    }

    public async Task<Guid> CreatePermissionAsync(CreatePermissionCommand command)
    {
        var permission = new Permission(command.Code, command.Name, command.Description, command.IsEnabled);

        _permissionRepository.Add(permission);
        await _unitOfWork.CommitAsync();

        return permission.Id;
    }

    public async Task UpdatePermissionAsync(UpdatePermissionCommand command)
    {
        var permission = await _permissionRepository.GetPermissionAsync(command.Id);

        permission.UpdateCode(command.Code);
        permission.UpdateName(command.Name);
        permission.UpdateDescription(command.Description);

        if (command.IsEnabled)
        {
            permission.Enable();
        }
        else
        {
            permission.Disable();
        }

        _permissionRepository.Update(permission);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeletePermissionAsync(Guid permissionId)
    {
        var permission = await _permissionRepository.GetPermissionAsync(permissionId);
        if (permission.RolePermissions.Any())
        {
            throw new InvalidCommandException("Is assigned can not be deleted");
        }

        _permissionRepository.Remove(permission);
        await _unitOfWork.CommitAsync();
    }
}
