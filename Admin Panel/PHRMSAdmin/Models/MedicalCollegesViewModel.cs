using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace PHRMSAdmin.Models
{
    public class MedicalCollegesViewModel
    {
        public long MedicalCollegeId { get; set; }
        [Required]
        [Display(Name = "Hospital")]
        public string MedicalCollegeName { get; set; }
        [Required]
        public string State { get; set; }
    }
}
