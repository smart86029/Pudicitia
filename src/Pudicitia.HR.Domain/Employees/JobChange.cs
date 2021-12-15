namespace Pudicitia.HR.Domain.Employees
{
    public class JobChange : Entity
    {
        private JobChange()
        {
        }

        public JobChange(Guid employeeId, Guid departmentId, Guid jobTitleId, DateTime startOn)
        {
            EmployeeId = employeeId;
            DepartmentId = departmentId;
            JobTitleId = jobTitleId;
            StartOn = startOn;
        }

        public Guid EmployeeId { get; private set; }

        public Guid DepartmentId { get; private set; }

        public Guid JobTitleId { get; private set; }

        public DateTime StartOn { get; private set; }

        public DateTime? EndOn { get; private set; }
    }
}
