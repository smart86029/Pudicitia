using Pudicitia.Common.Models;

namespace Pudicitia.Identity.App.Authorization
{
    public class PermissionSummary : EntityResult
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}