using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.Common.Domain;
using Pudicitia.Common.EntityFrameworkCore;
using Pudicitia.Common.Events;
using Pudicitia.HR.Domain;
using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Employees;
using Pudicitia.HR.Domain.Jobs;

namespace Pudicitia.HR.Data
{
    public sealed class HRContextSeed
    {
        private readonly HRContext context;

        public HRContextSeed(HRContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SeedAsync()
        {
            if (context.Set<Department>().Any())
                return;

            var employees = GetEmployees();
            var departments = GetDepartments();
            var jobs = GetJobs();

            employees[0].AssignJob(departments[0], jobs[0], DateTime.Parse("2018-07-01"));
            employees[1].AssignJob(departments[1], jobs[1], DateTime.Parse("2018-07-01"));
            employees[2].AssignJob(departments[2], jobs[2], DateTime.Parse("2018-07-01"));
            employees[3].AssignJob(departments[3], jobs[2], DateTime.Parse("2018-07-15"));
            employees[4].AssignJob(departments[4], jobs[2], DateTime.Parse("2018-08-04"));
            employees[5].AssignJob(departments[1], jobs[3], DateTime.Parse("2018-08-04"));
            employees[6].AssignJob(departments[2], jobs[6], DateTime.Parse("2018-08-04"));
            employees[7].AssignJob(departments[3], jobs[4], DateTime.Parse("2018-08-04"));
            employees[8].AssignJob(departments[3], jobs[4], DateTime.Parse("2018-08-04"));
            employees[9].AssignJob(departments[3], jobs[4], DateTime.Parse("2018-08-04"));
            employees[10].AssignJob(departments[4], jobs[5], DateTime.Parse("2018-08-04"));
            employees[11].AssignJob(departments[4], jobs[5], DateTime.Parse("2018-08-04"));

            context.Set<Employee>().AddRange(employees);
            context.Set<Department>().AddRange(departments);
            context.Set<Job>().AddRange(jobs);

            context.LogEvents();
            await context.SaveChangesAsync();
        }

        private List<Employee> GetEmployees()
        {
            var result = new List<Employee>();
            result.Add(new Employee("William Glaze", "William", DateTime.Parse("1955-12-02"), Gender.Male, MaritalStatus.Married));
            result.Add(new Employee("Kelley Hennig", "Kelley", DateTime.Parse("1984-01-02"), Gender.Female, MaritalStatus.Married));
            result.Add(new Employee("Raymond Miller", "Raymond", DateTime.Parse("1981-11-05"), Gender.Male, MaritalStatus.Married));
            result.Add(new Employee("Zella Rogers", "Zella", DateTime.Parse("1981-11-24"), Gender.Female, MaritalStatus.Married));
            result.Add(new Employee("Joel Metcalfe", "Joel", DateTime.Parse("1985-05-12"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("Anita Bowles", "Anita", DateTime.Parse("1992-08-27"), Gender.Female, MaritalStatus.Single));
            result.Add(new Employee("Ben Buendia", "Ben", DateTime.Parse("1983-04-16"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("Kian Marsh", "Kian", DateTime.Parse("1987-03-19"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("Nancy Morrison", "Nancy", DateTime.Parse("1984-08-09"), Gender.Female, MaritalStatus.Single));
            result.Add(new Employee("Riley Hooper", "Riley", DateTime.Parse("1996-11-24"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("Jonathan Abbott", "Jonathan", DateTime.Parse("1992-02-13"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("Gina Barnes", "Gina", DateTime.Parse("1986-10-20"), Gender.Female, MaritalStatus.Married));

            return result;
        }

        private List<Department> GetDepartments()
        {
            var result = new List<Department>();
            var departmentRoot = new Department("Headquarters", true, null);
            result.Add(departmentRoot);
            result.Add(new Department("Finance", true, departmentRoot.Id));
            result.Add(new Department("Human Resources", true, departmentRoot.Id));
            result.Add(new Department("Research & Development", true, departmentRoot.Id));
            result.Add(new Department("Information Technology", true, departmentRoot.Id));

            return result;
        }

        private List<Job> GetJobs()
        {
            var result = new List<Job>();
            result.Add(new Job("Chief Executive Officer", true));
            result.Add(new Job("Chief Financial Officer", true));
            result.Add(new Job("Manager", true));
            result.Add(new Job("Accountant", true));
            result.Add(new Job("Engineer", true));
            result.Add(new Job("Technician", true));
            result.Add(new Job("Staff", true));

            return result;
        }
    }
}