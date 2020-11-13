using System;
using System.Collections.Generic;
using Pudicitia.Common.Models;

namespace Pudicitia.Identity.App.Authorization
{
    public class RoleDetail : EntityResult
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<Guid> PermissionIds { get; set; } = new List<Guid>();
    }
}