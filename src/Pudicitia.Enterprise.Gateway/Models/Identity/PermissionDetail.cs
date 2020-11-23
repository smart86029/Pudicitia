using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class PermissionDetail : EntityResult
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }
    }
}