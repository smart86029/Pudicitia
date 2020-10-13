using System;
using System.Collections.Generic;
using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization
{
    public class CreateRoleCommand : Command
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Guid> PermissionIds { get; set; }
    }
}