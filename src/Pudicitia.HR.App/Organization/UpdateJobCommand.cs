namespace Pudicitia.HR.App.Organization;

public class UpdateJobCommand : Command
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
