using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHRMSAdmin.Models
{
    public class PatientModel
    {



        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public string AadhaarNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
    public class PatientCustomModel
    {
        public int MaleCount;
        public int FeMaleCount;
        public int OthersCount;
        public int StateCount;
        public string StateName { get; set; }
        public string Count { get; set; }
        public string Day { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string StateId { get; internal set; }
        public int Total { get; internal set; }
    }

    public class DocPatientDetailsViewModel
    {
        public string strState;

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
        public DateTime DOB { get; set; }
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

    public class DoctorCustomModel
    {
        public int MaleCount;
        public int FeMaleCount;
        public int OthersCount;
        public int StateCount;
        public string StateName { get; set; }
        public string Count { get; set; }
        public string Day { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string StateId { get; internal set; }
        public int Total { get; internal set; }
    }
    public class WeekModel
    {

        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesdaty { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; internal set; }
        public int Jan { get; internal set; }
        public int Feb { get; internal set; }
        public int March { get; internal set; }
        public int April { get; internal set; }
        public int May { get; internal set; }
        public int June { get; internal set; }
        public int July { get; internal set; }
        public int Aug { get; internal set; }
        public int Sep { get; internal set; }
        public int Oct { get; internal set; }
        public int Nov { get; internal set; }
        public int Dec { get; internal set; }
    }

    public class PersonalInformationModel
    {

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

    public class DateViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }



}