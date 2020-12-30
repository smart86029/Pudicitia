using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class CreateDepartmentInput
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public Guid ParentId { get; set; }
    }
}