using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMRLib.DataModels
{
    public class MedicalColleges
    {
        [Key]
        public long MedicalCollegeId { get; set; }
        public string MedicalCollegeName { get; set; }
        public string State { get; set; }
    }
}
