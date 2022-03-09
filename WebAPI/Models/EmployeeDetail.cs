using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class EmployeeDetail
    {
        public EmployeeDetail()
        {
           
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public int DepartmentId { get; set; }
        public int ManagerId { get; set; }
        public bool IsActive { get; set; }

        public virtual MstDepartment Department { get; set; }
    
    }
}
