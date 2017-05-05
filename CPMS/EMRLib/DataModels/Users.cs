using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EMRLib.DataModels
{
    public class Users
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }
        [MaxLength(12)]
        public string AadhaarNo { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public Int16 RoleId { get; set; }
        public bool Status { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime PwdChangeDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string CUG { get; set; }
        public string ImgPath { get; set; }
        //   public virtual ICollection<FilePath> FilePaths { get; set; }
    }

    public class MedicalHistory
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }

        public string PersonalHistory { get; set; }
        public string FamilyHistory { get; set; }
        public Guid PrescriptionId { get; set; }
    }

}