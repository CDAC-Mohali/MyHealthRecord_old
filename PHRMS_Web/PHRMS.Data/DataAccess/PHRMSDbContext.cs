using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using PHRMS.ViewModels;

namespace PHRMS.Data.DataAccess
{
    public class ApplicationUser : IdentityUser
    {

    }
    public class PHRMSDbContext : IdentityDbContext<ApplicationUser>
    {
        #region Profile
        public DbSet<Users> Users { get; set; }
        public DbSet<PersonalInformation> PersonalInformation { get; set; }
        public DbSet<DoctorUserMapping> DoctorUserMapping { get; set; }

        public DbSet<VitalSign> VitalSign { get; set; }
        public DbSet<DocPatientDetails> DocPatientDetails { get; set; }
        public DbSet<Places_of_Practice> Places_of_Practice { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }
        public DbSet<Appointment_Fees> Appointment_Fees { get; set; }
        public DbSet<Advice> Advice { get; set; }



        public DbSet<EmergencyInformation> EmergencyInformation { get; set; }
        public DbSet<EmployerInformation> EmployerInformation { get; set; }
        public DbSet<InsuranceInformation> InsuranceInformation { get; set; }
        public DbSet<LegalInformation> LegalInformation { get; set; }
        public DbSet<Preferences> Preferences { get; set; }
        public DbSet<LoginLog> LoginLog { get; set; }
        public DbSet<BloodGroups> BloodGroups { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Relationship> Relationship { get; set; }
        //Db set for VisionRx is stil required here
        public DbSet<DisabilityTypes> DisabilityTypes { get; set; }
        #endregion

        #region Immunizations
        public DbSet<ImmunizationsMasters> Immunizationsmasters { get; set; }
        public DbSet<Immunizations> Immunizations { get; set; }
        #endregion

        #region Allergies
        public DbSet<AllergyMaster> AllergyMaster { get; set; }
        public DbSet<Allergies> Allergies { get; set; }
        public DbSet<AllergySeverity> AllergySeverity { get; set; }
        public DbSet<AllergyDuration> AllergyDuration { get; set; }
        #endregion

        #region Procedures
        public DbSet<ProcedureMaster> ProcedureMaster { get; set; }
        public DbSet<Procedures> Procedures { get; set; }
        #endregion

        #region Medication
        public DbSet<MedicationMaster> MedicationMaster { get; set; }
        public DbSet<MedicineRoute> MedicineRoute { get; set; }
        public DbSet<DosageValue> DosageValue { get; set; }
        public DbSet<DosageUnit> DosageUnit { get; set; }
        public DbSet<FrequencyTaken> FrequencyTaken { get; set; }
        public DbSet<Medication> Medication { get; set; }
        #endregion

        #region Lab
        public DbSet<LabTestMaster> LabTestMaster { get; set; }
        public DbSet<LabTest> LabTest { get; set; }
        #endregion

        #region Wellness
        public DbSet<ActivityMaster> ActivityMaster { get; set; }
        public DbSet<Activities> Activities { get; set; }
        public DbSet<BloodGlucose> BloodGlucose { get; set; }
        public DbSet<BloodPressureAndPulse> BloodPressureAndPulse { get; set; }
        public DbSet<Sleep> Sleep { get; set; }
        public DbSet<Temperature> Temperature { get; set; }
        public DbSet<Weight> Weight { get; set; }
        #endregion

        #region HealthCondition
        public DbSet<HealthConditionMaster> HealthConditionMaster { get; set; }
        public DbSet<HealthCondition> HealthCondition { get; set; }
        #endregion

        #region Eprescription
        public DbSet<Eprescription> Eprescription { get; set; }
        #endregion

        #region Common
        public DbSet<PinCodes> PinCodes { get; set; }
        public DbSet<FilePath> FilePath { get; set; }
        public DbSet<HealthTip> HealthTip { get; set; }
        #endregion

        #region DashboardAnalytics
        //  public DbSet<DashboardAnalytics> DashboardAnalytics { get; set; }
        #endregion  

        #region Side Panel
        public DbSet<UserActivities> UserActivity { get; set; }
        #endregion

        #region MedicalContacts
        public DbSet<ContactTypes> ContactTypes { get; set; }
        public DbSet<MedicalContactRecords> MedicalContactRecords { get; set; }
        #endregion

        #region OpenEMR Tables
        public DbSet<StatesOpenEMR> StatesOpenEMR { get; set; }
        public DbSet<Districts> Districts { get; set; }
        public DbSet<SubDistricts> SubDistricts { get; set; }
        public DbSet<Hospital_OpenEMR> Hospital_OpenEMR { get; set; }
        public DbSet<OpenEMRT> OpenEMRT { get; set; }
        #endregion

        public DbSet<TempReg> TempReg { get; set; }

        public DbSet<PullSMS> PullSMS { get; set; }
        public DbSet<UserShareRecord> UserShareRecord { get; set; }
        public DbSet<ShareReportFeedBack> ShareReportFeedBack { get; set; }
        public DbSet<ShareReportNotification> ShareReportNotification { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = "";//apply your connectionstring.

  options.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
