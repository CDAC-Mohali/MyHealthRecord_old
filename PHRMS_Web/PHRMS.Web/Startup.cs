using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using PHRMS.Data.DataAccess;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Serilog;
using Serilog.Sinks.AzureDocumentDb;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Data.Entity.Infrastructure;

namespace PHRMS.Web
{

    public class WebAPIDataContext : DbContext
    {
        public WebAPIDataContext(DbContextOptions<WebAPIDataContext> options)
            : base(options)
        {
        }
        // public DbSet<Book> Books { get; set; }
    //    public DbSet<testtable> testtable { get; set; }
    }
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env, IApplicationEnvironment appEnv)
        {

            // Setup configuration sources.
            var configurationBuilder = new ConfigurationBuilder().SetBasePath(appEnv.ApplicationBasePath).AddJsonFile("config.json").AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .MinimumLevel.Debug()
            .WriteTo.RollingFile(System.IO.Path.Combine(
                env.WebRootPath, "log-{Date}.txt"),
 outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level}:{EventId} [{SourceContext}] {Message}{NewLine}{Exception}")
            .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSqlServerCache(o =>
            {
                o.ConnectionString = "";
                o.SchemaName = "dbo";
                o.TableName = "SessionData";

            });
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                    .AllowAnyMethod()
                                                                     .AllowAnyHeader());
            });

          
            var defaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            services.AddEntityFramework().AddSqlServer().AddDbContext<PHRMSDbContext>(options =>
            {
                options.UseSqlServer(Configuration["Data:ConnectionString"]);

            });

            services.AddLogging();

            // add ASP.NET Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<PHRMSDbContext>();


            //   services.AddCaching();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromHours(1); /*TimeSpan.FromSeconds(10);*/

            });

            //Resolve dependency injection
            services.AddScoped<IPHRMSRepo, PHRMSRepo>();
            services.AddScoped<PHRMS.BLL.CatalogService>();
            services.AddScoped<PHRMSDbContext>();
            services.AddScoped<BLL.ClsSendSMS>();
            services.AddMvc(setup =>
            {
                //setup.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {

            //   loggerFactory.EnableSystemDiagnosticsTracing();
            //loggerFactory.AddConsole(minLevel: LogLevel.Debug);
            app.UseSession();

            //  loggerFactory.MinimumLevel = LogLevel.Debug;
            //loggerFactory.AddConsole();
            //     loggerFactory.AddSerilog();
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
            app.UseIISPlatformHandler();
            app.UseStaticFiles();


            // Add the following to the request pipeline only in development environment.
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.OrdinalIgnoreCase))
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {

                // Add Error handling middleware which catches all application specific errors and
                // send the request to the following path or controller action.
                app.UseExceptionHandler("/Home/Error");
            }

            //  app.UseMiddleware<Middleware.ErrorLoggingMiddleware>();
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{

            //    LoginPath = new PathString(@"/Home/Index"),
            //    AutomaticAuthenticate = true,
            //    LogoutPath = new PathString(@"/Home/Index"),
            //    AccessDeniedPath = new PathString("/Home/Error"),
            //    AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme,
            //    ExpireTimeSpan = TimeSpan.FromHours(24)
            //});
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            app.UseCors(builder =>
     builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());
        }

        //private static LoggerConfiguration GetLoggerConfiguration(string strWebRoot)
        //{
        //    return new LoggerConfiguration()
        //    .Enrich.WithMachineName()
        //    .Enrich.WithProcessId()
        //    .Enrich.WithThreadId()
        //    .MinimumLevel.Debug()
        //    .WriteTo.RollingFile(System.IO.Path.Combine(
        //       strWebRoot, "log-{Date}.txt"),
        //    outputTemplate:
        //    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level}:{EventId} [{SourceContext}] {Message}{NewLine}{Exception}");
        //}
    }
}