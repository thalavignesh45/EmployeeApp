using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPI.Models
{
    public partial class MstDepartment
    {
        public MstDepartment()
        {
            EmployeeDetails = new HashSet<EmployeeDetail>();
        }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<EmployeeDetail> EmployeeDetails { get; set; }
    }
}
