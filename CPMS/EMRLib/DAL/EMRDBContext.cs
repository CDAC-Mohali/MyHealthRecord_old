using EMRLib.DataModels;
using EMRViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMRLib.DAL
{
    public class EMRDBContext : DbContext
    {

        public EMRDBContext()
            : base("EMRConnection")
        {
            //   Database.Log = log => Common.QSLLogger.SaveDBLog(log);
            Database.SetInitializer<EMRDBContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //DONT DO THIS ANYMORE
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Vote>().ToTable("Votes")
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    // Database.SetInitializer<TBDB>(new QSLInitializer());
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        //    // modelBuilder.Entity<RoleTask>()  .MapToStoredProcedures();
        //}

        //public ChetnasDBContext()
        //    : base("chetnbpb_visions8Entities")
        //{
        //    // Database.SetInitializer<ChetnasDBContext>(new CreateDatabaseIfNotExists<ChetnasDBContext>());
        //    //  Database.CreateIfNotExists();
        //    // Database.SetInitializer<ChetnasDBContext>(new DropCreateDatabaseIfModelChanges<ChetnasDBContext>());
        //    //Database.SetInitializer<SchoolDBContext>(new DropCreateDatabaseAlways<SchoolDBContext>());
        //    //Database.SetInitializer<SchoolDBContext>(new SchoolDBInitializer());
        //}
        public DbSet<Users> Users { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<DocPatientDetails> DocPatientDetails { get; set; }
        public DbSet<Places_of_Practice> Places_of_Practice { get; set; }
        public DbSet<EMRFiles> EMRFiles { get; set; }
        public DbSet<Appointment_Fees> Appointment_Fees { get; set; }
        public DbSet<PersonalInformation> PersonalInformation { get; set; }
        public DbSet<PatientDetailOTP> PatientDetailOTP { get; set; }
        public DbSet<EmergencyInformation> EmergencyInformation { get; set; }
        public DbSet<EmployerInformation> EmployerInformation { get; set; }
        public DbSet<InsuranceInformation> InsuranceInformation { get; set; }
        public DbSet<LegalInformation> LegalInformation { get; set; }
        public DbSet<Preferences> Preferences { get; set; }
        public DbSet<Allergies> Allergies { get; set; }
        public DbSet<DataModels.Advice> Advice { get; set; }

        public DbSet<AllergyMaster> AllergyMaster { get; set; }
        public DbSet<AllergySeverity> AllergySeverity { get; set; }
        public DbSet<AllergyDuration> AllergyDuration { get; set; }

        public DbSet<HealthConditionMaster> HealthConditionMaster { get; set; }
        public DbSet<HealthCondition> HealthCondition { get; set; }
        public DbSet<Medication> Medication { get; set; }
        public DbSet<MedicineRoute> MedicineRoute { get; set; }
        public DbSet<DosageValue> DosageValue { get; set; }
        public DbSet<DosageUnit> DosageUnit { get; set; }
        public DbSet<FrequencyTaken> FrequencyTaken { get; set; }

        public DbSet<MedicationMaster> MedicationMaster { get; set; }
        public DbSet<Immunizations> Immunizations { get; set; }
        public DbSet<ImmunizationsMasters> Immunizationsmasters { get; set; }
        public DbSet<LabTestMaster> LabTestMaster { get; set; }
        public DbSet<LabTest> LabTest { get; set; }
        public DbSet<Procedures> Procedures { get; set; }
        public DbSet<ProcedureMaster> ProcedureMaster { get; set; }
        public DbSet<Activities> Activities { get; set; }
        public DbSet<ActivityMaster> ActivityMaster { get; set; }
        public DbSet<BloodPressureAndPulse> BloodPressureAndPulse { get; set; }
        public DbSet<Weight> Weight { get; set; }
        public DbSet<BloodGlucose> BloodGlucose { get; set; }
        public DbSet<FilePath> FilePath { get; set; }
        public DbSet<BloodGroups> BloodGroups { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<Relationship> Relationship { get; set; }
        //Db set for VisionRx is stil required here
        public DbSet<DisabilityTypes> DisabilityTypes { get; set; }
        public DbSet<VitalSign> VitalSign { get; set; }
        public DbSet<VitalSignMaster> VitalSignMaster { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }
        public DbSet<Eprescription> Eprescription { get; set; }
        public DbSet<DoctorUserMapping> DoctorUserMapping { get; set; }
        public DbSet<EMRDocFilePath> EMRDocFilePath { get; set; }
        public DbSet<ContactTypes> ContactTypes { get; set; }
        public DbSet<MedicalContactRecords> MedicalContactRecords { get; set; }

        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<MedicalColleges> MedicalColleges { get; set; }

        public DbSet<HealthTip> HealthTip { get; set; }

    }
}
