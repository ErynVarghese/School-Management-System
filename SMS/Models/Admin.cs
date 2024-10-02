using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Admin
    {
        
        public int? AdminId { get; set; }

        [Required(ErrorMessage = "AdmUserName is required.")]
        public string AdminUsername { get; set; }

        [Required(ErrorMessage = "AdmName is required.")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "AdmPassword is required.")]
        public int? AdminPassword { get; set; }

    }
}