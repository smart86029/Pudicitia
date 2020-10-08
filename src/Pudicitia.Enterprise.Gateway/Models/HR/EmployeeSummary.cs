using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class EmployeeSummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobId { get; set; }
    }
}