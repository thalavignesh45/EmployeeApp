using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebAPI.Models
{
    public partial class EmployeeDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailId { get; set; }
        [Required]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        [Required]
        public int ManagerId { get; set; }
        public bool IsActive { get; set; }

     
    }
}
