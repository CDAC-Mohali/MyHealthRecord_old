using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EMRViewModels
{
    public class SuperViewModel
    {
        public EprescriptionViewModel EprescriptionViewModel { get; set; }
        public BloodPressureAndPulseViewModel BloodPressureAndPulseViewModel { get; set; }
        public BloodGlucoseViewModel BloodGlucoseViewModel { get; set; }
        public WeightViewModel WeightViewModel { get; set; }
        public List<VitalSignViewModel> VitalSignViewModel { get; set; }
        public MedicalHistoryViewModel MedicalHistoryViewModel { get; set; }
        public List<MedicationViewModel> MedicationViewModel { get; set; }
        public Appointment_FeesViewModel Appointment_FeesViewModel { get; set; }
        public List<AllergiesViewModel> AllergiesViewModel { get; set; }
        public List<Advice> Advice { get; set; }
        public DoctorViewModel DoctorViewModel { get; set; }
        public UsersViewModel UsersViewModel { get; set; }
        public PersonalInformationViewModel PersonalInformationViewModel { get; set; }
        public PlaceViewModel PlaceViewModel { get; set; }
        public DocPatientDetailsViewModel DocPatientDetailsViewModel { get; set; }
        public List<ProblemViewModel> ProblemViewModel { get; set; }
    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
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



    public class AppointmentViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DocId { get; set; }
        public DateTime Date { get; set; }
        public short Hours { get; set; }
        public short Mins { get; set; }
        public string meridiem { get; set; }
        public decimal NetFee { get; set; }
        public bool Visited { get; set; }
        public string strTime { get; set; }
        public string strPatientName { get; set; }
        public string MobileNo { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public Guid PrescriptionId { get; set; }
    }

    public class DoctorViewModel
    {

        public Guid docid { get; set; }
        [Required]
        public string name { get; set; }

        public string LastName { get; set; }

        public DateTime date_of_birth { get; set; }

        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number contain only numbers and must be of 10 digits.")]
        [Remote("DoesMobileExist", "Account", HttpMethod = "POST", ErrorMessage = "Mobile already registered.")]
        public string phone_number { get; set; }

        [Required]
        [Display(Name = "Email")]
        [Remote("DoesEmailExist", "Account", HttpMethod = "POST", ErrorMessage = "Email Address is already registered.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                        @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                        @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                        ErrorMessage = "Email is not valid")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }




        public string qualification_set { get; set; }
        public bool delete_flag { get; set; }
        public DateTime request_time { get; set; }
        public bool email_flag { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string Conf_password { get; set; }

        [Display(Name = "Other Speciality")]
        public string OtherSpeciality { get; set; }

        [Display(Name = "Aadhaar No")]
        public string AadhaarNo { get; set; }

        public int IsApproved { get; set; }
        public char Gender { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int OTP { get; set; }
        public PlaceViewModel PlaceViewModel { get; set; }
        public int Speciality { get; set; }
        public long ? MedicalCollegeId { get; set; }
        [Required]
        public string HospitalName { get; set; }
        public string DocFile { get; set; }
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
    }

    public class Appointment_FeesViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DocId { get; set; }
        public DateTime Date { get; set; }
        public short Hours { get; set; }
        public short Mins { get; set; }
        public string meridiem { get; set; }
        public decimal NetFee { get; set; }
        public bool Visited { get; set; }
        public Guid PrescriptionId { get; set; }
        public DoctorViewModel DoctorViewModel { get; set; }
        public UsersViewModel UsersViewModel { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    public class AllergiesViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int AllergyType { get; set; }
        public int Severity { get; set; }
        public bool Still_Have { get; set; }

        public int DurationId { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
        public string strAllergyType;
        public string strDuration;
        public string strSeverity;

    }



    public class DoctorUserMappingViewModel
    {
        [Required]
        public long Id { get; set; }
        public Guid UserId { get; set; }


        public Guid DocId { get; set; }
        public Guid PrescriptionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public UsersViewModel UsersViewModel { get; set; }
        public DoctorViewModel DoctorViewModel { get; set; }
        public EprescriptionViewModel EprescriptionViewModel { get; set; }
    }




    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "Registered Mobile No. or Email Id")]
       [Remote("DoesEmailOrMobileExist", "Account", HttpMethod = "POST", ErrorMessage = "This Mobile No. or Email-Id is not registered with PHRMS.")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public System.Guid Id { get; set; }
        public int StatusCode { get; set; }
    }

    public class FPProcessStatus
    {
        public static int SysFailure = 0;
        public static int VerificationFailure = -1;
        public static int Success = 1;
    }


    public class ContactusViewModel
    {

        public long ContactUsId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessage = "No Special Charatcers allowed")]
        public string FirstName { get; set; }

         [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessage = "No Special Charatcers allowed")]
        public string LastName { get; set; }



        [Required]
        [Display(Name = "Mobile No.")]
        //[StringLength(15, ErrorMessage = "Mobile Number length Should be less than 15")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number contain only numbers and must be of 10 digits.")]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required]
        [Display(Name = "E-Mail")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        [Display(Name = "State/UT")]
        public int Status { get; set; }
    }

    public class Events
    {
        [Column(TypeName = "text")]
        public string text { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }

    }


    public class PatientMalefemaleforGraph
    {
        public string Gender { get; set; }
        public string CreatedDate { get; set; }
    }

    public class PatientReginModel
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
        public int StateId { get;  set; }
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
        public int Sunday { get; set; }
    }


}
