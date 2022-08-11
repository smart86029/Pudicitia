namespace Pudicitia.HR.App.Organization;

public class CreateJobCommand : Command
{
    public string Title { get; set; } = string.Empty;

    public bool IsEnabled { get; set; }
}
