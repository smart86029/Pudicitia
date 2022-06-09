namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class CreateDepartmentInput
{
    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }

    public Guid ParentId { get; set; }
}
