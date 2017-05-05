using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHRMSAdmin.Library
{
    public class HealthConditionMaster
    {
        [Key]
        public int Id { get; set; }
        public string HealthCondition { get; set; }
    }



    [Table("HealthCondition")]
    public class HealthCondition
    {
        [NotMapped]
        internal string strProblemType;

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ConditionType { get; set; }
        [Required]
        public DateTime DiagnosisDate { get; set; }
        [Required]
        public DateTime ServiceDate { get; set; }
        public string Provider { get; set; }
        public string Notes { get; set; }
        public bool StillHaveCondition { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
    }
}
