using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pudicitia.Enterprise.Gateway.Models.HR;

namespace Pudicitia.Enterprise.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HumanResources")]
public class HRController : ControllerBase
{
    private readonly ILogger<HRController> _logger;
    private readonly Organization.OrganizationClient _organizationClient;

    public HRController(
        ILogger<HRController> logger,
        Organization.OrganizationClient organizationClient)
    {
        _logger = logger;
        _organizationClient = organizationClient;
    }

    [HttpGet("Organization")]
    public async Task<IActionResult> GetOrganizationAsync()
    {
        var requesttDepartments = new ListDepartmentsRequest();
        var responsetDepartments = await _organizationClient.ListDepartmentsAsync(requesttDepartments);
        var requesttJobs = new ListJobsRequest();
        var responsetJobs = await _organizationClient.ListJobsAsync(requesttJobs);
        var result = new GetOrganizationOutput
        {
            Departments = responsetDepartments.Items
                .Select(x => new DepartmentSummary
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsEnabled = x.IsEnabled,
                    ParentId = x.ParentId,
                })
                .ToList(),
            Jobs = responsetJobs.Items
                .Select(x => new JobSummary
                {
                    Id = x.Id,
                    Title = x.Title,
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
    public async Task<IActionResult> CreateDepartmentAsync([FromBody] CreateDepartmentInput input)
    {
        var request = new CreateDepartmentRequest
        {
            Name = input.Name,
            IsEnabled = input.IsEnabled,
            ParentId = input.ParentId,
        };
        var response = (Guid)await _organizationClient.CreateDepartmentAsync(request);

        return CreatedAtAction(nameof(GetDepartmentAsync), new { Id = response }, default);
    }

    [HttpPut("Departments/{id}")]
    public async Task<IActionResult> UpdateDepartmentAsync([FromRoute] Guid id, [FromBody] UpdateDepartmenInput input)
    {
        if (id != input.Id)
        {
            return BadRequest();
        }

        var request = new UpdateDepartmentRequest
        {
            Id = input.Id,
            Name = input.Name,
            IsEnabled = input.IsEnabled,
        };
        _ = await _organizationClient.UpdateDepartmentAsync(request);

        return NoContent();
    }

    [HttpDelete("Departments/{id}")]
    public async Task<IActionResult> DeleteDepartmentAsync([FromRoute] Guid id)
    {
        var request = new DeleteDepartmentRequest
        {
            Id = id,
        };
        _ = await _organizationClient.DeleteDepartmentAsync(request);

        return NoContent();
    }

    [HttpGet("Employees")]
    public async Task<IActionResult> GetEmployeesAsync([FromQuery] GetEmployeesInput input)
    {
        var request = new PaginateEmployeesRequest
        {
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            DepartmentId = input.DepartmentId,
        };
        var response = await _organizationClient.PaginateEmployeesAsync(request);
        var result = new PaginationResult<EmployeeSummary>
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

    [HttpPost("Employees")]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeInput input)
    {
        var request = new CreateEmployeeRequest
        {
            Name = input.Name,
            DisplayName = input.DisplayName,
            BirthDate = Timestamp.FromDateTime(input.BirthDate),
            Gender = input.Gender,
            MaritalStatus = input.MaritalStatus,
        };
        var response = (Guid)await _organizationClient.CreateEmployeeAsync(request);

        return CreatedAtAction(nameof(GetDepartmentAsync), new { Id = response }, default);
    }
}
