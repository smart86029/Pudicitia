using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pudicitia.Common;
using Pudicitia.Enterprise.Gateway.Models;
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

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRolesAsync()
        {
            var request = new PaginateRolesRequest();
            var response = await authorizationClient.PaginateRolesAsync(request);
            var result = new PaginationOutput<RoleSummary>
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
        [ActionName(nameof(GetRoleAsync))]
        public async Task<IActionResult> GetNewRoleAsync([FromRoute] Guid id)
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
                    .Select(x => new NamedEntity
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
                    .Select(x => new NamedEntity
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
            await authorizationClient.DeleteRoleAsync(request);

            return NoContent();
        }

        [HttpGet("Permissions")]
        public async Task<IActionResult> GetPermissionsAsync()
        {
            var request = new PaginatePermissionsRequest();
            var response = await authorizationClient.PaginatePermissionsAsync(request);
            var result = new PaginationOutput<PermissionSummary>
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
    }
}