using Pudicitia.Common.Models;

namespace Pudicitia.Identity.App.Authorization;

    public class UserSummary : EntityResult
    {
        public string UserName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
    }
