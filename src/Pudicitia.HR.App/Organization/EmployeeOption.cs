using System;
using Pudicitia.Common.App;

namespace Pudicitia.HR.App.Organization
{
    public class EmployeeOption : PaginationOption
    {
        public Guid DepartmentId { get; set; }
    }
}