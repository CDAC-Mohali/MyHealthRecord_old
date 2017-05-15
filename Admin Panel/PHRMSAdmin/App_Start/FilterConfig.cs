using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PHRMSAdmin
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
    public class AdminAuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserData"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary{{ "controller", "Account" },
                                      { "action", "Login" },
                             { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                                     });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
