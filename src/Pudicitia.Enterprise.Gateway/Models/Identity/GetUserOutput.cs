using System.Collections.Generic;
using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class GetUserOutput
    {
        public UserDetail User { get; set; }

        public ICollection<NamedEntityResult> Roles { get; set; }
    }
}