namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class UpdateJobInput
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
