using System;
using Pudicitia.Common.Domain;
using Pudicitia.Common.Exceptions;

namespace Pudicitia.HR.Domain
{
    public abstract class Person : AggregateRoot
    {
        protected Person()
        {
        }

        protected Person(string name, string displayName, DateTime birthDate, Gender gender, MaritalStatus maritalStatus)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not bet null");
            if (string.IsNullOrWhiteSpace(displayName))
                throw new DomainException("Display name can not bet null");
            if (birthDate > DateTime.UtcNow)
                throw new DomainException("Birth date must less than now");

            Name = name.Trim();
            DisplayName = displayName.Trim();
            BirthDate = birthDate.Date;
            Gender = gender;
            MaritalStatus = maritalStatus;
        }

        public string Name { get; private set; }

        public string DisplayName { get; private set; }

        public DateTime BirthDate { get; private set; }

        public Gender Gender { get; private set; }

        public MaritalStatus MaritalStatus { get; private set; }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name can not bet null");

            Name = name.Trim();
        }

        public void UpdateDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                throw new DomainException("Display name can not bet null");

            DisplayName = displayName.Trim();
        }

        public void UpdateBirthDate(DateTime birthDate)
        {
            if (birthDate > DateTime.UtcNow)
                throw new DomainException("Birth date must less than now");

            BirthDate = birthDate.Date;
        }

        public void UpdateGender(Gender gender)
        {
            Gender = gender;
        }

        public void UpdateMaritalStatus(MaritalStatus maritalStatus)
        {
            MaritalStatus = maritalStatus;
        }
    }
}