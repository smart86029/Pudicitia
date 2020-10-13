using System;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class RoleSummary
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}