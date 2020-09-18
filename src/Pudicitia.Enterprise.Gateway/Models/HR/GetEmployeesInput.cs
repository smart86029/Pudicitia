using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class GetEmployeesInput : PaginationInput
    {
        public Guid DepartmentId { get; set; }
    }
}