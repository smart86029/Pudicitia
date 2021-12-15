namespace Pudicitia.Enterprise.Gateway.Models.HR;

public class UpdateDepartmenInput
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
