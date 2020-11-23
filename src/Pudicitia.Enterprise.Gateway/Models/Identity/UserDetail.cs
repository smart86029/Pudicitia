using System;
using System.Collections.Generic;
using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class UserDetail : EntityResult
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Guid> RoleIds { get; set; }
    }
}