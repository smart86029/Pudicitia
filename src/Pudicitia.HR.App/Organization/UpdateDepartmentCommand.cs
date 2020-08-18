using System;
using Pudicitia.Common.App;

namespace Pudicitia.HR.App.Organization
{
    public class UpdateDepartmentCommand : Command
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}