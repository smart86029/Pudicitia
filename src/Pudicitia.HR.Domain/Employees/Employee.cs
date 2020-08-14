using System;
using System.Collections.Generic;
using System.Linq;
using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.JobTitles;

namespace Pudicitia.HR.Domain.Employees
{
    public class Employee : Person
    {
        private Employee() : base()
        {
        }

        public Employee(string name, string displayName, DateTime birthDate, Gender gender, MaritalStatus maritalStatus) : base(name, displayName, birthDate, gender, maritalStatus)
        {
        }

        private List<JobChange> jobChanges = new List<JobChange>();

        public Guid DepartmentId => jobChanges.SingleOrDefault(j => j.StartOn <= DateTime.UtcNow && j.EndOn >= DateTime.UtcNow)?.DepartmentId ?? jobChanges.Last().DepartmentId;

        public Guid JobTitleId => jobChanges.SingleOrDefault(j => j.StartOn <= DateTime.UtcNow && j.EndOn >= DateTime.UtcNow)?.JobTitleId ?? jobChanges.Last().JobTitleId;

        public bool IsEmployed => jobChanges.Any(j => j.StartOn <= DateTime.UtcNow && j.EndOn >= DateTime.UtcNow);

        public IReadOnlyCollection<JobChange> JobChanges => jobChanges.AsReadOnly();

        public void AssignJob(Department department, JobTitle jobTitle, DateTime startOn)
        {
            jobChanges.Add(new JobChange(Id, department.Id, jobTitle.Id, startOn));
        }
    }
}