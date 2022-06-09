namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class EmployeeSummary
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }

    public Guid JobId { get; set; }
}
