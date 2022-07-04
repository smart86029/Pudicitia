namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class UpdateDepartmentInput
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
