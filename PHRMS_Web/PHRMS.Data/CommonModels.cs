using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{
    [Table("PinCodes")]
    public class PinCodes
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string PostOfficeName { get; set; }
        [MaxLength(6)]
        public string Pincode { get; set; }
        [MaxLength(200)]
        public string City { get; set; }
        [MaxLength(200)]
        public string District { get; set; }
        [MaxLength(200)]
        public string State { get; set; }
    }

    [Table("HealthTip")]
    public class HealthTip
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        public string Tip { get; set; }
    }


    [Table("ContactUs")]
    public class ContactUs
    {
        [Key]
        public long ContactUsId { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string LastName { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(20)]

        public string MobileNo { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        //public string State { get; set; }
        public int State { get; set; }
        [Display(Name = "State/UT")]
        public int  Status { get; set; }
    }
}
