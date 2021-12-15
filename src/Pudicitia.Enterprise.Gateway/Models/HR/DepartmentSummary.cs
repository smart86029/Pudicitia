namespace Pudicitia.Enterprise.Gateway.Models.HR;

public class DepartmentSummary
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }
}
