namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class DepartmentDetail
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public Guid? ParentId { get; set; }
}
