using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class GetRoleOutput
    {
        public RoleDetail Role { get; set; }

        public ICollection<NamedEntity> Permissions { get; set; }
    }
}