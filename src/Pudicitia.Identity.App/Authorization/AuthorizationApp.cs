using System;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.Common.App;
using Pudicitia.Identity.Domain;
using Pudicitia.Identity.Domain.Permissions;
using Pudicitia.Identity.Domain.Roles;

namespace Pudicitia.Identity.App.Authorization
{
    public class AuthorizationApp
    {
        private readonly IIdentityUnitOfWork unitOfWork;
        private readonly IRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;

        public AuthorizationApp(
            IIdentityUnitOfWork unitOfWork,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            this.permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
        }

        public async Task<PaginationResult<RoleSummary>> GetRolesAsync(RoleOptions options)
        {
            var roles = await roleRepository.GetRolesAsync(options.Offset, options.Limit);
            var count = await roleRepository.GetCountAsync();
            var result = new PaginationResult<RoleSummary>
            {
                Items = roles
                    .Select(x => new RoleSummary
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsEnabled = x.IsEnabled,
                    })
                    .ToList(),
                ItemCount = count,
            };

            return result;
        }

        public async Task<RoleDetail> GetRoleAsync(Guid roleId)
        {
            var role = await roleRepository.GetRoleAsync(roleId);
            var result = new RoleDetail
            {
                Id = role.Id,
                Name = role.Name,
                IsEnabled = role.IsEnabled,
                PermissionIds = role.RolePermissions
                    .Select(x => x.Id)
                    .ToList(),
            };

            return result;
        }

        public async Task<Guid> CreateRoleAsync(CreateRoleCommand command)
        {
            var role = new Role(command.Name, command.IsEnabled);
            var permissionIdsToAssign = command.PermissionIds;
            var permissionsToAssign = await permissionRepository.GetPermissionsAsync(x => permissionIdsToAssign.Contains(x.Id));
            foreach (var permission in permissionsToAssign)
                role.AssignPermission(permission);

            roleRepository.Add(role);
            await unitOfWork.CommitAsync();

            return role.Id;
        }

        public async Task UpdateRoleAsync(UpdateRoleCommand command)
        {
            var role = await roleRepository.GetRoleAsync(command.Id);

            role.UpdateName(command.Name);

            if (command.IsEnabled)
                role.Enable();
            else
                role.Disable();

            var permissionIdsToAssign = command.PermissionIds
                .Except(role.RolePermissions
                .Select(x => x.PermissionId));
            var permissionsToAssign = await permissionRepository.GetPermissionsAsync(x => permissionIdsToAssign.Contains(x.Id));
            foreach (var permission in permissionsToAssign)
                role.AssignPermission(permission);

            var permissionIdsToUnassign = role.RolePermissions
                .Select(x => x.PermissionId)
                .Except(command.PermissionIds);
            var permissionsToUnassign = await permissionRepository.GetPermissionsAsync(x => permissionIdsToUnassign.Contains(x.Id));
            foreach (var permission in permissionsToUnassign)
                role.UnassignPermission(permission);

            roleRepository.Update(role);
            await unitOfWork.CommitAsync();
        }

        public async Task<PaginationResult<PermissionSummary>> GetPermissionsAsync(PermissionOptions options)
        {
            var permissions = await permissionRepository.GetPermissionsAsync(options.Offset, options.Limit);
            var count = await permissionRepository.GetCountAsync();
            var result = new PaginationResult<PermissionSummary>
            {
                Items = permissions
                    .Select(x => new PermissionSummary
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        IsEnabled = x.IsEnabled,
                    })
                    .ToList(),
                ItemCount = count,
            };

            return result;
        }

        public async Task<PermissionDetail> GetPermissionAsync(Guid permissionId)
        {
            var permission = await permissionRepository.GetPermissionAsync(permissionId);
            var result = new PermissionDetail
            {
                Id = permission.Id,
                Code = permission.Code,
                Name = permission.Name,
                IsEnabled = permission.IsEnabled,
            };

            return result;
        }

        public async Task<Guid> CreatePermissionAsync(CreatePermissionCommand command)
        {
            var permission = new Permission(command.Code, command.Name, command.Description, command.IsEnabled);

            permissionRepository.Add(permission);
            await unitOfWork.CommitAsync();

            return permission.Id;
        }

        public async Task UpdatePermissionAsync(UpdatePermissionCommand command)
        {
            var permission = await permissionRepository.GetPermissionAsync(command.Id);

            permission.UpdateCode(command.Code);
            permission.UpdateName(command.Name);
            permission.UpdateDescription(command.Description);

            if (command.IsEnabled)
                permission.Enable();
            else
                permission.Disable();

            permissionRepository.Update(permission);
            await unitOfWork.CommitAsync();
        }
    }
}