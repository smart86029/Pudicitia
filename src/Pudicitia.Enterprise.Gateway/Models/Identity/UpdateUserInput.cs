using System;
using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class UpdateUserInput
    {
        public Guid Id { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Guid> RoleIds { get; set; }
    }
}