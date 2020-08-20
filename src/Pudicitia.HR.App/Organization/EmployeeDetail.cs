using System;
using Pudicitia.Common.App;
using Pudicitia.HR.Domain;

namespace Pudicitia.HR.App.Organization
{
    public class EmployeeDetail : EntityResult
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        public MaritalStatus MaritalStatus { get; set; }

        public string DepartmentName { get; set; }

        public string JobTitleName { get; set; }
    }
}