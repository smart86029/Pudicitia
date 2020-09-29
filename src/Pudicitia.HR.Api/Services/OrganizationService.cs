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
            var items = departments.Select(x => new Department
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

        public override async Task<ListEmployeesResponse> ListEmployees(ListEmployeesRequest request, ServerCallContext context)
        {
            var options = new EmployeeOption
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                DepartmentId = request.DepartmentId,
            };
            var employees = await organizationApp.GetEmployeesAsync(options);
            var items = employees.Items.Select(x => new Employee
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                DepartmentId = x.DepartmentId,
                JobId = x.JobId,
            });
            var result = new ListEmployeesResponse
            {
                PageIndex = options.PageIndex,
                PageSize = options.PageSize,
                ItemCount = employees.ItemCount,
            };
            result.Items.AddRange(items);

            return result;
        }
    }
}