using System;
using System.Collections.Generic;
using System.Text;

namespace Pudicitia.HR.App.Organization
{
    public class CreateDepartmentCommand
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public Guid ParentId { get; set; }
    }
}
