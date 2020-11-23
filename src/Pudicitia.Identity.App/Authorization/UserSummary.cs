using Pudicitia.Common.Models;

namespace Pudicitia.Identity.App.Authorization
{
    public class UserSummary : EntityResult
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }
    }
}