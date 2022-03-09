using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public  class Employee
    {
       
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string EmailId { get; set; }
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }

        [Range(0, int.MaxValue)]
        public int ManagerId { get; set; }
        public List<Department> DepartmentList { get; set; }
        public string SearchString { get; set; }
    }
}
