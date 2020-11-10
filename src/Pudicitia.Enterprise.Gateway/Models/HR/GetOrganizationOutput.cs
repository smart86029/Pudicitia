using System.Collections.Generic;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class GetOrganizationOutput
    {
        public ICollection<DepartmentSummary> Departments { get; set; }

        public ICollection<JobSummary> Jobs { get; set; }
    }
}