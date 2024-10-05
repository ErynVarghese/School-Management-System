using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class Class
    {

       

        public int ClassId { get; set; }

        [Required(ErrorMessage = "ClassName is required.")]
        public string ClassName { get; set; }


        [Required(ErrorMessage = "ClassSize is required.")]
        public int ClassSize { get; set; }


        [Required(ErrorMessage = "Class Fee is required.")]
        public decimal ClassFee { get; set; }

        [Required(ErrorMessage = "No: of installments is required.")]
        public int InstallmentNo { get; set; }


    }
}