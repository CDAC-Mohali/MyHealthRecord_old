using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{
  
    [Table("Eprescription")]
    public class Eprescription
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string DocName { get; set; }
        [Required]
        public string ClinicName { get; set; }
        [Required]
        public string DocAddress { get; set; }
        [Required]
        public string DocPhone { get; set; }
        [Required]
        public string Prescription { get; set; }
        [Required]
        public DateTime PresDate { get; set; }
        public string FileName { get; set; }                   
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
        public string PhysicalExamination { get; set; }
        public string ProblemDiagnosis { get; set; }
        public string OtherAdvice { get; set; }
    }
}
