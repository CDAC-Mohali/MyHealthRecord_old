using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHRMS.Data
{

    [Table("ActivityMaster")]
    public class ActivityMaster
    {
        [Key]
        [Required]
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
       
    } 
    [Table("Activities")]
    public class Activities
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }      
        public int ActivityId { get; set; }       
        public string Result { get; set; }
        public string PathName { get; set; }
        public decimal Distance { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    [Table("BloodGlucose")]
    public class BloodGlucose
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }       
        public string Goal { get; set; }
        public string Result { get; set; }
        public string ValueType { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
    }

    [Table("BloodPressureAndPulse")]
    public class BloodPressureAndPulse
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }    
        public string ResSystolic { get; set; }
        public string ResDiastolic { get; set; }
        public string GoalSystolic { get; set; }
        public string GoalDiastolic { get; set; }
        public DateTime CollectionDate { get; set; }
        public string GoalPulse { get; set; }
        public string ResPulse { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
    }

    [Table("Sleep")]
    public class Sleep
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Result { get; set; }
        public string Goal { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
    }

    [Table("Weight")]
    public class Weight
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Result { get; set; }
        public string Goal { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
    }
    [Table("Temperature")]
    public class Temperature
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Result { get; set; }
        public string Goal { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
    }


}
