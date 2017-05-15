
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMRViewModels
{
    public class SuperPatientDetails
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


    }
    public class BloodGlucoseViewModel
    {

        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public string ValueType { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
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
        public  VitalSignMasterViewModel VitalSignMasterViewModel { get; set; }
    }
    public class VitalSignMasterViewModel
    {
      
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Name { get; set; }
    }
    public class WeightViewModel
    {

        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; set; }
    }
    public class BloodPressureAndPulseViewModel
    {

        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ResSystolic { get; set; }
        public string ResDiastolic { get; set; }
        public string GoalSystolic { get; set; }
        public string GoalDiastolic { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string GoalPulse { get; set; }
        public string ResPulse { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }
    public class ActivitiesViewModel
    {

        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Result { get; set; }
        public string PathName { get; set; }
        public decimal Distance { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }


    public class ProceduresViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int ProcedureType { get; set; }
        public string ProcedureName { get; set; }
        public DateTime StartDate { get; set; }
        public string strStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string strEndDate { get; set; }
        public string Comments { get; set; }
        public string SurgeonName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int sno { get; set; }
        public bool DeleteFlag { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
    }
    public enum FileType
    {
        ProfilePic = 1, DisablityCert, ePrescription, LabReport, Medication, Procedure, Feedback
    }


    public class FileViewModel
    {
        public int FilePathId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public FileType FileType { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public Guid RecId { get; set; }

        public FileViewModel()
        {
            RecId = Guid.Empty;
            DeleteFlag = false;
            UserId = Guid.Empty;
            CreatedDate = DateTime.Now;
        }
    }
    public class LabTestViewModel
    {

        public Guid Id { get; set; }
        public int sno { get; set; }
        public Guid UserId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime PerformedDate { get; set; }
        public string strPerformedDate { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
    }

    public class ImmunizationViewModel
    {

        public Guid Id { get; set; }
        public int sno { get; set; }
        [Display(Name = "Immunization")]
        public string ImmunizationName { get; set; }
        public DateTime ImmunizationDate { get; set; }
        [Display(Name = "Immunization Date")]
        public string strImmunizationDate { get; set; }
        public int ImmunizationsTypeId { get; set; }
        [Display(Name = "Entered By")]
        public Guid UserId { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }


    public class MedicationViewModel
    {
        
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int sno { get; set; }
        public int MedicineType { get; set; }
        public string MedicineName { get; set; }
        public bool TakingMedicine { get; set; }
        public string strTakingMedicine { get; set; }
        public DateTime PrescribedDate { get; set; }
        public string strPrescribedDate { get; set; }
        public DateTime DispensedDate { get; set; }
        public string strDispensedDate { get; set; }
        public string Provider { get; set; }
        public int Route { get; set; }
        public string strRoute { get; set; }
        public string Strength { get; set; }
        public int DosValue { get; set; }
        public string strDosValue { get; set; }
        public int DosUnit { get; set; }
        public string strDosUnit { get; set; }
        public int Frequency { get; set; }
        public string strFrequency { get; set; }
        public string LabelInstructions { get; set; }
        public string Notes { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
        public string MedicationName { get; set; }
    }

    public class HealthConditionViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int sno { get; set; }
        public int ConditionType { get; set; }
        public string HealthCondition { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public string strDiagnosisDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string strServiceDate { get; set; }
        public string Provider { get; set; }
        public string Notes { get; set; }
        public bool StillHaveCondition { get; set; }
        public string strStillHaveCondition { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
    }

    public class AllergyViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int AllergyType { get; set; }
        public string AllergyName { get; set; }
        public bool Still_Have { get; set; }
        public int Severity { get; set; }
        public string strSeverity { get; set; }
        public int DurationId { get; set; }
        public string strDuration { get; set; }
        //public DateTime StartDate { get; set; }
        //public string strStartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public string strEndDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string strStill_Have { get; set; }
        //public string reactDes { get; set; }
        public int sno { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    public class ProblemViewModel
    {
        public Guid? PrescriptionId { get; internal set; }
        public Guid Id { get; set; }      
        public Guid UserId { get; set; }       
        public int ConditionType { get; set; }
        public string ProblemName { get; set; }
        public DateTime DiagnosisDate { get; set; }      
        public DateTime ServiceDate { get; set; }
        public string Provider { get; set; }
        public string Notes { get; set; }
        public bool StillHaveCondition { get; set; }
      
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
        public string strProblemType;
    }

        public class AllergyMasterViewModel
    {
        public int Id { get; set; }
        public string AllergyName { get; set; }
        public string DescriptionId { get; set; }
        public string ConceptId { get; set; }
    }

    public class AllergySeverityViewModel
    {
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Severity { get; set; }
    }

    public class AllergyDurationViewModel
    {
        public int Id { get; set; }
        public string Duration { get; set; }
    }
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
    public class PatientDetailOTPViewModel
    {

        public long PatientDetailOTPId { get; set; }
        public Guid PHRMSUserId { get; set; }
        public string OTP { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class PersonalViewModel
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
        //public bool DiffAbled { get; set; }
        //[NotMapped]
        //public int DAbilityType { get; set; }

        //[NotMapped]
        //public string strDOB { get; set; }
        //[NotMapped]
        //public string strDOBwAge { get; set; }
        //[NotMapped]
        //public string strGender { get; set; }
        //[NotMapped]
        //public string strBloodType { get; set; }

        //[NotMapped]
        //public string strDiffAbled { get; set; }
        //[NotMapped]
        //public int DiffAbledType { get; set; }
        //[NotMapped]
        //public int StatusCode { get; set; }
        //[NotMapped]
        //public string DyCertPath { get; set; }
        //[NotMapped]
        //public string strState { get; set; }
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
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Policy Number contain numbers only.")]
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
        [Display(Name = "Preferred Hospital Type")]
        public string Pref_Hosp { get; set; }
        [Display(Name = "Preferred Hospital")]
        public string Prim_Care_Prov { get; set; }
        [Display(Name = "Special Needs")]
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

    public class ImmunizationsMastersViewModel
    {
        public int ImmunizationsTypeId { get; set; }
        public string ImmunizationName { get; set; }

    }

    public class LabTestMasterViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
    }

    public class ProblemMasterViewModel
    {
        public int Id { get; set; }
        public string ProblemName { get; set; }
    }

    public class ProcedureMasterViewModel
    {
        public int Id { get; set; }
        public string ProcedureName { get; set; }
    }

    public class MedicationMasterViewModel
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string RXCUI { get; set; }
        public string RXAUI { get; set; }
    }

    public sealed class VitalSignParameters
    {
        public const int BloodPressure = 1, PulseRate = 2, RespiratoryRate = 3, PulseOximetry = 4, BodyTemperature = 5, Systolic = 6, Diastolic = 7;
    }

    public sealed class ReportParameters
    {
        public const int PersonalInformation = 1, EmergencyInformation = 2, EmployerInformation = 3, InsuranceInformation = 4,
        LegalInformation = 5, Preferences = 6, Allergies = 7, Immunizations = 8, Medications = 9, Procedures = 10, Tests = 11, Problems = 12, Activities = 13,
        BP = 14, Glucose = 15, Weight = 16;
    }
}
