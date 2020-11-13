using System;
using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class GetEmployeesInput : PaginationOptions
    {
        public Guid DepartmentId { get; set; }
    }
}