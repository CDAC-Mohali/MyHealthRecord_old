using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{

    [Table("AllergyMaster")]
    public class AllergyMaster
    {
        [Key]
        public int Id { get; set; }
        public string AllergyName { get; set; }
        //public string DescriptionId { get; set; }
        //public string ConceptId { get; set; }
    }

    [Table("AllergySeverity")]
    public class AllergySeverity
    {
        [Key]
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Severity { get; set; }
    }

    [Table("Allergies")]
    public class Allergies
    {
        [NotMapped]
        internal string strAllergyType;
        [NotMapped]
        internal string strDuration;
        [NotMapped]
        internal string strSeverity;

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

    [Table("AllergyDuration")]
    public class AllergyDuration
    {
        [Key]
        public int Id { get; set; }
        public string Duration { get; set; }
    }
}
