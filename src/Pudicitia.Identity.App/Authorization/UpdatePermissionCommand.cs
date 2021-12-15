using System;
using Pudicitia.Common.App;

namespace Pudicitia.Identity.App.Authorization;
    public class UpdatePermissionCommand : Command
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
    }
