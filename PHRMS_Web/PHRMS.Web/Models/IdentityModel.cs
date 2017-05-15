using Microsoft.Data.Entity;
using PHRMS.Data.DataAccess;

namespace PHRMS.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    //public class ApplicationUser : IdentityUser
    //{

    //}
    public class ApplicationDbContext : PHRMSDbContext
    {
        private static bool _created = false;

        public ApplicationDbContext()
        {
            // Create the database and schema if it doesn't exist
            // This is a temporary workaround to create database until Entity Framework database migrations 
            // are supported in ASP.NET 5
            if (!_created)
            {
                Database.Migrate();
                _created = true;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Startup.Configuration["Data:DefaultConnection:ConnectionString"]);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
