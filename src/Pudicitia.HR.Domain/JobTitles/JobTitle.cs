using System;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;

namespace Pudicitia.HR.Domain.JobTitles
{
    public class JobTitle : AggregateRoot
    {
        private JobTitle()
        {
        }

        public JobTitle(string name, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

            Name = name.Trim();
            IsEnabled = isEnabled;
        }

        public string Name { get; private set; }

        public bool IsEnabled { get; private set; }

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not be null");

            Name = name.Trim();
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