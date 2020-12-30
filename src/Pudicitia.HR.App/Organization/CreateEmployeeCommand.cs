using System;
using Pudicitia.HR.Domain;

namespace Pudicitia.HR.App.Organization
{
    public class CreateEmployeeCommand
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public MaritalStatus MaritalStatus { get; set; }
    }
}