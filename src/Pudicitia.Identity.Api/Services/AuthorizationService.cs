using Pudicitia.Identity.App.Authorization;

namespace Pudicitia.Identity.Api;

public class AuthorizationService : Authorization.AuthorizationBase
{
    private readonly AuthorizationApp _authorizationApp;

    public AuthorizationService(AuthorizationApp authorizationApp)
    {
        _authorizationApp = authorizationApp ?? throw new ArgumentNullException(nameof(authorizationApp));
    }

    public override async Task<ListNamedEntityResponse> ListUsers(
        ListUsersRequest request,
        ServerCallContext context)
    {
        var users = await _authorizationApp.GetUsersAsync(request.UserName);
        var items = users.Select(x => new ListNamedEntityResponse.Types.NamedEntity
        {
            Id = x.Id,
            Name = x.Name,
        });
        var result = new ListNamedEntityResponse();
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<PaginateUsersResponse> PaginateUsers(
        PaginateUsersRequest request,
        ServerCallContext context)
    {
        var options = new UserOptions
        {
            Page = request.Page,
            UserName = request.UserName,
            Name = request.Name,
            IsEnabled = request.IsEnabled,
        };

        var users = await _authorizationApp.GetUsersAsync(options);
        var items = users.Items.Select(x => new PaginateUsersResponse.Types.User
        {
            Id = x.Id,
            UserName = x.UserName,
            Name = x.Name,
            DisplayName = x.DisplayName,
            IsEnabled = x.IsEnabled,
        });
        var result = new PaginateUsersResponse
        {
            Page = users.Page,
        };
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<ExistsResponse> ExistUser(
        GuidRequired request,
        ServerCallContext context)
    {
        var doesExist = await _authorizationApp.ContainsUserAsync(request);
        var result = new ExistsResponse
        {
            DoesExist = doesExist
        };

        return result;
    }

    public override async Task<GetUserResponse> GetUser(
        GuidRequired request,
        ServerCallContext context)
    {
        var user = await _authorizationApp.GetUserAsync(request);
        var roleIds = user.RoleIds.Select(x => (GuidRequired)x);
        var result = new GetUserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Name = user.Name,
            DisplayName = user.DisplayName,
            IsEnabled = user.IsEnabled,
        };
        result.RoleIds.AddRange(roleIds);

        return result;
    }

    public override async Task<GuidRequired> CreateUser(
        CreateUserRequest request,
        ServerCallContext context)
    {
        var command = new CreateUserCommand
        {
            UserName = request.UserName,
            Password = request.Password,
            Name = request.Name,
            DisplayName = request.DisplayName,
            IsEnabled = request.IsEnabled,
            RoleIds = request.RoleIds
                .Select(x => (Guid)x)
                .ToList(),
        };
        var result = await _authorizationApp.CreateUserAsync(command);

        return result;
    }

    public override async Task<Empty> UpdateUser(
        UpdateUserRequest request,
        ServerCallContext context)
    {
        var command = new UpdateUserCommand
        {
            Id = request.Id,
            Password = request.Password,
            Name = request.Name,
            DisplayName = request.DisplayName,
            IsEnabled = request.IsEnabled,
            RoleIds = request.RoleIds
                .Select(x => (Guid)x)
                .ToList(),
        };
        await _authorizationApp.UpdateUserAsync(command);

        return new Empty();
    }

    public override async Task<Empty> DeleteUser(
        GuidRequired request,
        ServerCallContext context)
    {
        await _authorizationApp.DeleteUserAsync(request);

        return new Empty();
    }

    public override async Task<ListNamedEntityResponse> ListRoles(
        ListRolesRequest request,
        ServerCallContext context)
    {
        var roles = await _authorizationApp.GetRolesAsync();
        var items = roles.Select(x => new ListNamedEntityResponse.Types.NamedEntity
        {
            Id = x.Id,
            Name = x.Name,
        });
        var result = new ListNamedEntityResponse();
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<PaginateRolesResponse> PaginateRoles(
        PaginateRolesRequest request,
        ServerCallContext context)
    {
        var options = new RoleOptions
        {
            Page = request.Page,
            Name = request.Name,
            IsEnabled = request.IsEnabled,
        };
        var roles = await _authorizationApp.GetRolesAsync(options);
        var items = roles.Items.Select(x => new PaginateRolesResponse.Types.Role
        {
            Id = x.Id,
            Name = x.Name,
            IsEnabled = x.IsEnabled,
        });
        var result = new PaginateRolesResponse
        {
            Page = roles.Page,
        };
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<GetRoleResponse> GetRole(
        GuidRequired request,
        ServerCallContext context)
    {
        var role = await _authorizationApp.GetRoleAsync(request);
        var permissionIds = role.PermissionIds.Select(x => (GuidRequired)x);
        var result = new GetRoleResponse
        {
            Id = role.Id,
            Name = role.Name,
            IsEnabled = role.IsEnabled,
        };
        result.PermissionIds.AddRange(permissionIds);

        return result;
    }

    public override async Task<GuidRequired> CreateRole(
        CreateRoleRequest request,
        ServerCallContext context)
    {
        var command = new CreateRoleCommand
        {
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            PermissionIds = request.PermissionIds
                .Select(x => (Guid)x)
                .ToList(),
        };
        var result = await _authorizationApp.CreateRoleAsync(command);

        return result;
    }

    public override async Task<Empty> UpdateRole(
        UpdateRoleRequest request,
        ServerCallContext context)
    {
        var command = new UpdateRoleCommand
        {
            Id = request.Id,
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            PermissionIds = request.PermissionIds
                .Select(x => (Guid)x)
                .ToList(),
        };
        await _authorizationApp.UpdateRoleAsync(command);

        return new Empty();
    }

    public override async Task<Empty> DeleteRole(
        GuidRequired request,
        ServerCallContext context)
    {
        await _authorizationApp.DeleteRoleAsync(request);

        return new Empty();
    }

    public override async Task<ListNamedEntityResponse> ListPermissions(
        ListPermissionsRequest request,
        ServerCallContext context)
    {
        var permissions = await _authorizationApp.GetPermissionsAsync();
        var items = permissions.Select(x => new ListNamedEntityResponse.Types.NamedEntity
        {
            Id = x.Id,
            Name = x.Name,
        });
        var result = new ListNamedEntityResponse();
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<PaginatePermissionsResponse> PaginatePermissions(
        PaginatePermissionsRequest request,
        ServerCallContext context)
    {
        var options = new PermissionOptions
        {
            Page = request.Page,
            Code = request.Code,
            Name = request.Name,
            IsEnabled = request.IsEnabled,
        };
        var permissions = await _authorizationApp.GetPermissionsAsync(options);
        var items = permissions.Items.Select(x => new PaginatePermissionsResponse.Types.Permission
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            IsEnabled = x.IsEnabled,
        });
        var result = new PaginatePermissionsResponse
        {
            Page = permissions.Page,
        };
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<GetPermissionResponse> GetPermission(
        GuidRequired request,
        ServerCallContext context)
    {
        var permission = await _authorizationApp.GetPermissionAsync(request);
        var result = new GetPermissionResponse
        {
            Id = permission.Id,
            Code = permission.Code,
            Name = permission.Name,
            Description = permission.Description,
            IsEnabled = permission.IsEnabled,
        };

        return result;
    }

    public override async Task<GuidRequired> CreatePermission(
        CreatePermissionRequest request,
        ServerCallContext context)
    {
        var command = new CreatePermissionCommand
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
        };
        var result = await _authorizationApp.CreatePermissionAsync(command);

        return result;
    }

    public override async Task<Empty> UpdatePermission(
        UpdatePermissionRequest request,
        ServerCallContext context)
    {
        var command = new UpdatePermissionCommand
        {
            Id = request.Id,
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
        };
        await _authorizationApp.UpdatePermissionAsync(command);

        return new Empty();
    }

    public override async Task<Empty> DeletePermission(
        GuidRequired request,
        ServerCallContext context)
    {
        await _authorizationApp.DeletePermissionAsync(request);

        return new Empty();
    }
}
