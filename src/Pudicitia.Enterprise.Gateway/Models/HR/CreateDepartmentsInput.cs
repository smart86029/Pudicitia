using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class CreateDepartmenInput
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public Guid ParentId { get; set; }
    }
}