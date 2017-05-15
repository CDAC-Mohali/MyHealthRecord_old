using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PHRMSAdmin.Startup))]
namespace PHRMSAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
