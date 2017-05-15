using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMRLib.DataModels
{
    [Table("VitalSign")]
    public class VitalSign
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(5)]
        public string Result { get; set; }
        public int SourceId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public Guid PrescriptionId { get; set; }

        [ForeignKey("Type")]
        public virtual VitalSignMaster vitalSignMaster { get; set; }
    }

    [Table("VitalSignMaster")]
    public class VitalSignMaster
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Name { get; set; }
    }
}
