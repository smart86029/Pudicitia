using System;
using Pudicitia.Common.App;

namespace Pudicitia.HR.App.Organization
{
    public class DepartmentSummary : EntityResult
    {
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}