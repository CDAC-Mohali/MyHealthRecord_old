using System;

namespace PHRMSEMR.Models
{
    public class PatientModel
    {
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

    public class WeekModel
    {

        public int Monday { get; set; }
        public int Tuesday { get; set; }
        public int Wednesdaty { get; set; }
        public int Thursday { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Sunday { get; internal set; }
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

    public class  PatientDataforGraphModel
    {
        public int Count { get; set; }
        public string Date { get; set; }
    }
}