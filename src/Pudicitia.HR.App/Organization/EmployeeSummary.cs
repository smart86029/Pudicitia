using System;
using Pudicitia.Common.Models;

namespace Pudicitia.HR.App.Organization
{
    public class EmployeeSummary : EntityResult
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobId { get; set; }
    }
}