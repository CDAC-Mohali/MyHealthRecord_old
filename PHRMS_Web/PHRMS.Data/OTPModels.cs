using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{
    [Table("PullSMS")]
    public class PullSMS
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(50)]
        public string Message { get; set; }
        [StringLength(50)]
        public string Time_Stamp { get; set; }
        [StringLength(50)]
        public string operatorName { get; set; }
        [StringLength(50)]
        public string areaCode { get; set; }
        [StringLength(50)]
        public string mobileNumber { get; set; }
    }
}
