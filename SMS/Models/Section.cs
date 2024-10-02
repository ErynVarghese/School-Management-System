using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Section
    {

        public int? SectionId { get; set; }

        [Required(ErrorMessage = "SectionName is required.")]
        public string SectionName { get; set; }

        [Required(ErrorMessage = "ClassId is required.")]
        public int? ClassId { get; set; }

        [Required(ErrorMessage = "TotalSpace is required.")]
        public int? TotalSpace { get; set; }
    }

}