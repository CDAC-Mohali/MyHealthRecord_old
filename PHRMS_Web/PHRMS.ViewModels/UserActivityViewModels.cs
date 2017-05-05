using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PHRMS.ViewModels
{
    public class UserActivityViewModels
    {
        public Guid Id { get; set; }
        public int Module { get; set; }
        public string strModuleName { get; set; }
        public int Operation { get; set; }
        public string strOperationName { get; set; }
        public DateTime TimeStamp { get; set; }
        public string strTimeStamp { get; set; }
        public Guid ActivityId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ActivityName { get; set; }
    }


    public class UserShareRecordViewModels
    {
        public long UserRecordId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string strChecks { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ValidUpto { get; set; }

        public string ImagePath { get; set; }
        public string DocEmail { get; set; }
        public string DocPhone { get; set; }
        public virtual UsersViewModel UsersViewModel { get; set; }
        public Guid MedicalContactId { get; set; }
        public string DoctorName { get; set; }
        public string ClinicName { get; set; }
        public string Query { get; set; }
    }
    public class VitalSignViewModel
    {


        public Guid Id { get; set; }
        public int Type { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Result { get; set; }
        public int SourceId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public Guid PrescriptionId { get; set; }
        public VitalSignMasterViewModel VitalSignMasterViewModel { get; set; }
    }
    public class VitalSignMasterViewModel
    {

        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Name { get; set; }
    }
    public class MedicalHistoryViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PersonalHistory { get; set; }
        public string FamilyHistory { get; set; }
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
    public class Advice
    {
        public long Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public int ModuleId { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
    }
    public class PersonalInformationViewModel
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
        //Foriegn Key for UserId Required here
        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; }
    }
    public class DoctorViewModel
    {

        public Guid docid { get; set; }
        public string name { get; set; }
        public DateTime date_of_birth { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string qualification_set { get; set; }
        public bool delete_flag { get; set; }
        public DateTime request_time { get; set; }
        public bool email_flag { get; set; }
        public string password { get; set; }
        public int IsApproved { get; set; }
        public char Gender { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int OTP { get; set; }
        public PlaceViewModel PlaceViewModel { get; set; }
    }
    public class AdviceViewModel
    {
        
        [Required]
        public long Id { get; set; }
        public Guid PrescriptionId { get; set; }
        public int ModuleId { get; set; }
        public int TypeId { get; set; }
        [NotMapped]
        public string Name { get; set; }
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
        public List<AdviceViewModel> Advice { get; set; }
        public DoctorViewModel DoctorViewModel { get; set; }
        public UsersViewModel UsersViewModel { get; set; }
        public PersonalInformationViewModel PersonalInformationViewModel { get; set; }
        public PlaceViewModel PlaceViewModel { get; set; }
        public DocPatientDetailsViewModel DocPatientDetailsViewModel { get; set; }
        public List<HealthConditionViewModel> HealthConditionViewModel { get; set; }
    }
    public class SuperReportShareViewModel
    {
        public PersonalViewModel PersonalViewModel { get; set; }
        public EmergencyViewModel EmergencyViewModel { get; set; }
        public EmployerViewModel EmployerViewModel { get; set; }
        public InsuranceViewModel InsuranceViewModel { get; set; }
        public PreferencesViewModel PreferencesViewModel { get; set; }
        public List<AllergyViewModel> AllergyViewModel { get; set; }
        public List<HealthConditionViewModel> HealthConditionViewModel { get; set; }
        public List<MedicationViewModel> MedicationViewModel { get; set; }
        public List<ImmunizationViewModel> ImmunizationViewModel { get; set; }
        public List<LabTestViewModel> LabTestViewModel { get; set; }
        public List<ProceduresViewModel> ProceduresViewModel { get; set; }
        public List<ActivitiesViewModel> ActivitiesViewModel { get; set; }
        public List<BloodPressureAndPulseViewModel> BloodPressureAndPulseViewModel { get; set; }
        public List<WeightViewModel> WeightViewModel { get; set; }
        public List<BloodGlucoseViewModel> BloodGlucoseViewModel { get; set; }
        public LegalViewModel LegalViewModel { get; set; }
        public string Status { get; set; }
        public string ImagePath { get; set; }
        public Guid MedicalContactId { get; set; }
        public Guid UserId { get; set; }
        public ShareReportFeedBackViewModel ShareReportFeedBack { get; set; }
        public long UserRecordId { get; set; }
        public string ClinincName { get; set; }
        public string DoctorName { get; set; }
        public string Query { get; set; }
        public DateTime QueryDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
    }

    public class ShareFeedBack
    {
        public Guid MedicalContactId { get; set; }
        public string FeedBack { get; set; }
        public Guid UserId { get; set; }
        public long UserRecordId { get; set; }
       

    }

    public class ShareReportNotificationViewModel
    {
        public long NotificationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isViewedByDoctor { get; set; }
        public bool isPrescribedByDoctor { get; set; }
        public bool isNotificationViewed { get; set; }

        public long UserRecordId { get; set; }
        public virtual UserShareRecordViewModels UserShareRecordViewModels { get; set; }

    }
    public class NotificationViewModel
    {
        public long userRecordId { get; set; }
        public string MedicalContactName { get; set; }
        public string Message { get; set; }
    }
}
