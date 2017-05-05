using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.ViewModels
{
    public class ProfileViewModel
    {
        public PersonalViewModel oPersonalInformation { get; set; }
        public EmergencyViewModel oEmergencyInformation { get; set; }
        public EmployerViewModel oEmployerInformation { get; set; }
        public InsuranceViewModel oInsuranceInformation { get; set; }
        public LegalViewModel oLegalInformation { get; set; }
        public PreferencesViewModel oPreferences { get; set; }

        public ProfileViewModel()
        {
            oPersonalInformation = new PersonalViewModel();
            oEmergencyInformation = new EmergencyViewModel();
            oInsuranceInformation = new InsuranceViewModel();
            oEmployerInformation = new EmployerViewModel();
            oLegalInformation = new LegalViewModel();
            oPreferences = new PreferencesViewModel();
        }
    }

    public class PersonalViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "E-Mail")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        //[Required]
        [Display(Name = "Aadhaar No.")]
        [RegularExpression(@"^[0-9]{12,12}$", ErrorMessage = "Not a valid Aadhaar Number.")]
        public string Uhid { get; set; }
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }
        [Display(Name = "City/Village/Town")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string City_Vill_Town { get; set; }
        [Display(Name = "District")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string District { get; set; }
        [Display(Name = "State/UT")]
        public int State { get; set; }
        [Display(Name = "State/UT")]
        public string strState { get; set; }
        [Display(Name = "PIN")]
        [RegularExpression(@"^[0-9]{6,6}$", ErrorMessage = "PIN must contain exactly six digits")]
        public string Pin { get; set; }
        [Display(Name = "Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string Home_Phone { get; set; }
        [Display(Name = "Work Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string Work_Phone { get; set; }
        [Display(Name = "Mobile No.")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string Cell_Phone { get; set; }
        [Display(Name = "Date Of Birth")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        [Display(Name = "Date Of Birth")]
        public string strDOB { get; set; }
        public string strDOBwAge { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        public string strGender { get; set; }
        [Display(Name = "Blood Group")]
        public int BloodType { get; set; }
        public string strBloodType { get; set; }
        [Display(Name = "Ethinicity")]
        public string Ethinicity { get; set; }
        [Display(Name = "Are you differently abled?")]
        public bool DiffAbled { get; set; }
        public string strDiffAbled { get; set; }
        public int DiffAbledType { get; set; }
        public int StatusCode { get; set; }
        public string DyCertPath { get; set; }
        public string country { get; set; }
    }

    public class EmergencyViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name = "Name")]
        public string Primary_Emergency_Contact { get; set; }

        [Display(Name = "Relationship")]
        public int PC_Relationship { get; set; }
        [Display(Name = "Relationship")]
        public string strPC_Relationship { get; set; }

        [Display(Name = "Address Line 1")]
        public string PC_AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string PC_AddressLine2 { get; set; }
        [Display(Name = "City/Village/Town")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string PC_City_Vill_Town { get; set; }
        [Display(Name = "District")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string PC_District { get; set; }
        [Display(Name = "State/UT")]
        public int PC_State { get; set; }
        [Display(Name = "State/UT")]
        public string strPC_State { get; set; }
        [Display(Name = "Pin")]
        [RegularExpression(@"^[0-9]{6,6}$", ErrorMessage = "PIN must contain exactly six digits")]
        public string PC_Pin { get; set; }
        [Display(Name = "Primary Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string PC_Phone1 { get; set; }
        [Display(Name = "Secondary Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string PC_Phone2 { get; set; }

        [Display(Name = "Emergency Contact-Secondary")]
        public string Secondary_Emergency_Contact { get; set; }
        [Display(Name = "Relationship")]
        public int SC_Relationship { get; set; }
        [Display(Name = "Relationship")]
        public string strSC_Relationship { get; set; }

        [Display(Name = "Address Line 1")]
        public string SC_AddressLine1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string SC_AddressLine2 { get; set; }
        [Display(Name = "City/Village/Town")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string SC_City_Vill_Town { get; set; }
        [Display(Name = "District")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string SC_District { get; set; }
        [Display(Name = "State/UT")]
        public int SC_State { get; set; }
        [Display(Name = "State/UT")]
        public string strSC_State { get; set; }
        [Display(Name = "Pin")]
        [RegularExpression(@"^[0-9]{6,6}$", ErrorMessage = "PIN must contain exactly six digits")]
        public string SC_Pin { get; set; }
        [Display(Name = "Primary Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string SC_Phone1 { get; set; }
        [Display(Name = "Secondary Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string SC_Phone2 { get; set; }

        public int StatusCode { get; set; }
    }


    public class EmployerViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name = "Employer")]
        public string EmployerName { get; set; }
        [Display(Name = "Address Line1")]
        public string EmpAddressLine1 { get; set; }
        [Display(Name = "Address Line2")]
        public string EmpAddressLine2 { get; set; }
        
        //newly added fields
        [Display(Name = "City/Village/Town")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string EmpCity_Vill_Town { get; set; }
        [Display(Name = "District")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only")]
        public string EmpDistrict { get; set; }
        [Display(Name = "State/UT")]
        public int EmpState { get; set; }
        [Display(Name = "State/UT")]
        public string strState { get; set; }
        [Display(Name = "PIN")]
        [RegularExpression(@"^[0-9]{6,6}$", ErrorMessage = "PIN must contain exactly six digits")]
        public string EmpPin { get; set; }
        //new addition ends here


        [Display(Name = "Phone")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string EmployerPhone { get; set; }
        [Display(Name = "Designation")]
        public string EmployerOccupation { get; set; }
        [Display(Name = "CUG Code")]
        public string CUG { get; set; }
        public int StatusCode { get; set; }
    }


    public class InsuranceViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name = "Insurance Provider")]
        public string Insu_Org_Name { get; set; }
        [Display(Name = "Policy Number")]
        //[RegularExpression(@"^[0-9]*$", ErrorMessage = "Policy Number contain numbers only.")]
        public string Insu_Org_Phone { get; set; }
        [Display(Name = "Policy Name")]
        public string Insu_Org_Grp_Num { get; set; }
        [Display(Name = "Valid Till")]
        [DataType(DataType.Date)]
        public DateTime ValidTill { get; set; }
        [Display(Name = "Valid Till")]
        public string strValidTill { get; set; }
        public int StatusCode { get; set; } 
    }


    public class LegalViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name = "Do you have a Health Care Power of Attorney?")]
        public bool Power_Attorney { get; set; }
        public string strPower_Attorney { get; set; }
        public DateTime PA_DateInit { get; set; }
        [Display(Name = "Date Initiated")]
        public string strPA_DateInit { get; set; }
        [Display(Name = "Contact Person")]
        public string PA_ContactPerson { get; set; }
        [Display(Name = "Contact Person's Phone Number")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string PA_PhoneNo { get; set; }
        [Display(Name = "Do you have an Advanced Health Care Directive?")]
        public bool Care_Directive { get; set; }
        public string strCare_Directive { get; set; }
        public DateTime CD_DateInit { get; set; }
        [Display(Name = "Date Initiated")]
        public string strCD_DateInit { get; set; }
        [Display(Name = "Contact Person")]
        public string CD_ContactPerson { get; set; }
        [Display(Name = "Contact Person's Phone Number")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string CD_PhoneNo { get; set; }
        [Display(Name = "Do you have a Living Will?")]
        public bool Living_Will { get; set; }
        public string strLiving_Will { get; set; }
        public DateTime LW_DateInit { get; set; }
        [Display(Name = "Date Initiated")]
        public string strLW_DateInit { get; set; }
        [Display(Name = "Contact Person")]
        public string LW_ContactPerson { get; set; }
        [Display(Name = "Contact Person's Phone Number")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Phone Number contain only numbers and must be of 10 digits")]
        public string LW_PhoneNo { get; set; }
        public int StatusCode { get; set; }
    }


    public class PreferencesViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [Display(Name = "Preferred Hospital Name")]
        public string Pref_Hosp { get; set; }
        [Display(Name = "Preferred Hospital Address")]
        public string Prim_Care_Prov { get; set; }
        [Display(Name = "Special Requirements")]
        public string Special_Needs { get; set; }
        public int StatusCode { get; set; }

    }

    public class UsersViewModel
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

    //VisionRx View Model to be worked out later after discussion
}
