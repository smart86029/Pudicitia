using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class PermissionSummary : EntityResult
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}