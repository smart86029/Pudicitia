﻿using Pudicitia.Common.Models;

namespace Pudicitia.Enterprise.Gateway.Models.Identity
{
    public class RoleSummary : EntityResult
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }
    }
}