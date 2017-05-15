using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{
    [Table("TempReg")]
    public class TempReg
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public string AadhaarNo { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string MobileNo { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        public string CUG { get; set; }
        public string OTP { get; set; }
        public string Gender { get; set; }
        public int State { get; set; }
    }
}
