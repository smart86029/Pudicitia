namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class EmployeeSummary
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;
}
