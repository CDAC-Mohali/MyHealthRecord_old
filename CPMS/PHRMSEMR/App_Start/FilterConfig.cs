using PHRMSEMR.Controllers;
using PHRMSEMR.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PHRMSEMR
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public class AuthorizationFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (HttpContext.Current.Session["DocSessionData"] == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary{{ "controller", "Account" },
                                      { "Account", "LogOff" }

                                         });
                }
                base.OnActionExecuting(filterContext);
            }
        }


        public class Error : System.Web.Mvc.HandleErrorAttribute
        {
            protected JsonResult AjaxError(string message, ExceptionContext filterContext)
            {
                //If message is null or empty, then fill with generic message
                if (String.IsNullOrEmpty(message))
                    message = "Something went wrong while processing your request. Please refresh the page and try again.";
                filterContext.Controller.TempData["ErrormessageShow"] = message;
                //Set the response status code to 500
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                //Needed for IIS7.0
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                return new JsonResult
                {

                    Data = new { ErrorMessage = message },
                    //ContentEncoding = System.Text.Encoding.UTF8,
                    //JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };
            }
            public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
            {
                Common.CreateLog(Common.ExecptionMessage(filterContext.Exception), MessageType.Error, "Global");
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        //filterContext.Result = AjaxError(filterContext.Exception.Message, filterContext);
                        filterContext.Result = AjaxError("Sorry, an error occurred while processing your request. Please try again after sometime.", filterContext);
                        filterContext.ExceptionHandled = true;

                    }
                    else
                    {
                        filterContext.ExceptionHandled = true;
                        filterContext.Controller.TempData["ErrormessageShow"] = "Sorry, an error occurred while processing your request. Please try again after sometime.";//filterContext.Exception.Message;
                        filterContext.Result = new RedirectResult("~/Home/Error");
                    }
                }
                else
                {
                    base.OnException(filterContext);
                }
                //OVERRIDE THE 500 ERROR  
                //filterContext.HttpContext.Response.StatusCode = 200;
            }

            private static void RaiseErrorSignal(System.Exception e)
            {
                var context = HttpContext.Current;
                // using.Elmah.ErrorSignal.FromContext(context).Raise(e, context);
            }

        }
    }
}
