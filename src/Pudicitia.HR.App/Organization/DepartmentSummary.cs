using System;

namespace Pudicitia.HR.App.Organization
{
    public class DepartmentSummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}