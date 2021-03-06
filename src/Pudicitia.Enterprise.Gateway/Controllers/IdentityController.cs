﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pudicitia.Common;
using Pudicitia.Common.Models;
using Pudicitia.Enterprise.Gateway.Models.Identity;

namespace Pudicitia.Enterprise.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<HRController> logger;
        private readonly Authorization.AuthorizationClient authorizationClient;

        public IdentityController(
            ILogger<HRController> logger,
            Authorization.AuthorizationClient authorizationClient)
        {
            this.logger = logger;
            this.authorizationClient = authorizationClient;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> GetUsersAsync([FromQuery] PaginationOptions input)
        {
            var request = new PaginateUsersRequest
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };
            var response = await authorizationClient.PaginateUsersAsync(request);
            var result = new PaginationResult<UserSummary>
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                ItemCount = response.ItemCount,
                Items = response.Items
                    .Select(x => new UserSummary
                    {
                        Id = x.Id,
                        UserName = x.UserName,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        IsEnabled = x.IsEnabled,
                    })
                   .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Users/New")]
        public async Task<IActionResult> GetNewUserAsync()
        {
            var requestRoles = new ListRolesRequest();
            var responseRoles = await authorizationClient.ListRolesAsync(requestRoles);
            var result = new GetUserOutput
            {
                User = new UserDetail
                {
                    IsEnabled = true,
                },
                Roles = responseRoles.Items
                    .Select(x => new NamedEntityResult
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Users/{id}")]
        [ActionName(nameof(GetUserAsync))]
        public async Task<IActionResult> GetUserAsync([FromRoute] Guid id)
        {
            var requestUser = new GetUserRequest
            {
                Id = id,
            };
            var responseUser = await authorizationClient.GetUserAsync(requestUser);
            var requestRoles = new ListRolesRequest();
            var responseRoles = await authorizationClient.ListRolesAsync(requestRoles);
            var result = new GetUserOutput
            {
                User = new UserDetail
                {
                    Id = responseUser.Id,
                    UserName = responseUser.UserName,
                    Name = responseUser.Name,
                    DisplayName = responseUser.DisplayName,
                    IsEnabled = responseUser.IsEnabled,
                    RoleIds = responseUser.RoleIds
                        .Select(x => (Guid)x)
                        .ToList(),
                },
                Roles = responseRoles.Items
                    .Select(x => new NamedEntityResult
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpPost("Users")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserInput input)
        {
            var roleIds = input.RoleIds.Select(x => (GuidRequired)x);
            var request = new CreateUserRequest
            {
                UserName = input.UserName,
                Password = input.Password,
                Name = input.Name,
                DisplayName = input.DisplayName,
                IsEnabled = input.IsEnabled,
            };
            request.RoleIds.AddRange(roleIds);
            var response = (Guid)await authorizationClient.CreateUserAsync(request);

            return CreatedAtAction(nameof(GetUserAsync), new { Id = response }, default);
        }

        [HttpPut("Users/{id}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute] Guid id, [FromBody] UpdateUserInput input)
        {
            if (id != input.Id)
                return BadRequest();

            var roleIds = input.RoleIds.Select(x => (GuidRequired)x);
            var request = new UpdateUserRequest
            {
                Id = input.Id,
                Password = input.Password,
                Name = input.Name,
                DisplayName = input.DisplayName,
                IsEnabled = input.IsEnabled,
            };
            request.RoleIds.AddRange(roleIds);
            var response = await authorizationClient.UpdateUserAsync(request);

            return NoContent();
        }

        [HttpDelete("Users/{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            var request = new DeleteUserRequest
            {
                Id = id,
            };
            var response = await authorizationClient.DeleteUserAsync(request);

            return NoContent();
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRolesAsync([FromQuery] PaginationOptions input)
        {
            var request = new PaginateRolesRequest
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };
            var response = await authorizationClient.PaginateRolesAsync(request);
            var result = new PaginationResult<RoleSummary>
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                ItemCount = response.ItemCount,
                Items = response.Items
                    .Select(x => new RoleSummary
                    {
                        Id = x.Id,
                        Name = x.Name,
                        IsEnabled = x.IsEnabled,
                    })
                   .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Roles/New")]
        public async Task<IActionResult> GetNewRoleAsync()
        {
            var requestPermissions = new ListPermissionsRequest();
            var responsePermissions = await authorizationClient.ListPermissionsAsync(requestPermissions);
            var result = new GetRoleOutput
            {
                Role = new RoleDetail
                {
                    IsEnabled = true,
                },
                Permissions = responsePermissions.Items
                    .Select(x => new NamedEntityResult
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Roles/{id}")]
        [ActionName(nameof(GetRoleAsync))]
        public async Task<IActionResult> GetRoleAsync([FromRoute] Guid id)
        {
            var requestRole = new GetRoleRequest
            {
                Id = id,
            };
            var responseRole = await authorizationClient.GetRoleAsync(requestRole);
            var requestPermissions = new ListPermissionsRequest();
            var responsePermissions = await authorizationClient.ListPermissionsAsync(requestPermissions);
            var result = new GetRoleOutput
            {
                Role = new RoleDetail
                {
                    Id = responseRole.Id,
                    Name = responseRole.Name,
                    IsEnabled = responseRole.IsEnabled,
                    PermissionIds = responseRole.PermissionIds
                        .Select(x => (Guid)x)
                        .ToList(),
                },
                Permissions = responsePermissions.Items
                    .Select(x => new NamedEntityResult
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpPost("Roles")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] CreateRoleInput input)
        {
            var permissionIds = input.PermissionIds.Select(x => (GuidRequired)x);
            var request = new CreateRoleRequest
            {
                Name = input.Name,
                IsEnabled = input.IsEnabled,
            };
            request.PermissionIds.AddRange(permissionIds);
            var response = (Guid)await authorizationClient.CreateRoleAsync(request);

            return CreatedAtAction(nameof(GetRoleAsync), new { Id = response }, default);
        }

        [HttpPut("Roles/{id}")]
        public async Task<IActionResult> UpdateRoleAsync([FromRoute] Guid id, [FromBody] UpdateRoleInput input)
        {
            if (id != input.Id)
                return BadRequest();

            var permissionIds = input.PermissionIds.Select(x => (GuidRequired)x);
            var request = new UpdateRoleRequest
            {
                Id = input.Id,
                Name = input.Name,
                IsEnabled = input.IsEnabled,
            };
            request.PermissionIds.AddRange(permissionIds);
            var response = await authorizationClient.UpdateRoleAsync(request);

            return NoContent();
        }

        [HttpDelete("Roles/{id}")]
        public async Task<IActionResult> DeleteRoleAsync([FromRoute] Guid id)
        {
            var request = new DeleteRoleRequest
            {
                Id = id,
            };
            var response = await authorizationClient.DeleteRoleAsync(request);

            return NoContent();
        }

        [HttpGet("Permissions")]
        public async Task<IActionResult> GetPermissionsAsync([FromQuery] PaginationOptions input)
        {
            var request = new PaginatePermissionsRequest
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };
            var response = await authorizationClient.PaginatePermissionsAsync(request);
            var result = new PaginationResult<PermissionSummary>
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                ItemCount = response.ItemCount,
                Items = response.Items
                    .Select(x => new PermissionSummary
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        IsEnabled = x.IsEnabled,
                    })
                   .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Permissions/New")]
        public async Task<IActionResult> GetNewPermissionAsync()
        {
            await Task.CompletedTask;
            var result = new PermissionDetail
            {
                IsEnabled = true,
            };

            return Ok(result);
        }

        [HttpGet("Permissions/{id}")]
        [ActionName(nameof(GetPermissionAsync))]
        public async Task<IActionResult> GetPermissionAsync([FromRoute] Guid id)
        {
            var request = new GetPermissionRequest
            {
                Id = id,
            };
            var response = await authorizationClient.GetPermissionAsync(request);
            var result = new PermissionDetail
            {
                Id = response.Id,
                Code = response.Code,
                Name = response.Name,
                Description = response.Description,
                IsEnabled = response.IsEnabled,
            };

            return Ok(result);
        }

        [HttpPost("Permissions")]
        public async Task<IActionResult> CreatePermissionAsync([FromBody] CreatePermissionInput input)
        {
            var request = new CreatePermissionRequest
            {
                Code = input.Code,
                Name = input.Name,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
            };
            var response = (Guid)await authorizationClient.CreatePermissionAsync(request);

            return CreatedAtAction(nameof(GetPermissionAsync), new { Id = response }, default);
        }

        [HttpPut("Permissions/{id}")]
        public async Task<IActionResult> UpdatePermissionAsync([FromRoute] Guid id, [FromBody] UpdatePermissionInput input)
        {
            if (id != input.Id)
                return BadRequest();

            var request = new UpdatePermissionRequest
            {
                Id = input.Id,
                Code = input.Code,
                Name = input.Name,
                Description = input.Description,
                IsEnabled = input.IsEnabled,
            };
            var response = await authorizationClient.UpdatePermissionAsync(request);

            return NoContent();
        }

        [HttpDelete("Permissions/{id}")]
        public async Task<IActionResult> DeletePermissionAsync([FromRoute] Guid id)
        {
            var request = new DeletePermissionRequest
            {
                Id = id,
            };
            var response = await authorizationClient.DeletePermissionAsync(request);

            return NoContent();
        }
    }
}