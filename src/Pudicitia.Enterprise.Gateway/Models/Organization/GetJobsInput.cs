namespace Pudicitia.Enterprise.Gateway.Models.Organization;

public class GetJobsInput : Pagination
{
    public string? Title { get; set; }

    public bool? IsEnabled { get; set; }
}
