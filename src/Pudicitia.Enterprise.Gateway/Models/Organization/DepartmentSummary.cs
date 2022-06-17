namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class DepartmentSummary
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public Guid? ParentId { get; set; }

    public string HeadName { get; set; } = string.Empty;

    public int EmployeeCount { get; set; }
}
