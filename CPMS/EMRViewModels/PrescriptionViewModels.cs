using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMRViewModels
{
    public class EMRComplete
    {
        private EMRVitals _EMRVitals;
        private PatinetInfo _PatinetInfo;
        private MedicalHistoryViewModel _MedicalHistory;
        private List<MedicationViewModel> _Medications;
        private AppointmentViewModel _Appointment;
        private List<AllergyViewModel> _Allergies;
        private List<ProblemViewModel> _Problem;

        //private ProceduresViewModel _Procedures;
        //private ImmunizationViewModel _Immunizations;
        private List<Advice> _Advice;
        private int _SourceId;
        private Guid _UserId;
        private Guid _DocId;
        private string _AdviceHtml;
        private string _MedicationHtml;
        private string _AllergiesHtml;
        private string _PhysicalExamination;
        private string _ProblemDiagnosis;
        private string _OtherAdvice;
        public string PhysicalExamination { get { return _PhysicalExamination; } set { _PhysicalExamination = value; } }
        public string ProblemDiagnosis { get { return _ProblemDiagnosis; } set { _ProblemDiagnosis = value; } }
        public string OtherAdvice { get { return _OtherAdvice; } set { _OtherAdvice = value; } }
        //private string _Prescription_Copy;

        public Guid DocId
        {
            get { return _DocId; }
            set { _DocId = value; }
        }

        public int SourceId
        {
            get { return _SourceId; }
            set { _SourceId = value; }
        }

        public Guid UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string AdviceHtml
        {
            get { return _AdviceHtml; }
            set { _AdviceHtml = value; }
        }
        public string MedicationHtml
        {
            get { return _MedicationHtml; }
            set { _MedicationHtml = value; }
        }
        public string AllergiesHtml
        {
            get { return _AllergiesHtml; }
            set { _AllergiesHtml = value; }
        }

        public EMRVitals EMRVitals
        {
            get { return _EMRVitals; }
            set { _EMRVitals = value; }
        }

        public PatinetInfo PatinetInfo
        {
            get { return _PatinetInfo; }
            set {_PatinetInfo = value; }
        }

        public List<Advice> Advice
        {
            get { return _Advice; }
            set { _Advice = value; }
        }

        public List<AllergyViewModel> Allergies
        {
            get { return _Allergies; }
            set { _Allergies = value; }
        }

        public List<ProblemViewModel> Problem
        {
            get { return _Problem; }
            set { _Problem = value; }
        }

     

        //public ImmunizationViewModel Immunizations
        //{
        //    get { return _Immunizations; }
        //    set { _Immunizations = value; }
        //}

        //public ProceduresViewModel Procedures
        //{
        //    get { return _Procedures; }
        //    set { _Procedures = value; }
        //}

        public MedicalHistoryViewModel MedicalHistory
        {
            get { return _MedicalHistory; }
            set { _MedicalHistory = value; }
        }

        //public LabTestViewModel LabTest
        //{
        //    get { return _LabTest; }
        //    set { _LabTest = value; }
        //}

        public List<MedicationViewModel> Medications
        {
            get { return _Medications; }
            set { _Medications = value; }
        }

        public AppointmentViewModel Appointment
        {
            get { return _Appointment; }
            set { _Appointment = value; }
        }

        public DoctorViewModel DoctorDetail { get; set; }
        public DocPatientDetailsViewModel PatientDetail { get; set; }
        public PlaceViewModel PlaceViewModel { get; set; }

        //public string Prescription_Copy
        //{
        //    get { return _Prescription_Copy; }
        //    set { _Prescription_Copy = value; }
        //}
        public long EMRUserId { get; set; }
        
    }
    public class PatinetInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }
    public class EMRVitals
    {
        public string ProbDiag { get; set; }
        public string Systolic { get; set; }
        public string Diastolic { get; set; }
        public string Pulse { get; set; }
        public string Weight { get; set; }
        public string RespiratoryRate { get; set; }
        public string SpO2 { get; set; }
        public string Glucose { get; set; }
        public string OtherAdvice { get; set; }
        public string PhyExam { get; set; }
        public string Temprature { get; set; }
    }

    public class MedicalHistoryViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PersonalHistory { get; set; }
        public string FamilyHistory { get; set; }
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
    public class EprescriptionViewModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string DocName { get; set; }

        public string ClinicName { get; set; }

        public string DocAddress { get; set; }

        public string DocPhone { get; set; }

        public string Prescription { get; set; }
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
}
