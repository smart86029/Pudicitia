﻿using System;
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

        [HttpGet("Organization")]
        public async Task<IActionResult> GetOrganizationAsync()
        {
            var requesttDepartments = new ListDepartmentsRequest();
            var responsetDepartments = await organizationClient.ListDepartmentsAsync(requesttDepartments);
            var requesttJobs = new ListJobsRequest();
            var responsetJobs = await organizationClient.ListJobsAsync(requesttJobs);
            var result = new OrganizationOutput
            {
                Departments = responsetDepartments.Items
                    .Select(x => new DepartmentSummary
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ParentId = x.ParentId,
                    })
                    .ToList(),
                Jobs = responsetJobs.Items
                    .Select(x => new JobSummary
                    {
                        Id = x.Id,
                        Title = x.Title,
                        IsEnabled = x.IsEnabled,
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

        [HttpPut("Departments/{id}")]
        public async Task<IActionResult> UpdateDepartmentAsync([FromRoute] Guid id, [FromBody] UpdateDepartmenInput input)
        {
            if (id != input.Id)
                return BadRequest();

            var request = new UpdateDepartmentRequest
            {
                Id = input.Id,
                Name = input.Name,
                IsEnabled = input.IsEnabled,
            };
            var response = await organizationClient.UpdateDepartmentAsync(request);

            return NoContent();
        }

        [HttpDelete("Departments/{id}")]
        public async Task<IActionResult> DeleteDepartmentAsync([FromRoute] Guid id)
        {
            var request = new DeleteDepartmentRequest
            {
                Id = id,
            };
            var response = await organizationClient.DeleteDepartmentAsync(request);

            return NoContent();
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
                        JobId = x.JobId,
                    })
                    .ToList(),
            };

            return Ok(result);
        }
    }
}