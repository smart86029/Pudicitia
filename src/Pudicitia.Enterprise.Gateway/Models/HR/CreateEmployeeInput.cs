using System;

namespace Pudicitia.Enterprise.Gateway.Models.HR
{
    public class CreateEmployeeInput
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public DateTime BirthDate { get; set; }

        public int Gender { get; set; }

        public int MaritalStatus { get; set; }
    }
}