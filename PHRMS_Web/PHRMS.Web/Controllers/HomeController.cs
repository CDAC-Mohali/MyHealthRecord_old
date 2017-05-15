using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using System.Security.Claims;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.AspNet.Mvc.Rendering;
using System.IO;
using Microsoft.AspNet.Mvc.ViewFeatures;
using System;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http.Features;
using System.Net.Http;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [AllowAnonymous]

    public class HomeController : Controller
    {
        private CatalogService _catalogService;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public HomeController(CatalogService catalogService, IHostingEnvironment appHostingEnvironment)
        {
            _catalogService = catalogService;
            _appHostingEnvironment = appHostingEnvironment;
            CatalogService.WebRootPath = appHostingEnvironment.WebRootPath;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return View();
        }

        public IActionResult Index1()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return View();
        }

        public IActionResult Login()
        {

            try
            {
                ViewBag.Browser = false;
                if (Request.Headers["User-Agent"].ToString().Contains("Edge") || Request.Headers["User-Agent"].ToString().Contains("Firefox"))
                {
                    ViewBag.Browser = true;
                }
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return View();
        }

        [HttpGet]
        public int GetUserCount()
        {
            try
            {
                return _catalogService.GetUserCount();
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
                return 0;
            }
        }
        private async Task<bool> RecaptchaIsValid(string captchaResponse)
        {

            try
            {
                //WriteToFile("Before connectionFeature");
                var connectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
                var requestUrl =
                    String.Format(
                        "enter captcha url",
                        "",
                        captchaResponse,
                        connectionFeature);
                string result;
                using (var client = new HttpClient())
                {
                    result = await client.GetStringAsync(requestUrl);
                }
                if (!String.IsNullOrWhiteSpace(result))
                {
                    var obj = JsonConvert.DeserializeObject<RecaptchaResponse>(result);
                    if (obj.Success)
                    {
                        //   _logger.LogInformation("You out of RecaptchaIsValid: Status is true");
                        return true;
                    }
                }
                //   _logger.LogInformation("You out of RecaptchaIsValid: Status is false");
                return false;
            }
            catch (Exception ex)
            {
                //     _logger.LogInformation("You got Exception at RecaptchaIsValid: " + ex.Message);
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

                throw;
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(ViewModels.LoginModel model)
        {
            ViewBag.Browser = false;
            if (Request.Headers["User-Agent"].ToString().Contains("Edge") || Request.Headers["User-Agent"].ToString().Contains("Firefox"))
            {
                ViewBag.Browser = true;
            }
            ViewBag.LoginFailure = 0;
            int loginfailure = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    string strMessage = "";

                    if (HttpContext.Session.GetString("loginfailure") != null)
                    {
                        loginfailure = int.Parse(HttpContext.Session.GetString("loginfailure"));
                    }
                    if (loginfailure < 3 || await RecaptchaIsValid(Request.Form["g-recaptcha-response"]))
                    {
                        ViewModels.UsersViewModel oUserModel = _catalogService.ValidateUser(model);
                        if (oUserModel != null)
                        {
                            HttpContext.Session.SetString("loginfailure", 0.ToString());
                            HttpContext.Session.SetString("UserId", oUserModel.UserId.ToString());
                            HttpContext.Session.SetString("FirstName", oUserModel.FirstName);
                            HttpContext.Session.SetString("FullName", oUserModel.FirstName + " " + oUserModel.LastName);
                            HttpContext.Session.SetString("UserImgPath", (oUserModel.ImgPath == null) ? "" : oUserModel.ImgPath);
                            HttpContext.Session.SetString("MobileNo", oUserModel.MobileNo);
                            HttpContext.Session.SetString("Count", "");
                            //        var claims = new List<Claim>
                            //{
                            //    new Claim(ClaimTypes.Name, model.UserName)
                            //    //NameIdentifier + IdentityProvider needed for anti-forgery
                            //    //or set AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
                            //    //new Claim(ClaimTypes.NameIdentifier, model.UserName)
                            //};

                            //var identity = new ClaimsIdentity(claims, "local", "name", "role");

                            //HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                            oUserModel = null;
                            return RedirectToAction("Dashboard", "Account");

                        }
                    }
                    else
                    {
                        strMessage = "Please answer the recaptcha challenge.";
                        ViewBag.LoginFailure = loginfailure;
                        ModelState.AddModelError("CustomValidation", strMessage);
                        return View();
                    }
                }
                #region check Session
                loginfailure = 0;
                if (HttpContext.Session.GetString("loginfailure") != null)
                {
                    loginfailure = int.Parse(HttpContext.Session.GetString("loginfailure"));
                }
                loginfailure = loginfailure + 1;
                HttpContext.Session.SetString("loginfailure", loginfailure.ToString());
                ViewBag.LoginFailure = loginfailure;
                #endregion
                ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                return View();

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
                return View();
            }
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ViewModels.LoginModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    ViewModels.UsersViewModel oUserModel = _catalogService.ValidateUser(model);
                    if (oUserModel != null)
                    {
                        HttpContext.Session.SetString("UserId", oUserModel.UserId.ToString());
                        HttpContext.Session.SetString("FirstName", oUserModel.FirstName);
                        HttpContext.Session.SetString("FullName", oUserModel.FirstName + " " + oUserModel.LastName);
                        HttpContext.Session.SetString("UserImgPath", oUserModel.ImgPath);
                        HttpContext.Session.SetString("MobileNo", oUserModel.MobileNo);
                        HttpContext.Session.SetString("Count", "");
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.UserName)
                        //NameIdentifier + IdentityProvider needed for anti-forgery
                        //or set AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
                        //new Claim(ClaimTypes.NameIdentifier, model.UserName)
                    };

                        var identity = new ClaimsIdentity(claims, "local", "name", "role");
                        HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                        oUserModel = null;
                        return RedirectToAction("Dashboard", "Account");
                    }
                }
                ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                return View();
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
                return View();
            }
        }

        public IActionResult LogOff()
        {
            try
            {
                HttpContext.Authentication.SignOutAsync("Cookies");
                HttpContext.Session.Clear();

                //    HttpContext.Response.Cookies.Delete("ASP.NET_SessionId");
                HttpContext.Authentication.SignOutAsync("ASP.NET_SessionId");
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return View("~/Views/Shared/Error.cshtml");
        }

        #region ForgotPassword

        public IActionResult ForgotPassword()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return View();
        }

        public async Task<IActionResult> VerifyUserDetails(ViewModels.ForgotPasswordModel model)
        {
            try
            {
                // model.OTPMobileNo = model.UserName;
                if (model.UserName != "" && model.UserName != null)
                {
                    model = _catalogService.VerifyUsernameforPasswordChange(model);
                    if (model != null && model.StatusCode == ViewModels.FPProcessStatus.Success)
                    {
                        TempData["UserId"] = model.Id;
                        //System.Threading.Thread email2 = new System.Threading.Thread(delegate ()
                        //{
                        await SendVerificationCode('F', model.MobileNo, model.Email);
                        //});
                        //email2.IsBackground = true;
                        //email2.Start();
                    }
                }
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return Json(new { status = model.StatusCode, view = RenderPartialViewToString("_SecretCodeVerification", null) });
        }

        public IActionResult VerifySecretCode(string OTP)
        {
            ViewBag.OTP = OTP;
            int pstatus = ViewModels.FPProcessStatus.SysFailure;
            try
            {

                if (!string.IsNullOrEmpty(OTP))
                {
                    if (TempData["SmsCode"] != null && OTP.Equals(TempData["SmsCode"].ToString()))
                    {
                        return Json(new { status = ViewModels.FPProcessStatus.Success, view = RenderPartialViewToString("_FPAfterVerification", null) });
                    }
                    else
                    {
                        TempData.Keep("SmsCode");
                        pstatus = ViewModels.FPProcessStatus.VerificationFailure;
                    }
                }
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");

            }
            return Json(new { status = pstatus, view = RenderPartialViewToString("_SecretCodeVerification", null) });
        }

        public async Task<IActionResult> ResetPassword(string Password)
        {
            try
            {
                bool result = await _catalogService.ResetPassword(Guid.Parse(TempData["UserId"].ToString()), Password);
                return Json(new { status = (result ? ViewModels.FPProcessStatus.Success : ViewModels.FPProcessStatus.SysFailure), view = RenderPartialViewToString("_FPAfterVerification", null) });
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
                return null;
            }

        }

        #endregion
        private async Task<bool> SendOTPVerificationCode(char flag, string strMobileNo, string strEmailId, string rno)
        {
            bool result = false;
            string sms_content = "";
            try
            {


                sms_content = "The verification code for Login at MyHealthRecord is " + rno + ".";
                //    _catalogService.BeforeSMSsend();
                result = CatalogService.sendInfiniSMS(strMobileNo, sms_content);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
            }
            return result;
        }

        private async Task<bool> SendVerificationCode(char flag, string strMobileNo, string strEmailId)
        {
            bool result = false;
            string sms_content = "";
            try
            {
                System.Random randomclass = new System.Random();
                long rno = randomclass.Next(100000, 999999);
                //ViewBag.SmsCode = rno.ToString();
                TempData["SmsCode"] = rno.ToString();
                //WriteToFile(rno.ToString() + "   " + DateTime.Now.ToString());
                //ViewState["smscode"] = rno.ToString();
                //ViewState["password"] = txtpass.Text;
                if (flag == 'S') //send code via SMS
                {

                    sms_content = "The verification code for registration at MyHealthRecord is " + rno + ".";
                    //   _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS(strMobileNo, sms_content);

                }
                else if (flag == 'R') //resend code via SMS
                {
                    sms_content = "The verification code for registration at MyHealthRecord is " + rno + ".";
                   // _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS(strMobileNo, sms_content);
                }
                else if (flag == 'W') //resend code via SMS
                {
                    sms_content = "Thank you for registering at MyHealthRecord. You can use your Email address or Mobile No. for login to the System.";
                  //  _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS( strMobileNo, sms_content);
                }
                else if (flag == 'F')
                {
                    string bodysignup1 = string.Empty;
                    using (StreamReader reader = new StreamReader(_appHostingEnvironment.WebRootPath + "\\Templates\\OTPChgPwd.html"))
                    {
                        bodysignup1 = reader.ReadToEnd();
                    }
                    bodysignup1 = bodysignup1.Replace("{Title}", "Hi there!");
                    bodysignup1 = bodysignup1.Replace("{rno}", Convert.ToString(rno));
                    bodysignup1 = bodysignup1.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");




                    sms_content = "The verification code for changing the password of your account is " + " " + rno;
                //    _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS(strMobileNo, sms_content);
                    sms_content = "The verification code for changing the password of your account is";
                    bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                    EMailer.SendEmail("MyHealthRecord - Verification Code", bodysignup1, strEmailId, true);
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
            }
            return result;
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {

                if (string.IsNullOrEmpty(viewName))
                    viewName = ActionContext.ActionDescriptor.Name;

                ViewData.Model = model;

                using (StringWriter sw = new StringWriter())
                {
                    var engine = Resolver.GetService(typeof(ICompositeViewEngine))
                        as ICompositeViewEngine;
                    ViewEngineResult viewResult = engine.FindPartialView(ActionContext, viewName);

                    ViewContext viewContext = new ViewContext(
                        ActionContext,
                        viewResult.View,
                        ViewData,
                        TempData,
                        sw,
                        new HtmlHelperOptions() //Added this parameter in
                    );

                    //Everything is async now!
                    var t = viewResult.View.RenderAsync(viewContext);
                    t.Wait();

                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
                return "";
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginOTP()
        {
            try
            {
                ViewBag.Status = 0;
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
            }
            return View();
        }
        //



        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginOTP(ViewModels.ForgotPasswordModel model)
        {

            try
            {
                model.UserName = model.OTPMobileNo;


                model = _catalogService.VerifyUsernameforPasswordChange(model);
                if (model != null && model.StatusCode == ViewModels.FPProcessStatus.Success)
                {
                    TempData["UserId"] = model.Id;
                    //System.Threading.Thread email2 = new System.Threading.Thread(delegate ()
                    //{
                    System.Random randomclass = new System.Random();
                    string rno = randomclass.Next(100000, 999999).ToString();
                    //ViewBag.SmsCode = rno.ToString();
                    TempData["SmssCode"] = rno;
                    await SendOTPVerificationCode('F', model.MobileNo, model.Email, rno);

                    //});
                    //email2.IsBackground = true;
                    //email2.Start();
                }
            }


            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
            }
            return Json(new { status = model.StatusCode, view = RenderPartialViewToString("_LoginOTPVerification", null) });

        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult VerifyLoginOTP(string OTP)
        {
            string ret = "0";
            try
            {
                ViewBag.OTP = OTP;
                int pstatus = ViewModels.FPProcessStatus.SysFailure;
                if (!string.IsNullOrEmpty(OTP))
                {

                    if (TempData["SmssCode"] != null && OTP.Equals(TempData["SmssCode"].ToString()))
                    {
                        Guid DocId = ((Guid)TempData["UserId"]);

                        ViewModels.UsersViewModel oUserModel = _catalogService.LoginByOTP(DocId); //db.Doctor.Where(x => (x.email.Equals(model.UserName) || x.phone_number.Equals(model.UserName)) && x.password == model.Password).FirstOrDefault();

                        if (oUserModel != null)
                        {
                            HttpContext.Session.SetString("UserId", oUserModel.UserId.ToString());
                            HttpContext.Session.SetString("FirstName", oUserModel.FirstName);
                            HttpContext.Session.SetString("FullName", oUserModel.FirstName + " " + oUserModel.LastName);
                            HttpContext.Session.SetString("UserImgPath", (oUserModel.ImgPath == null) ? "" : oUserModel.ImgPath);
                            HttpContext.Session.SetString("MobileNo", oUserModel.MobileNo);
                            HttpContext.Session.SetString("Count", "");
                            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, oUserModel.MobileNo)
                        //NameIdentifier + IdentityProvider needed for anti-forgery
                        //or set AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
                        //new Claim(ClaimTypes.NameIdentifier, model.UserName)
                    };

                            var identity = new ClaimsIdentity(claims, "local", "name", "role");
                            HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                            oUserModel = null;

                        }
                        return Json(new { status = ViewModels.FPProcessStatus.Success, view = RenderPartialViewToString("_LoginOTPVerification", null) });
                    }

                }

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Home");
            }
            TempData.Keep("SmssCode");
            return Json(new { status = ViewModels.FPProcessStatus.VerificationFailure, view = RenderPartialViewToString("_LoginOTPVerification", null) });


        }
    }
}
