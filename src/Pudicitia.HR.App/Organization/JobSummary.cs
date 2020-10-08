using Pudicitia.Common.App;

namespace Pudicitia.HR.App.Organization
{
    public class JobSummary : EntityResult
    {
        public string Title { get; set; }

        public bool IsEnabled { get; set; }
    }
}