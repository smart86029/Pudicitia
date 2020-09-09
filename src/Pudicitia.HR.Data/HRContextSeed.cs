using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pudicitia.HR.Domain;
using Pudicitia.HR.Domain.Departments;
using Pudicitia.HR.Domain.Employees;
using Pudicitia.HR.Domain.JobTitles;

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
            var jobTitles = GetJobTitles();

            employees[0].AssignJob(departments[0], jobTitles[0], DateTime.Parse("2018-07-01"));
            employees[1].AssignJob(departments[1], jobTitles[1], DateTime.Parse("2018-07-01"));
            employees[2].AssignJob(departments[2], jobTitles[1], DateTime.Parse("2018-07-01"));
            employees[3].AssignJob(departments[3], jobTitles[1], DateTime.Parse("2018-07-15"));
            employees[4].AssignJob(departments[4], jobTitles[1], DateTime.Parse("2018-08-04"));
            employees[5].AssignJob(departments[3], jobTitles[2], DateTime.Parse("2018-08-04"));
            employees[6].AssignJob(departments[3], jobTitles[3], DateTime.Parse("2018-08-04"));
            employees[7].AssignJob(departments[4], jobTitles[3], DateTime.Parse("2018-08-04"));
            employees[8].AssignJob(departments[3], jobTitles[2], DateTime.Parse("2018-08-04"));
            employees[9].AssignJob(departments[3], jobTitles[2], DateTime.Parse("2018-08-04"));
            employees[10].AssignJob(departments[4], jobTitles[3], DateTime.Parse("2018-08-04"));
            employees[11].AssignJob(departments[3], jobTitles[3], DateTime.Parse("2018-08-04"));

            context.Set<Employee>().AddRange(employees);
            context.Set<Department>().AddRange(departments);
            context.Set<JobTitle>().AddRange(jobTitles);

            await context.SaveChangesAsync();
        }

        private List<Employee> GetEmployees()
        {
            var result = new List<Employee>();
            result.Add(new Employee("梁瑋文", "William", DateTime.Parse("1955-12-02"), Gender.Male, MaritalStatus.Married));
            result.Add(new Employee("程怡萱", "Bract", DateTime.Parse("1984-01-02"), Gender.Female, MaritalStatus.Married));
            result.Add(new Employee("鄭致堯", "Yao", DateTime.Parse("1981-11-05"), Gender.Male, MaritalStatus.Married));
            result.Add(new Employee("趙安庭", "Ann", DateTime.Parse("1981-11-24"), Gender.Female, MaritalStatus.Married));
            result.Add(new Employee("牛軒銘", "Kent", DateTime.Parse("1985-05-12"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("常鈺茹", "Chelsea", DateTime.Parse("1992-08-27"), Gender.Female, MaritalStatus.Single));
            result.Add(new Employee("葉志宏", "Bodega", DateTime.Parse("1983-04-16"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("蔡育德", "Ducky", DateTime.Parse("1987-03-19"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("蘇于倩", "Shirley", DateTime.Parse("1984-08-09"), Gender.Female, MaritalStatus.Single));
            result.Add(new Employee("陳祥軒", "Natter", DateTime.Parse("1996-11-24"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("范達維", "Davie", DateTime.Parse("1992-02-13"), Gender.Male, MaritalStatus.Single));
            result.Add(new Employee("雷淑芬", "Virginia", DateTime.Parse("1986-10-20"), Gender.Female, MaritalStatus.Married));

            return result;
        }

        private List<Department> GetDepartments()
        {
            var result = new List<Department>();
            var departmentRoot = new Department("總公司", true, null);
            result.Add(departmentRoot);
            result.Add(new Department("管理部", true, departmentRoot.Id));
            result.Add(new Department("會計部", true, departmentRoot.Id));
            result.Add(new Department("研發部", true, departmentRoot.Id));
            result.Add(new Department("資訊部", true, departmentRoot.Id));

            return result;
        }

        private List<JobTitle> GetJobTitles()
        {
            var result = new List<JobTitle>();
            result.Add(new JobTitle("總經理", true));
            result.Add(new JobTitle("經理", true));
            result.Add(new JobTitle("高級工程師", true));
            result.Add(new JobTitle("中級工程師", true));

            return result;
        }
    }
}