using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.ViewModels
{
    public class ImmunizationViewModel
    {
        public ImmunizationViewModel()
        {
            SourceId = Source.WebApp;
        }
        public Guid Id { get; set; }
        public int sno { get; set; }
        [Display(Name = "Immunization")]
        public string ImmunizationName { get; set; }        
        public DateTime ImmunizationDate { get; set; }
        [Display(Name = "Immunization Date")]
        public string strImmunizationDate { get; set; }
        public int ImmunizationsTypeId { get; set; }
        [Display(Name = "Entered By")]
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    public class ImmunizationsMastersViewModel
    {     
        public int ImmunizationsTypeId { get; set; }
        public string ImmunizationName { get; set; }

    }

    
}
