using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{

    [Table("ProcedureMaster")]
    public class ProcedureMaster
    {
        [Key]
        public int Id { get; set; }
        public string ProcedureName { get; set; }
    }

    [Table("Procedures")]
    public class Procedures
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ProcedureType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
        public string SurgeonName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }
}