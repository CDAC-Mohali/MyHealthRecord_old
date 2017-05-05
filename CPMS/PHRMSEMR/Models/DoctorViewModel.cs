using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHRMSEMR.Models
{
    //public class DoctorViewModel
    //{
    //    public Guid docid { get; set; }
    //    public string name { get; set; }
    //    public DateTime date_of_birth { get; set; }
    //    public string phone_number { get; set; }
    //    public string email { get; set; }
    //    public string qualification_set { get; set; }
    //    public bool delete_flag { get; set; }
    //    public DateTime request_time { get; set; }
    //    public bool email_flag { get; set; }
    //    public string password { get; set; }
    //    public int IsApproved { get; set; }
    //    public char Gender { get; set; }
    //    public DateTime? ApprovedDate { get; set; }
    //    public int OTP { get; set; }
    //    public PlaceViewModel PlaceViewModel { get; set; }

    //}

    //public class PlaceViewModel
    //{
    //    public Guid id { get; set; }
    //    public Guid docid { get; set; }
    //    public string name { get; set; }
    //    public int private_practice { get; set; }
    //    public string AddressLine1 { get; set; }
    //    public string AddressLine2 { get; set; }
    //    public string city { get; set; }
    //    public int state { get; set; }
    //    public string pincode { get; set; }
    //    public string strState { get; set; }
    //    public string license_number { get; set; }
    //    public string licence_copy { get; set; }
    //}

    public class DocPatientDetailsViewModel
    {
        public DocPatientDetailsViewModel()
        {
            DocPatientId = 0;
            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            EmailAddress = "";
            AadhaarNumber = "";
            Address1 = "";
            Address2 = "";
            City_Vill_Town = "";
            District = "";
            CreatedDate = DateTime.Now;
        }

        public long DocPatientId { get; set; }
        public Guid DocId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB {
            get
            {
                return DOB;
            }
            set
            {
                DOB = value;
                strDOB = value.ToString("dd/MM/yyyy");
            }
        }
        public string strDOB { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string AadhaarNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City_Vill_Town { get; set; }
        public string District { get; set; }
        public int State { get; set; }
        public Guid? PHRMSUserId { get; set; }
    }
}