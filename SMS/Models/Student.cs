using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Student
    {
    
        public int StudentId { get; set; }

        [Required(ErrorMessage = "StudentName is required.")]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        [Range(typeof(DateTime), "1980-01-01", "2024-12-31", ErrorMessage = "DOB must be between 01/01/1980 and 12/31/2024.")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "ClassId is required.")]
        public int? ClassId { get; set; }

        [Required(ErrorMessage = "SectionId is required.")]
        public int? SectionId { get; set; }

        [Required(ErrorMessage = "FatherName is required.")]
        public string FatherName { get; set; }

        [Required(ErrorMessage = "ContactNo is required.")]
        public int? ContactNo { get; set; }

        [Required(ErrorMessage = "Student Address is required.")]
        public string StudentAddress { get; set; }

        [Required(ErrorMessage = "StudentUsername is required.")]
        public string StudentUsername { get; set; }

        [Required(ErrorMessage = "StudentPassword is required.")]
        public int StudentPassword { get; set; }

        [Required(ErrorMessage = "StudentFee is required.")]
        public decimal StudentFee { get; set; }

        public string ImageName { get; set; }

    }
}