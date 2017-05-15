using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.ViewModels
{

    public class ContactTypesViewModel
    {
        public int Id { get; set; }
        public string MedContType { get; set; }
    }


    public class MedicalContactRecordsViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int sno { get; set; }
        [Display(Name = "Contact Name")]
        [RegularExpression(@"^[a-z][A-Z]$", ErrorMessage = "Name must contain alphabets only")]
        public string ContactName { get; set; }
        [Display(Name = "Contact Type")]
        public int MedContType { get; set; }
        [Display(Name = "Contact Type")]
        public string strMedContType { get; set; }

        public string EmailAddress { get; set; }

        [Display(Name = "Primary Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number must contain exactly ten digits")]
        public string PrimaryPhone { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityVillage { get; set; }
        public string PIN { get; set; }
        public string District { get; set; }
      
        public string strState { get; set; }
        public int State { get; set; }
        public string ClinicName { get; set; }
        
    }
}

