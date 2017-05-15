using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{

    [Table("LabTestMaster")]
    public class LabTestMaster
    {
        [Key]
        public int Id { get; set; }
        public string TestName { get; set; }
    }

    [Table("LabTest")]
    public class LabTest
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TestId { get; set; }//from lab test master table
        [Required]
        public DateTime PerformedDate { get; set; }
        [Required]
        public string Result { get; set; }
        [Required]
        public string Unit { get; set; }
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

}



//using System;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace PHRMS.Data
//{

//    [Table("LabTestMaster")]
//    public class LabTestMaster
//    {
//        [Key]
//        public int Id { get; set; }
//        public string TestName { get; set; }
//    }

//    [Table("LabTestPrescribed")]
//    public class LabTestPrescribed
//    {
//        [Key]
//        [Required]
//        public Guid Id { get; set; }
//        public Guid UserId { get; set; }
//        [Required]       
//        public int TestId { get; set; } 
//        public string TestName { get; set; }
//        public string Comments { get; set; }
//        public DateTime CreatedDate { get; set; }
//        public DateTime ModifiedDate { get; set; }
//        public bool DeleteFlag { get; set; }

//    }

//    [Table("LabTestResult")]
//    public class LabTestResult
//    {
//        [Key]
//        [Required]
//        public Guid Id { get; set; }
//        public Guid UserId { get; set; }
//        public string TestName { get; set; }
//        public DateTime PerformedDate { get; set; }
//        public string Result { get; set; }
//        public string Unit { get; set; }
//        public DateTime CreatedDate { get; set; }
//        public DateTime ModifiedDate { get; set; }
//        public bool DeleteFlag { get; set; }
//        public Guid PresTestId { get; set; }
//    }
//}
