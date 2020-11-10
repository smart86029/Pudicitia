using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Pudicitia.Common;
using Pudicitia.Hr;
using Pudicitia.HR.App.Organization;

namespace Pudicitia.HR.Api
{
    public class OrganizationService : Organization.OrganizationBase
    {
        private readonly ILogger<OrganizationService> logger;
        private readonly OrganizationApp organizationApp;

        public OrganizationService(
            ILogger<OrganizationService> logger,
            OrganizationApp organizationApp)
        {
            this.logger = logger;
            this.organizationApp = organizationApp;
        }

        public override async Task<ListDepartmentsResponse> ListDepartments(ListDepartmentsRequest request, ServerCallContext context)
        {
            var departments = await organizationApp.GetDepartmentsAsync();
            var items = departments.Select(x => new ListDepartmentsResponse.Types.Department
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
            });
            var result = new ListDepartmentsResponse();
            result.Items.AddRange(items);

            return result;
        }

        public override async Task<GuidRequired> CreateDepartment(CreateDepartmentRequest request, ServerCallContext context)
        {
            var command = new CreateDepartmentCommand
            {
                Name = request.Name,
                IsEnabled = request.IsEnabled,
                ParentId = request.ParentId,
            };
            var result = await organizationApp.CreateDepartmentAsync(command);

            return result;
        }

        public override async Task<Empty> UpdateDepartment(UpdateDepartmentRequest request, ServerCallContext context)
        {
            var command = new UpdateDepartmentCommand
            {
                Id = request.Id,
                Name = request.Name,
                IsEnabled = request.IsEnabled,
            };
            await organizationApp.UpdateDepartmentAsync(command);

            return new Empty();
        }

        public override async Task<Empty> DeleteDepartment(DeleteDepartmentRequest request, ServerCallContext context)
        {
            await organizationApp.DeleteDepartmentAsync(request.Id);

            return new Empty();
        }

        public override async Task<PaginateEmployeesResponse> PaginateEmployees(PaginateEmployeesRequest request, ServerCallContext context)
        {
            var options = new EmployeeOptions
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                DepartmentId = request.DepartmentId,
            };
            var employees = await organizationApp.GetEmployeesAsync(options);
            var items = employees.Items.Select(x => new PaginateEmployeesResponse.Types.Employee
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                DepartmentId = x.DepartmentId,
                JobId = x.JobId,
            });
            var result = new PaginateEmployeesResponse
            {
                PageIndex = options.PageIndex,
                PageSize = options.PageSize,
                ItemCount = employees.ItemCount,
            };
            result.Items.AddRange(items);

            return result;
        }

        public override async Task<GetEmployeeResponse> GetEmployee(GetEmployeeRequest request, ServerCallContext context)
        {
            var employee = await organizationApp.GetEmployeeAsync(request.Id);
            var result = new GetEmployeeResponse
            {
                Id = employee.Id,
                Name = employee.Name,
                DisplayName = employee.DisplayName,
                DepartmentId = employee.DepartmentId,
                JobId = employee.JobId,
            };

            return result;
        }

        public override async Task<ListJobsResponse> ListJobs(ListJobsRequest request, ServerCallContext context)
        {
            var jobs = await organizationApp.GetJobsAsync();
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
}