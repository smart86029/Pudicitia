namespace Pudicitia.HR.App.Organization;

public class EmployeeSummary : EntityResult
{
    public string Name { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string DepartmentName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;
}
