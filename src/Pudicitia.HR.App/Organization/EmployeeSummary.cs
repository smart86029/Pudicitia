using System;
using Pudicitia.Common.App;

namespace Pudicitia.HR.App.Organization
{
    public class EmployeeSummary : EntityResult
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid JobTitleId { get; set; }
    }
}