using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Department
    {
       
        public int? DeptId { get; set; }

        [Required(ErrorMessage = "DeptName is required.")]
        public string DeptName { get; set; }
    }
}