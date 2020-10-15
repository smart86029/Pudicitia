using System;
using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization
{
    public class UpdatePermissionCommand : Command
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }
    }
}