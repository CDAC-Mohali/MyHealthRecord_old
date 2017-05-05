using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{

    [Table("ImmunizationsMaster")]
    public class ImmunizationsMasters
    {

        [Key]
        [Required]
        public int ImmunizationsTypeId { get; set; }
        public string ImmunizationName { get; set; }

    }


    [Table("Immunizations")]
    public class Immunizations
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ImmunizationsTypeId { get; set; }
        public DateTime ImmunizationDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }
}
