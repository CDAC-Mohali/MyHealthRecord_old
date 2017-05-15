using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace PHRMSAdmin.Library
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


    public class PersonalInformation
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Uhid { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City_Vill_Town { get; set; }
        public string District { get; set; }
        public int State { get; set; }
        public string Pin { get; set; }
        public string Home_Phone { get; set; }
        public string Work_Phone { get; set; }
        public string Cell_Phone { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public int BloodType { get; set; }
        public string Ethinicity { get; set; }
        public bool DiffAbled { get; set; }
        public int DAbilityType { get; set; }
   
    }



    public class States
    {
        [Key]
   
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string StateId { get; set; }
    }

}