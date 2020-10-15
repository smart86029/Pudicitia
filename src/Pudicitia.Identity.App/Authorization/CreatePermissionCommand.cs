using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization
{
    public class CreatePermissionCommand : Command
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }
    }
}