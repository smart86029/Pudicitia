using System;
using System.Collections.Generic;
using System.Linq;
using Pudicitia.Common.Extensions;
using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Domain.Employees
{
    public class Employee : Person
    {
        private List<JobChange> jobChanges = new List<JobChange>();

        private Employee()
        {
        }

        public Employee(string name, string displayName, DateTime birthDate, Gender gender, MaritalStatus maritalStatus) :
            base(name, displayName, birthDate, gender, maritalStatus)
        {
        }

        private JobChange LastJobChange =>
            jobChanges.SingleOrDefault(x => DateTime.UtcNow.IsBetween(x.StartOn, x.EndOn)) ?? jobChanges.Last();

        public Guid DepartmentId { get; private set; }

        public Guid JobId { get; private set; }

        public bool IsEmployed => jobChanges.Any(x => DateTime.UtcNow.IsBetween(x.StartOn, x.EndOn));

        public IReadOnlyCollection<JobChange> JobChanges => jobChanges.AsReadOnly();

        public void AssignJob(Department department, Job job, DateTime startOn)
        {
            jobChanges.Add(new JobChange(Id, department.Id, job.Id, startOn));
            DepartmentId = department.Id;
            JobId = job.Id;
        }
    }
}