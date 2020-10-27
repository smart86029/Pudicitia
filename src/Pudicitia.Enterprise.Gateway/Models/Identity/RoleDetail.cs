using System;
using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class RoleDetail
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}