using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace PHRMS.Data
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    [Table("Users")]
    public class Users
    {
        [Key]
        [Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        //[Required]
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
        public virtual ICollection<FilePath> FilePaths { get; set; }
    }

    public class Places_of_Practice
    {
        [Key]
        [Required]
        public Guid id { get; set; }
        [Required]
        public Guid docid { get; set; }
        public string license_number { get; set; }
        public string name { get; set; }
        public int private_practice { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string city { get; set; }
        public int state { get; set; }
        [MaxLength(6)]
        public string pincode { get; set; }

        //    [ForeignKey("docid")]
        public virtual Doctor doctors { get; set; }
    }
    [Table("VitalSign")]
    public class VitalSign
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }
        [MaxLength(5)]
        public string Result { get; set; }
        public int SourceId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public Guid PrescriptionId { get; set; }

        [ForeignKey("Type")]
        public virtual VitalSignMaster vitalSignMaster { get; set; }
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
    [Table("VitalSignMaster")]
    public class VitalSignMaster
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Name { get; set; }
    }
    [Table("DoctorUserMapping")]
    public class DoctorUserMapping
    {
        [Key]
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DocId { get; set; }
        public Guid? PrescriptionId { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("DocId")]
        public virtual Doctor Doctor { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Eprescription Eprescription { get; set; }
    }

    [Table("PersonalInformation")]
    public class PersonalInformation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
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
        [Column(TypeName = "char")]
        public string Gender { get; set; }
        public int BloodType { get; set; }
        public string Ethinicity { get; set; }
        public bool DiffAbled { get; set; }
        public int DAbilityType { get; set; }
        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }


    [Table("EmergencyInformation")]
    public class EmergencyInformation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        //Primary Contacts Fields
        public string Primary_Emergency_Contact { get; set; }
        public int PC_Relationship { get; set; }
        public string PC_AddressLine1 { get; set; }
        public string PC_AddressLine2 { get; set; }
        public string PC_City_Vill_Town { get; set; }
        public string PC_District { get; set; }
        public int PC_State { get; set; }
        public string PC_Pin { get; set; }
        public string PC_Phone1 { get; set; }
        public string PC_Phone2 { get; set; }

        //Secondary Contacts Fields
        public string Secondary_Emergency_Contact { get; set; }
        public int SC_Relationship { get; set; }
        public string SC_AddressLine1 { get; set; }
        public string SC_AddressLine2 { get; set; }
        public string SC_City_Vill_Town { get; set; }
        public string SC_District { get; set; }
        public int SC_State { get; set; }
        public string SC_Pin { get; set; }
        public string SC_Phone1 { get; set; }
        public string SC_Phone2 { get; set; }

        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }

    [Table("EmployerInformation")]
    public class EmployerInformation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string EmployerName { get; set; }
        public string EmpAddressLine1 { get; set; }
        public string EmpAddressLine2 { get; set; }

        //Newly added after review
        public string EmpCity_Vill_Town { get; set; }
        public string EmpDistrict { get; set; }
        public int EmpState { get; set; }
        public string EmpPin { get; set; }
        //new addition ends here
        public string EmployerPhone { get; set; }
        public string EmployerOccupation { get; set; }
        public string CUG { get; set; }

        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }

    [Table("InsuranceInformation")]
    public class InsuranceInformation
    {

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Insu_Org_Name { get; set; }
        public string Insu_Org_Phone { get; set; }
        public string Insu_Org_Grp_Num { get; set; }
        //public string Insu_Org_Mem_num { get; set; }
        public DateTime ValidTill { get; set; }

        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }

    [Table("LegalInformation")]
    public class LegalInformation
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public bool Power_Attorney { get; set; }
        public DateTime PA_DateInit { get; set; }
        public string PA_ContactPerson { get; set; }
        public string PA_PhoneNo { get; set; }
        public bool Care_Directive { get; set; }
        public DateTime CD_DateInit { get; set; }
        public string CD_ContactPerson { get; set; }
        public string CD_PhoneNo { get; set; }
        public bool Living_Will { get; set; }
        public DateTime LW_DateInit { get; set; }
        public string LW_ContactPerson { get; set; }
        public string LW_PhoneNo { get; set; }

        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }

    [Table("Preferences")]
    public class Preferences
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }

        public string Pref_Hosp { get; set; }
        public string Prim_Care_Prov { get; set; }
        public string Special_Needs { get; set; }

        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }



    //To be disscussed and done later
    //    [Table("VisionRx")]
    //public class VisionRx {
    //    //Foriegn Key for UserId Required here
    //    [ForeignKey("UserId")]
    //    public virtual Users User { get; set; }
    //}

    [Table("LoginLog")]
    public class LoginLog
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }
        public bool LoginStatus { get; set; }
        public string SessionId { get; set; }
        public string IPAddress { get; set; }
        public int RequestSource { get; set; }

        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }

    }

    [Table("BloodGroups")]
    public class BloodGroups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

    }

    [Table("States")]
    public class States
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

    }


    [Table("Relationship")]
    public class Relationship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Relation { get; set; }

    }



    [Table("DisabilityTypes")]
    public class DisabilityTypes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

    }

}
