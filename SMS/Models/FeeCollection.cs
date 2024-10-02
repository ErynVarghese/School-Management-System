using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SMS.Models
{
    public class FeeCollection
    {

    
        public int? FeeColId { get; set; }

        
        public int StudentId { get; set; }

        
        public int ClassId { get; set; }

       
        public string Installment1 { get; set; }

        
        public string Installment2 { get; set; }

     
        public string Installment3 { get; set; }

       
        public string FeeStatus { get; set; }
        public string StudentName { get; set; }

        public string ClassName { get; set; }


    }
}