using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PHRMSEMR.Startup))]
namespace PHRMSEMR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
