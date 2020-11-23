using System;
using System.Collections.Generic;
using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization
{
    public class CreateUserCommand : Command
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Guid> RoleIds { get; set; }
    }
}