using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PHRMSEMR
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
             routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // Route Pattern
                new { controller = "Account", action = "Login", id = UrlParameter.Optional } // Default values for above defined parameters
            );
        }

     

       

    }
}
