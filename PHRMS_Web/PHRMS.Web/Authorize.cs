using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PHRMS.Web
{
    //Modify your filter to be like this to get the logger factory DI injectable.
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        
        public override void OnException(ExceptionContext context)
        {
            //...
        }
    }
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {


          //  var ss = filterContext.HttpContext.Session.GetString("UserId");
            if (filterContext.HttpContext.Session.GetString("UserId") == null)
            {
                //if (filterContext.HttpContext.Request.IsAjaxRequest())
                //{
                //    // the controller action was invoked with an AJAX request
                //}
                //   FormsAuthentication.SignOut();
                filterContext.Result =
               new RedirectToRouteResult(new RouteValueDictionary
                 {
             { "action", "Login" },
            { "controller", "Home" }
                  });

                return;
            }
        }

    }

    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter(ILoggerFactory logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this._logger = logger.CreateLogger("Global Exception Filter");
        }

        public void OnException(ExceptionContext context)
        {
            //var response = new ErrorResponse()
            //{
            //    Message = context.Exception.Message,
            //    StackTrace = context.Exception.StackTrace
            //};

            //context.Result = new ObjectResult(response)
            //{
            //    StatusCode = 500,
            //    DeclaredType = typeof(ErrorResponse)
            //};
            //if (context.HttpContext.Request())
            //{
            //    //filterContext.Result = AjaxError(filterContext.Exception.Message, filterContext);
            //    context.Result = AjaxError("Sorry, an error occurred while processing your request. Please try again after sometime.", context);


            //}
            //else
            //{

            // //   filterContext.Controller.TempData["ErrormessageShow"] = "Sorry, an error occurred while processing your request. Please try again after sometime.";//filterContext.Exception.Message;
            //    context.Result = new RedirectResult("~/Home/Error");
            //}
            context.Result = new RedirectResult("~/Home/Error");

            this._logger.LogError("GlobalExceptionFilter", context.Exception);
        }
    }
    //public class myAuthorizeAttribute : AuthorizeAttribute
    //{

    //    //Custom named parameters for annotation
    //    public string ResourceKey { get; set; }
    //    public string OperationKey { get; set; }

    //    //Called when access is denied
    //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    //    {
    //        //User isn't logged in
    //        if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
    //        {
    //            filterContext.Result = new RedirectToRouteResult(
    //                    new RouteValueDictionary(new { controller = "Account", action = "Login" })
    //            );
    //        }
    //        //User is logged in but has no access
    //        else
    //        {
    //            filterContext.Result = new RedirectToRouteResult(
    //                    new RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" })
    //            );
    //        }
    //    }

    //    //Core authentication, called before each action
    //    //protected override bool AuthorizeCore(HttpContextBase httpContext)
    //    //{
    //    //    var b = myMembership.Instance.Member().IsLoggedIn;
    //    //    //Is user logged in?
    //    //    if (b)
    //    //        //If user is logged in and we need a custom check:
    //    //        if (ResourceKey != null && OperationKey != null)
    //    //            return ecMembership.Instance.Member().ActivePermissions.Where(x => x.operation == OperationKey && x.resource == ResourceKey).Count() > 0;
    //    //    //Returns true or false, meaning allow or deny. False will call HandleUnauthorizedRequest above
    //    //    return b;
    //    //}
    //}
}