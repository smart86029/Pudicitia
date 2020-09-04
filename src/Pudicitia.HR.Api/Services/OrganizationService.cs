using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
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
                Id = x.Id.ToString(),
                Name = x.Name,
                ParentId = x.ParentId?.ToString(),
            });
            var result = new ListDepartmentsResponse();
            result.Items.AddRange(items);

            return result;
        }

        public override Task<Empty> CreateDepartment(CreateDepartmentRequest request, ServerCallContext context)
        {
            return base.CreateDepartment(request, context);
        }
    }
}
