using System.Collections.Generic;
using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class GetRoleOutput
    {
        public RoleDetail Role { get; set; }

        public ICollection<NamedEntityResult> Permissions { get; set; }
    }
}