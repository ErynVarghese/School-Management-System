using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Employer
    {
     
        public int? EmpId { get; set; }

        [Required(ErrorMessage = "EmpName is required.")]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "ContactNo is required.")]
        public int? ContactNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        [Range(typeof(DateTime), "1970-01-01", "2005-12-31", ErrorMessage = "DOB must be between 01/01/1970 and 12/31/2005.")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "DeptId is required.")]
        public int? DeptId { get; set; }

        [Required(ErrorMessage = "DOJ is required.")]
        [Range(typeof(DateTime), "1980-01-01", "2024-12-31", ErrorMessage = "DOJ must be between 01/01/1980 and 12/31/2024.")]
        public DateTime? DOJ { get; set; }

        [Required(ErrorMessage = "EmpUserName is required.")]
        public string EmpUsername { get; set; }

        [Required(ErrorMessage = "EmpPassword is required.")]
        public int? EmpPassword { get; set; }
    }
}