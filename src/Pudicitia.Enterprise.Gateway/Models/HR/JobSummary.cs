using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class JobSummary
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsEnabled { get; set; }
    }
}