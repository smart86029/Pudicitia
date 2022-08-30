namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class EmployeeSummary : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public Guid? UserId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;
}
