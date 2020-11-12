using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
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

        public override async Task<PaginateRolesResponse> PaginateRoles(PaginateRolesRequest request, ServerCallContext context)
        {
            var options = new RoleOptions
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            var roles = await authorizationApp.GetRolesAsync(options);
            var items = roles.Items.Select(x => new PaginateRolesResponse.Types.Role
            {
                Id = x.Id,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
            });
            var result = new PaginateRolesResponse
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

        public override async Task<GuidRequired> CreateRole(CreateRoleRequest request, ServerCallContext context)
        {
            var command = new CreateRoleCommand
            {
                Name = request.Name,
                IsEnabled = request.IsEnabled,
                PermissionIds = request.PermissionIds
                    .Select(x => (Guid)x)
                    .ToList(),
            };
            var result = await authorizationApp.CreateRoleAsync(command);

            return result;
        }

        public override async Task<Empty> UpdateRole(UpdateRoleRequest request, ServerCallContext context)
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
            await authorizationApp.UpdateRoleAsync(command);

            return new Empty();
        }

        public override async Task<Empty> DeleteRole(DeleteRoleRequest request, ServerCallContext context)
        {
            await authorizationApp.DeleteRoleAsync(request.Id);

            return new Empty();
        }

        public override async Task<ListNamedEntityResponse> ListPermissions(ListPermissionsRequest request, ServerCallContext context)
        {
            var permissions = await authorizationApp.GetPermissionsAsync();
            var items = permissions.Items.Select(x => new ListNamedEntityResponse.Types.NamedEntity
            {
                Id = x.Id,
                Name = x.Name,
            });
            var result = new ListNamedEntityResponse();
            result.Items.AddRange(items);

            return result;
        }

        public override async Task<PaginatePermissionsResponse> PaginatePermissions(PaginatePermissionsRequest request, ServerCallContext context)
        {
            var options = new PermissionOptions
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
            };
            var permissions = await authorizationApp.GetPermissionsAsync(options);
            var items = permissions.Items.Select(x => new PaginatePermissionsResponse.Types.Permission
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
            });
            var result = new PaginatePermissionsResponse
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