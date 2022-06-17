namespace Pudicitia.HR.Domain.Employees
{
    public class JobChange : Entity
    {
        private JobChange()
        {
        }

        public JobChange(Guid employeeId, Guid departmentId, Guid jobId, bool isHead, DateTime startOn)
        {
            EmployeeId = employeeId;
            DepartmentId = departmentId;
            JobId = jobId;
            IsHead = isHead;
            StartOn = startOn;
        }

        public Guid EmployeeId { get; private set; }

        public Guid DepartmentId { get; private set; }

        public Guid JobId { get; private set; }

        public bool IsHead { get; private set; }

        public DateTime StartOn { get; private set; }

        public DateTime? EndOn { get; private set; }
    }
}
