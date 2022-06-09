namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class GetOrganizationOutput
{
    public ICollection<DepartmentSummary> Departments { get; set; } = new List<DepartmentSummary>();

    public ICollection<JobSummary> Jobs { get; set; } = new List<JobSummary>();
}
