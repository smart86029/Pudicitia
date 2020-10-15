using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            var request = new ListRolesRequest();
            var response = await authorizationClient.ListRolesAsync(request);
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

        [HttpGet("Permissions")]
        public async Task<IActionResult> GetPermissionsAsync()
        {
            var request = new ListPermissionsRequest();
            var response = await authorizationClient.ListPermissionsAsync(request);
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