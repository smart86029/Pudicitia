using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class UpdateDepartmenInput
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}