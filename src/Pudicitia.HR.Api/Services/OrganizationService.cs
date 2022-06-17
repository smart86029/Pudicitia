using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Pudicitia.Common;
using Pudicitia.Hr;
using Pudicitia.HR.App.Organization;
using Pudicitia.HR.Domain;

namespace Pudicitia.HR.Api;

public class OrganizationService : Organization.OrganizationBase
{
    private readonly ILogger<OrganizationService> _logger;
    private readonly OrganizationApp _organizationApp;

    public OrganizationService(
        ILogger<OrganizationService> logger,
        OrganizationApp organizationApp)
    {
        _logger = logger;
        _organizationApp = organizationApp;
    }

    public override async Task<ListDepartmentsResponse> ListDepartments(
        ListDepartmentsRequest request,
        ServerCallContext context)
    {
        var departments = await _organizationApp.GetDepartmentsAsync();
        var items = departments.Select(x => new ListDepartmentsResponse.Types.Department
        {
            Id = x.Id,
            Name = x.Name,
            IsEnabled = x.IsEnabled,
            ParentId = x.ParentId,
            HeadName = x.HeadName,
            EmployeeCount = x.EmployeeCount,
        });
        var result = new ListDepartmentsResponse();
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<GetDepartmentResponse> GetDepartment(
        GetDepartmentRequest request,
        ServerCallContext context)
    {
        var department = await _organizationApp.GetDepartmentAsync(request.Id);
        var result = new GetDepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            IsEnabled = department.IsEnabled,
            ParentId = department.ParentId,
        };

        return result;
    }

    public override async Task<GuidRequired> CreateDepartment(
        CreateDepartmentRequest request,
        ServerCallContext context)
    {
        var command = new CreateDepartmentCommand
        {
            Name = request.Name,
            IsEnabled = request.IsEnabled,
            ParentId = request.ParentId,
        };
        var result = await _organizationApp.CreateDepartmentAsync(command);

        return result;
    }

    public override async Task<Empty> UpdateDepartment(
        UpdateDepartmentRequest request,
        ServerCallContext context)
    {
        var command = new UpdateDepartmentCommand
        {
            Id = request.Id,
            Name = request.Name,
            IsEnabled = request.IsEnabled,
        };
        await _organizationApp.UpdateDepartmentAsync(command);

        return new Empty();
    }

    public override async Task<Empty> DeleteDepartment(
        DeleteDepartmentRequest request,
        ServerCallContext context)
    {
        await _organizationApp.DeleteDepartmentAsync(request.Id);

        return new Empty();
    }

    public override async Task<PaginateEmployeesResponse> PaginateEmployees(
        PaginateEmployeesRequest request,
        ServerCallContext context)
    {
        var options = new EmployeeOptions
        {
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            DepartmentId = request.DepartmentId,
        };
        var employees = await _organizationApp.GetEmployeesAsync(options);
        var items = employees.Items.Select(x => new PaginateEmployeesResponse.Types.Employee
        {
            Id = x.Id,
            Name = x.Name,
            DisplayName = x.DisplayName,
            DepartmentName = x.DepartmentName,
            JobTitle = x.JobTitle,
        });
        var result = new PaginateEmployeesResponse
        {
            PageIndex = employees.PageIndex,
            PageSize = employees.PageSize,
            ItemCount = employees.ItemCount,
        };
        result.Items.AddRange(items);

        return result;
    }

    public override async Task<GetEmployeeResponse> GetEmployee(
        GetEmployeeRequest request,
        ServerCallContext context)
    {
        var employee = await _organizationApp.GetEmployeeAsync(request.Id);
        var result = new GetEmployeeResponse
        {
            Id = employee.Id,
            Name = employee.Name,
            DisplayName = employee.DisplayName,
            BirthDate = employee.BirthDate.ToTimestamp(),
            Gender = (int)employee.Gender,
            MaritalStatus = (int)employee.MaritalStatus,
        };

        return result;
    }

    public override async Task<GuidRequired> CreateEmployee(
        CreateEmployeeRequest request,
        ServerCallContext context)
    {
        var command = new CreateEmployeeCommand
        {
            Name = request.Name,
            DisplayName = request.DisplayName,
            BirthDate = request.BirthDate.ToDateTime(),
            Gender = (Gender)request.Gender,
            MaritalStatus = (MaritalStatus)request.MaritalStatus,
        };
        var result = await _organizationApp.CreateEmployeeAsync(command);

        return result;
    }

    public override async Task<Empty> UpdateEmployee(
        UpdateEmployeeRequest request,
        ServerCallContext context)
    {
        var command = new UpdateEmployeeCommand
        {
            Id = request.Id,
            Name = request.Name,
            DisplayName = request.DisplayName,
            BirthDate = request.BirthDate.ToDateTime(),
            Gender = (Gender)request.Gender,
            MaritalStatus = (MaritalStatus)request.MaritalStatus,
        };
        await _organizationApp.UpdateEmployeeAsync(command);

        return new Empty();
    }

    public override async Task<ListJobsResponse> ListJobs(
        ListJobsRequest request,
        ServerCallContext context)
    {
        var jobs = await _organizationApp.GetJobsAsync();
        var items = jobs.Select(x => new ListJobsResponse.Types.Job
        {
            Id = x.Id,
            Title = x.Title,
        });
        var result = new ListJobsResponse();
        result.Items.AddRange(items);

        return result;
    }
}
