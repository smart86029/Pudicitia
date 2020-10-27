using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Pudicitia.Common;
using Pudicitia.Identity.App.Authorization;

namespace Pudicitia.Identity.Api
{
    public class AuthorizationService : Authorization.AuthorizationBase
    {
        private readonly AuthorizationApp authorizationApp;

        public AuthorizationService(AuthorizationApp authorizationApp)
        {
            this.authorizationApp = authorizationApp;
        }

        public override async Task<ListRolesResponse> ListRoles(ListRolesRequest request, ServerCallContext context)
        {
            var options = new RoleOptions
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            var roles = await authorizationApp.GetRolesAsync(options);
            var items = roles.Items.Select(x => new Role
            {
                Id = x.Id,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
            });
            var result = new ListRolesResponse
            {
                PageIndex = options.PageIndex,
                PageSize = options.PageSize,
                ItemCount = roles.ItemCount,
            };
            result.Items.AddRange(items);

            return result;
        }

        public override async Task<GetRoleResponse> GetRole(GetRoleRequest request, ServerCallContext context)
        {
            var role = await authorizationApp.GetRoleAsync(request.Id);
            var permissionIds = role.PermissionIds.Cast<GuidRequired>();
            var result = new GetRoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                IsEnabled = role.IsEnabled,
            };
            result.PermissionIds.AddRange(permissionIds);

            return result;
        }

        public override async Task<ListPermissionsResponse> ListPermissions(ListPermissionsRequest request, ServerCallContext context)
        {
            var options = new PermissionOptions
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            var permissions = await authorizationApp.GetPermissionsAsync(options);
            var items = permissions.Items.Select(x => new Permission
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
            });
            var result = new ListPermissionsResponse
            {
                PageIndex = options.PageIndex,
                PageSize = options.PageSize,
                ItemCount = permissions.ItemCount,
            };
            result.Items.AddRange(items);

            return result;
        }
    }
}