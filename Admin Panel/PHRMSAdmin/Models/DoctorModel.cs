using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PHRMSAdmin.Models
{
    public class DoctorModel
    {
        public string license_number;

        public Guid docid { get; set; }
        public string name { get; set; }
        public string LastName { get; set; }
        public DateTime date_of_birth { get; set; }
        public string phone_number { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        public string qualification_set { get; set; }
        public bool delete_flag { get; set; }
        public DateTime request_time { get; set; }
        public bool email_flag { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        [DataType(DataType.Password)]
        public string Conf_password { get; set; }
        [Display(Name = "Other Speciality")]
        public string OtherSpeciality { get; set; }
        [Display(Name = "Aadhaar No")]
        public string AadhaarNo { get; set; }
        public int IsApproved { get; set; }
        public string Gender { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int OTP { get; set; }
        public PlaceViewModel PlaceViewModel { get; set; }
        public int Speciality { get; set; }
        [Required]
        public long? MedicalCollegeId { get; set; }
        [Required]
        public string HospitalName { get; set; }
        public string DocFile { get; set; }
        public string ClinicName { get; internal set; }
        public string AddressLine1 { get; internal set; }
        public string AddressLine2 { get; internal set; }
        public string City { get; internal set; }
        public string State { get; internal set; }
        public string PIN { get; internal set; }
    }
    public class PlaceViewModel
    {
        public Guid id { get; set; }
        public Guid docid { get; set; }
        public string name { get; set; }
        public int private_practice { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string city { get; set; }
        public int state { get; set; }
        public string pincode { get; set; }
        public string strState { get; set; }
        public string license_number { get; set; }
        public string licence_copy { get; set; }
        //  public state PlaceViewModel { get; set; }
    }
}