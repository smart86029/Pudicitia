using System;
using Pudicitia.Common.Domain;

namespace Pudicitia.HR.Domain.Employees
{
    public class EmployeeCreated : DomainEvent
    {
        public EmployeeCreated(Guid employeeId)
        {
            EmployeeId = employeeId;
        }

        public Guid EmployeeId { get; private init; }
    }
}