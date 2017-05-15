using PHRMSAdmin.Library;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PHRMSAdmin.DALayer
{
    public class PHRMSDBContext : DbContext
    {

        public PHRMSDBContext()
            : base("connectionString")
        {
            //   Database.Log = log => Common.QSLLogger.SaveDBLog(log);
            Database.SetInitializer<PHRMSDBContext>(null);
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
        public DbSet<Role> Role { get; set; }
        public DbSet<AdminUsers> AdminUsers { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<RoleTaskMapping> RoleTaskMapping { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<EMRFiles> EMRFiles { get; set; }

        public DbSet<Places_of_Practice> Places_of_Practice { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<AllergyMaster> AllergyMaster { get; set; }
        public DbSet<Allergies> Allergies { get; set; }
        public DbSet<HealthCondition> HealthCondition { get; set; }
        public DbSet<HealthConditionMaster> HealthConditionMaster { get; set; }
        public DbSet<PersonalInformation> PersonalInformation { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<DoctorApprovalTrack> DoctorApprovalTrack { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<MedicalColleges> MedicalColleges { get; set; }
        public DbSet<DocPatientDetails> DocPatientDetails { get; set; }


    }
}