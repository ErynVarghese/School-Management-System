using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class FeeStructure
    {
        public int? FeeId { get; set; }

        [Required(ErrorMessage = "Class Id is required.")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Total Fee is required.")]
        public decimal TotalFee { get; set; }

        [Required(ErrorMessage = "Installment1 is required.")]
        public decimal Installment1 { get; set; }

        public decimal Installment2 { get; set; }
  
        public decimal Installment3 { get; set; }
    }
}