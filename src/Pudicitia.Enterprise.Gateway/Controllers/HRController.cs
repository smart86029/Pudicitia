using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
                        Id = x.Id,
                        Name = x.Name,
                        ParentId = x.ParentId,
                    })
                    .ToList(),
            };

            return Ok(result);
        }

        [HttpGet("Departments/{id}")]
        [ActionName(nameof(GetDepartmentAsync))]
        public async Task<IActionResult> GetDepartmentAsync([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpPost("Departments")]
        public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmenInput input)
        {
            var request = new CreateDepartmentRequest
            {
                Name = input.Name,
                IsEnabled = input.IsEnabled,
                ParentId = input.ParentId,
            };
            var response = (Guid)await organizationClient.CreateDepartmentAsync(request);

            return CreatedAtAction(nameof(GetDepartmentAsync), new { Id = response }, default);
        }

        [HttpGet("Employees")]
        public async Task<IActionResult> GetEmployeesAsync([FromQuery] GetEmployeesInput input)
        {
            var request = new ListEmployeesRequest
            {
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                DepartmentId = input.DepartmentId,
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
                        Id = x.Id,
                        Name = x.Name,
                        DisplayName = x.DisplayName,
                        DepartmentId = x.DepartmentId,
                        JobTitleId = x.JobId,
                    })
                    .ToList(),
            };

            return Ok(result);
        }
    }
}