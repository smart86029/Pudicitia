using System;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;

namespace Pudicitia.HR.Domain.Jobs
{
    public class Job : AggregateRoot
    {
        private Job()
        {
        }

        public Job(string title, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title can not be null");

            Title = title.Trim();
            IsEnabled = isEnabled;
        }

        public string Title { get; private set; }

        public bool IsEnabled { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Title can not be null");

            Title = title.Trim();
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }
    }
}