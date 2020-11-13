using Pudicitia.Common.App;
using Pudicitia.Common.Models;

namespace Pudicitia.HR.App.Organization
{
    public class JobSummary : EntityResult
    {
        public string Title { get; set; }

        public bool IsEnabled { get; set; }
    }
}