using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pudicitia.Common.Extensions;
using Pudicitia.Enterprise.Gateway.Models;
using Pudicitia.Enterprise.Gateway.Models.HR;

namespace Pudicitia.Enterprise.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        private readonly ILogger<HRController> logger;
        private readonly Organization.OrganizationClient organizationClient;

        public HRController(
            ILogger<HRController> logger,
            Organization.OrganizationClient organizationClient)
        {
            this.logger = logger;
            this.organizationClient = organizationClient;
        }

        [HttpGet("Departments")]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            var request = new ListDepartmentsRequest();
            var response = await organizationClient.ListDepartmentsAsync(request);
            var result = new ListOutput<DepartmentSummary>
            {
                Items = response.Items
                    .Select(x => new DepartmentSummary
                    {
                        Id = x.Id.ToGuid(),
                        Name = x.Name,
                        ParentId = x.ParentId?.ToGuid(),
                    })
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Employees")]
        public async Task<IActionResult> GetEmployeesAsync([FromQuery] GetEmployeesInput input)
        {
            var request = new ListEmployeesRequest
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                DepartmentId = input.DepartmentId.ToString(),
            };
            var response = await organizationClient.ListEmployeesAsync(request);
            var result = new PaginationOutput<EmployeeSummary>
            {
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
                ItemCount = response.ItemCount,
                Items = response.Items
                    .Select(x => new EmployeeSummary
                    {
                        Id = x.Id.ToGuid(),
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        DepartmentId = x.DepartmentId.ToGuid(),
                        JobTitleId = x.JobTitleId.ToGuid(),
                    })
                    .ToList(),
            };

            return Ok(result);
        }
    }
}