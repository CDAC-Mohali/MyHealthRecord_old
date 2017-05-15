using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.Data
{
    [Table("ContactTypes")]
    public class ContactTypes
    {
        [Key]
        public int Id { get; set; }
        public string MedContType { get; set; }        

    }

    [Table("MedicalContactRecords")]
    public class MedicalContactRecords
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string ContactName { get; set; }
        public int MedContType { get; set; }
        public string PrimaryPhone { get; set; }
        public string EmailAddress { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityVillage { get; set; }
        public string PIN { get; set; }
        public string District { get; set; }
        public int State { get; set; }
        public string ClinicName { get; set; }
    }


}
