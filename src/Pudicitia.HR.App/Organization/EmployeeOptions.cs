using System;
using Pudicitia.Common.App;

namespace Pudicitia.HR.App.Organization
{
    public class EmployeeOptions : PaginationOptions
    {
        public Guid DepartmentId { get; set; }
    }
}