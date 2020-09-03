using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Pudicitia.Enterprise.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> logger;
        private readonly Organization.OrganizationClient organizationClient;

        public OrganizationController(
            ILogger<OrganizationController> logger,
            Organization.OrganizationClient organizationClient)
        {
            this.logger = logger;
            this.organizationClient = organizationClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = organizationClient.ListDepartments(new ListDepartmentsRequest());

            await Task.CompletedTask;
            return Ok(result);
        }
    }
}