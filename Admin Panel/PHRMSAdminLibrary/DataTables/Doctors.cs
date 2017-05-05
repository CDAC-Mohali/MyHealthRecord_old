using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHRMSAdmin.Library
{
    public class Doctor
    {
        [Key]
        [Required]
        public Guid docid { get; set; }
        public string name { get; set; }
        public string LastName { get; set; }
        public DateTime date_of_birth { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string qualification_set { get; set; }
        public bool delete_flag { get; set; }
        public DateTime request_time { get; set; }
        public bool email_flag { get; set; }
        public string password { get; set; }
        public int IsApproved { get; set; }
        public string Gender { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int OTP { get; set; }
        public virtual List<Places_of_Practice> Places_of_Practice { get; set; }
        public int Speciality { get; set; }
        public string OtherSpeciality { get; set; }
        public string AadhaarNo { get; set; }
        public long? MedicalCollegeId { get; set; }
        public string HospitalName { get; set; }
    }

    [Table("DocPatientDetails")]
    public class DocPatientDetails
    {
        [Key]
        public long DocPatientId { get; set; }
        public Guid DocId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AadhaarNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City_Vill_Town { get; set; }
        public string District { get; set; }
        public int? State { get; set; }
        public Guid? PHRMSUserId { get; set; }
    }
    public enum FileTypes
    {
        ProfilePic = 1, Photo
    }
    public class EMRFiles
    {
        [Key]
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileTypes FileType { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid RecId { get; set; }
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
        [ForeignKey("state")]
        public virtual States States { get; set; }
        //    [ForeignKey("docid")]
        public virtual Doctor Doctor { get; set; }
    }
}
