using EMRViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMRLib.DataModels
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
    [Table("Appointment_Fees")]
    public class Appointment_Fees
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid DocId { get; set; }
        public DateTime Date { get; set; }
        public short Hours { get; set; }
        public short Mins { get; set; }
        [MaxLength(2)]
        public string meridiem { get; set; }
        public decimal NetFee { get; set; }
        public bool Visited { get; set; }
        //public bool IsApproved { get; set; }
        //public DateTime ApprovedDate { get; set; }
        //public char Gender { get; set; }
        public Guid PrescriptionId { get; set; }

        [ForeignKey("DocId")]
        public virtual Doctor doctors { get; set; }
        [ForeignKey("UserId")]
        public virtual Users users { get; set; }
    }

    [Table("Allergies")]
    public class Allergies
    {
        [NotMapped]
        public string strAllergyType;
        [NotMapped]
        public string strDuration;
        [NotMapped]
        public string strSeverity;

        public Allergies()
        {
            SourceId = 2;
            PrescriptionId = new Guid();
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int AllergyType { get; set; }
        public int Severity { get; set; }
        [Required]
        public bool Still_Have { get; set; }
        //[Required]
        //public DateTime StartDate { get; set; }
        //[Required]
        //public DateTime EndDate { get; set; }
        public int DurationId { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        //public string reactDes { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
        ////Foreign Keys
        //[ForeignKey("AllergyTypeId")]
        //public virtual AllergyMaster AllergyType { get; set; }
        //[ForeignKey("Severity")]
        //public virtual AllergySeverity Severity { get; set; }
    }
    [Table("HealthConditionMaster")]
    public class HealthConditionMaster
    {
        [Key]
        public int Id { get; set; }
        public string HealthCondition { get; set; }
    }
    [Table("MedicalContactRecords")]
    public class MedicalContactRecords
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string ContactName { get; set; }
        public int MedContType { get; set; }
        public string PrimaryPhone { get; set; }
        public string EmailAddress { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string CityVillage { get; set; }
        public string PIN { get; set; }
        public string District { get; set; }
        public int State { get; set; }
        public string ClinicName { get; set; }
    }
    [Table("DoctorUserMapping")]
    public class DoctorUserMapping
    {
        [Key]
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DocId { get; set; }
        public Guid ?PrescriptionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Users Users { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Eprescription Eprescription { get; set; }
   //     public long ?EMRUserId { get; internal set; }
    }

    [Table("HealthCondition")]
    public class HealthCondition
    {
        [NotMapped]
        internal string strProblemType;

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ConditionType { get; set; }
        [Required]
        public DateTime DiagnosisDate { get; set; }
        [Required]
        public DateTime ServiceDate { get; set; }
        public string Provider { get; set; }
        public string Notes { get; set; }
        public bool StillHaveCondition { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; internal set; }
    }
    [Table("Medication")]
    public class Medication
    {

        [NotMapped]
        public string strFrequency;
        [NotMapped]
        public string strDosValue;
        [NotMapped]
        public string strDosUnit;
        [NotMapped]
        public string strRoute;

        public Medication()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int MedicineType { get; set; }
        public bool TakingMedicine { get; set; }
        [Required]
        public DateTime PrescribedDate { get; set; }
        [Required]
        public DateTime DispensedDate { get; set; }
        public string Provider { get; set; }
        public int Route { get; set; }
        public string Strength { get; set; }
        public int DosValue { get; set; }
        public int DosUnit { get; set; }
        public int Frequency { get; set; }
        public string LabelInstructions { get; set; }
        public string Quantity { get; set; }
        public string Refills { get; set; }
        public string DaysSupply { get; set; }
        public string FillingPharmacy { get; set; }
        public string Notes { get; set; }
        public bool HideItem { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; set; }
        [NotMapped]
        public string MedicationName { get; internal set; }
    }

    [Table("MedicationMaster")]
    public class MedicationMaster
    {
        [Key]
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string RXCUI { get; set; }
        public string RXAUI { get; set; }
    }

    [Table("MedicineRoute")]
    public class MedicineRoute
    {
        [Key]
        public int Id { get; set; }
        public string Route { get; set; }
    }

    [Table("DosageValue")]
    public class DosageValue
    {
        [Key]
        public int Id { get; set; }
        public string DosValue { get; set; }
    }
    [Table("DosageUnit")]
    public class DosageUnit
    {
        [Key]
        public int Id { get; set; }
        public string DosUnit { get; set; }
    }
    [Table("FrequencyTaken")]
    public class FrequencyTaken
    {
        [Key]
        public int Id { get; set; }
        public string Frequency { get; set; }
    }


    [Table("ContactTypes")]
    public class ContactTypes
    {
        [Key]
        public int Id { get; set; }
        public string MedContType { get; set; }
    }


    [Table("AllergyMaster")]
    public class AllergyMaster
    {
        [Key]
        public int Id { get; set; }
        public string AllergyName { get; set; }
        //public string DescriptionId { get; set; }
        //public string ConceptId { get; set; }
    }

    [Table("AllergySeverity")]
    public class AllergySeverity
    {
        [Key]
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Severity { get; set; }
    }

    [Table("ImmunizationsMaster")]
    public class ImmunizationsMasters
    {

        [Key]
        [Required]
        public int ImmunizationsTypeId { get; set; }
        public string ImmunizationName { get; set; }

    }



    [Table("Immunizations")]
    public class Immunizations
    {
        public Immunizations()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ImmunizationsTypeId { get; set; }
        public DateTime ImmunizationDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }

    }

    [Table("AllergyDuration")]
    public class AllergyDuration
    {
        [Key]
        public int Id { get; set; }
        public string Duration { get; set; }
    }
    [Table("PatientDetailOTP")]
    public class PatientDetailOTP
    {
        [Key]
        public long PatientDetailOTPId { get; set; }
        public Guid PHRMSUserId { get; set; }
        public string OTP { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Type { get; set; }
    }
    [Table("PersonalInformation")]
    public class PersonalInformation
    {
        [Key]
        [Required]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    [Table("LabTestMaster")]
    public class LabTestMaster
    {
        [Key]
        public int Id { get; set; }
        public string TestName { get; set; }
    }


    [Table("LabTest")]
    public class LabTest
    {
        public LabTest()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TestId { get; set; }//from lab test master table
        [Required]
        public DateTime PerformedDate { get; set; }
        [Required]
        public string Result { get; set; }
        [Required]
        public string Unit { get; set; }
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }

    }
    [Table("ProcedureMaster")]
    public class ProcedureMaster
    {
        [Key]
        public int Id { get; set; }
        public string ProcedureName { get; set; }
    }

    [Table("Procedures")]
    public class Procedures
    {
        public Procedures()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public int ProcedureType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comments { get; set; }
        public string SurgeonName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }
    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Users User { get; set; }
        public Guid RecId { get; set; }
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
    [Table("BloodPressureAndPulse")]
    public class BloodPressureAndPulse
    {
        public BloodPressureAndPulse()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string ResSystolic { get; set; }
        public string ResDiastolic { get; set; }
        public string GoalSystolic { get; set; }
        public string GoalDiastolic { get; set; }
        public DateTime CollectionDate { get; set; }
        public string GoalPulse { get; set; }
        public string ResPulse { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid ?PrescriptionId { get; set; }
    }
    [Table("BloodGlucose")]
    public class BloodGlucose
    {
        public BloodGlucose()
        {
            SourceId = 2;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public string ValueType { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid ?PrescriptionId { get; set; }
    }

    [Table("Weight")]
    public class Weight
    {

        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Result { get; set; }
        public string Goal { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public Guid? PrescriptionId { get; set; }
    }

    [Table("ActivityMaster")]
    public class ActivityMaster
    {
        [Key]
        [Required]
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }

    }
    [Table("Activities")]
    public class Activities
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public int ActivityId { get; set; }
        public string Result { get; set; }
        public string PathName { get; set; }
        public decimal Distance { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public DateTime CollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
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

    [Table("Advice")]
    public class Advice
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public Guid ?PrescriptionId { get; set; }
        public int ModuleId { get; set; }
        public int TypeId { get; set; }
        [NotMapped]
        public string Name { get; set; }
    }


    [Table("Eprescription")]
    public class Eprescription
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }

        public string DocName { get; set; }

        public string ClinicName { get; set; }

        public string DocAddress { get; set; }

        public string DocPhone { get; set; }

        public string Prescription { get; set; }
        [Required]
        public DateTime PresDate { get; set; }
        public string FileName { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }

        public string PhysicalExamination { get; set; }
        public string ProblemDiagnosis { get; set; }
        public string OtherAdvice { get; set; }
    }

    [Table("ContactUs")]
    public class ContactUs
    {
        [Key]
        public long ContactUsId { get; set; }
        [MaxLength(20)]
       public string FirstName { get; set; }
        [MaxLength(20)]
        public string LastName { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(20)]
        public string MobileNo { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public int State { get; set; }
        public int Status { get; set; }
    }

    [Table("HealthTip")]
    public class HealthTip
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        public string Tip { get; set; }
    }
}
