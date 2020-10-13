using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization
{
    public class RoleSummary : EntityResult
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}