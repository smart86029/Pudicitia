using Google.Protobuf.WellKnownTypes;
using Pudicitia.Enterprise.Gateway.Models.Organization;
using Pudicitia.HR;
using Pudicitia.Identity;

namespace Pudicitia.Enterprise.Gateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "HumanResources")]
public class OrganizationController : ControllerBase
{
    private readonly ILogger<OrganizationController> _logger;
    private readonly Authorization.AuthorizationClient _authorizationClient;
    private readonly Organization.OrganizationClient _organizationClient;

    public OrganizationController(
        ILogger<OrganizationController> logger,
        Authorization.AuthorizationClient authorizationClient,
        Organization.OrganizationClient organizationClient)
    {
        _logger = logger;
        _authorizationClient = authorizationClient;
        _organizationClient = organizationClient;
    }

    [HttpGet("Departments")]
    public async Task<IActionResult> GetDepartmentsAsync()
    {
        var request = new ListDepartmentsRequest();
        var response = await _organizationClient.ListDepartmentsAsync(request);
        var result = response.Items
            .Select(x => new DepartmentSummary
            {
                Id = x.Id,
                Name = x.Name,
                IsEnabled = x.IsEnabled,
                ParentId = x.ParentId,
                HeadName = x.HeadName,
                EmployeeCount = x.EmployeeCount,
            })
            .ToList();

        return Ok(result);
    }

    [HttpGet("Departments/{id}")]
    [ActionName(nameof(GetDepartmentAsync))]
    public async Task<IActionResult> GetDepartmentAsync([FromRoute] Guid id)
    {
        var response = await _organizationClient.GetDepartmentAsync(id);
        var result = new DepartmentDetail
        {
            Id = response.Id,
            Name = response.Name,
            IsEnabled = response.IsEnabled,
            ParentId = response.ParentId,
        };

        return Ok(result);
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
    public async Task<IActionResult> UpdateDepartmentAsync([FromRoute] Guid id, [FromBody] UpdateDepartmentInput input)
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
        _ = await _organizationClient.DeleteDepartmentAsync(id);

        return NoContent();
    }

    [HttpGet("Employees")]
    public async Task<IActionResult> GetEmployeesAsync([FromQuery] GetEmployeesInput input)
    {
        var request = new PaginateEmployeesRequest
        {
            Page = input,
            Name = input.Name,
            DepartmentId = input.DepartmentId,
        };
        var response = await _organizationClient.PaginateEmployeesAsync(request);
        var result = new PaginationResult<EmployeeSummary>
        {
            Page = response.Page,
            Items = response.Items
                .Select(x => new EmployeeSummary
                {
                    Id = x.Id,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    DepartmentName = x.DepartmentName,
                    JobTitle = x.JobTitle,
                })
                .ToList(),
        };

        return Ok(result);
    }

    [HttpGet("Employees/New")]
    public async Task<IActionResult> GetNewEmployeesAsync()
    {
        var requestDepartments = new ListDepartmentsRequest
        {
            IsEnabled = true,
        };
        var responseDepartments = await _organizationClient.ListDepartmentsAsync(requestDepartments);
        var requestJobs = new ListJobsRequest();
        var responseJobs = await _organizationClient.ListJobsAsync(requestJobs);
        var result = new GetEmployeeOutput
        {
            Departments = responseDepartments.Items
                .Select(x => new DepartmentDetail
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.ParentId,
                })
                .ToList(),
            Jobs = responseJobs.Items
                .Select(x => new JobDetail
                {
                    Id = x.Id,
                    Title = x.Title,
                })
                .ToList(),
        };

        return Ok(result);
    }

    [HttpGet("Employees/{id}")]
    [ActionName(nameof(GetEmployeeAsync))]
    public async Task<IActionResult> GetEmployeeAsync([FromRoute] Guid id)
    {
        var response = await _organizationClient.GetEmployeeAsync(id);
        var result = new EmployeeDetail
        {
            Id = response.Id,
            Name = response.Name,
            DisplayName = response.DisplayName,
            BirthDate = response.BirthDate.ToDateTime(),
            Gender = response.Gender,
            MaritalStatus = response.MaritalStatus,
            UserId = response.UserId,
            DepartmentName = response.DepartmentName,
            JobTitle = response.JobTitle,
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
            BirthDate = input.BirthDate.ToTimestamp(),
            Gender = input.Gender,
            MaritalStatus = input.MaritalStatus,
        };
        var response = (Guid)await _organizationClient.CreateEmployeeAsync(request);

        return CreatedAtAction(nameof(GetEmployeeAsync), new { Id = response }, default);
    }

    [HttpPut("Employees/{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync([FromRoute] Guid id, UpdateEmployeeInput input)
    {
        if (input.UserId.HasValue)
        {
            var responseUser = await _authorizationClient.ExistUserAsync(input.UserId);
            if (!responseUser.DoesExist)
            {
                return BadRequest("User is invalid.");
            }
        }

        var request = new UpdateEmployeeRequest
        {
            Id = id,
            Name = input.Name,
            DisplayName = input.DisplayName,
            BirthDate = input.BirthDate.ToTimestamp(),
            Gender = input.Gender,
            MaritalStatus = input.MaritalStatus,
            UserId = input.UserId,
        };
        _ = await _organizationClient.UpdateEmployeeAsync(request);

        return NoContent();
    }

    [HttpGet("Jobs")]
    public async Task<IActionResult> GetJobsAsync([FromQuery] GetJobsInput input)
    {
        var request = new PaginateJobsRequest
        {
            Page = input,
            Title = input.Title,
            IsEnabled = input.IsEnabled,
        };
        var response = await _organizationClient.PaginateJobsAsync(request);
        var result = new PaginationResult<JobSummary>
        {
            Page = response.Page,
            Items = response.Items
                .Select(x => new JobSummary
                {
                    Id = x.Id,
                    Title = x.Title,
                    IsEnabled = x.IsEnabled,
                    EmployeeCount = x.EmployeeCount,
                })
                .ToList(),
        };

        return Ok(result);
    }

    [HttpGet("Jobs/{id}")]
    [ActionName(nameof(GetEmployeeAsync))]
    public async Task<IActionResult> GetJobAsync([FromRoute] Guid id)
    {
        var response = await _organizationClient.GetJobAsync(id);
        var result = new JobDetail
        {
            Id = response.Id,
            Title = response.Title,
            IsEnabled = response.IsEnabled,
        };

        return Ok(result);
    }

    [HttpPost("Jobs")]
    public async Task<IActionResult> CreateJobAsync([FromBody] CreateJobInput input)
    {
        var request = new CreateJobRequest
        {
            Title = input.Title,
            IsEnabled = input.IsEnabled,
        };
        var response = (Guid)await _organizationClient.CreateJobAsync(request);

        return CreatedAtAction(nameof(GetJobAsync), new { Id = response }, default);
    }

    [HttpPut("Jobs/{id}")]
    public async Task<IActionResult> UpdateJobAsync([FromRoute] Guid id, UpdateJobInput input)
    {
        var request = new UpdateJobRequest
        {
            Id = id,
            Title = input.Title,
            IsEnabled = input.IsEnabled,
        };
        _ = await _organizationClient.UpdateJobAsync(request);

        return NoContent();
    }

    [HttpDelete("Jobs/{id}")]
    public async Task<IActionResult> DeleteJobAsync([FromRoute] Guid id)
    {
        _ = await _organizationClient.DeleteJobAsync(id);

        return NoContent();
    }

    [HttpGet("Users")]
    public async Task<IActionResult> GetUsersAsync([FromQuery] string? userName)
    {
        var request = new ListUsersRequest
        {
            UserName = userName,
        };
        var response = await _authorizationClient.ListUsersAsync(request);
        var result = response.Items
            .Select(x => new NamedEntityResult
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();

        return Ok(result);
    }
}
