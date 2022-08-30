namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class GetEmployeeOutput
{
    public EmployeeDetail Employee { get; set; } = new();

    public ICollection<DepartmentDetail> Departments { get; set; } = new List<DepartmentDetail>();

    public ICollection<JobDetail> Jobs { get; set; } = new List<JobDetail>();
}
