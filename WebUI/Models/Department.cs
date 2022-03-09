using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class Department
    {
        [Required]
        [MaxLength(50)]
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
    }
}
