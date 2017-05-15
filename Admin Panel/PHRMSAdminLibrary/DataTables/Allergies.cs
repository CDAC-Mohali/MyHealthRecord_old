using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHRMSAdmin.Library
{
    public class AllergyMaster
    {
        [Key]
        public int Id { get; set; }
        public string AllergyName { get; set; }

    }

    [Table("Allergies")]
    public class Allergies
    {
        [NotMapped]
        public string strAllergyType;
        [NotMapped]
        public string strDuration;
        [NotMapped]
        public string strSeverity;

        public Allergies()
        {
            SourceId = 2;
            PrescriptionId = new Guid();
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int AllergyType { get; set; }
        public int Severity { get; set; }
        [Required]
        public bool Still_Have { get; set; }
        //[Required]
        //public DateTime StartDate { get; set; }
        //[Required]
        //public DateTime EndDate { get; set; }
        public int DurationId { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        //public string reactDes { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
        ////Foreign Keys
        //[ForeignKey("AllergyTypeId")]
        //public virtual AllergyMaster AllergyType { get; set; }
        //[ForeignKey("Severity")]
        //public virtual AllergySeverity Severity { get; set; }
    }
}
