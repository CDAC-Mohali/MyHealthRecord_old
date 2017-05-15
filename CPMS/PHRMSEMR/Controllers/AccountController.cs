using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PHRMSEMR.Models;
using EMRLib.DAL;
using EMRLib.DataModels;
using AutoMapper;
using System.Web.Security;
using EMRViewModels;
using System.IO;
using System.Collections.Generic;
using static PHRMSEMR.FilterConfig;
using System.Web.SessionState;
using System.Text;

using System.Web.Security.AntiXss;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace PHRMSEMR.Controllers
{
    public class RecaptchaResponse
    {
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public ICollection<string> ErrorCodes { get; set; }
        public RecaptchaResponse()
        {
            ErrorCodes = new HashSet<string>();
        }
    }
    [Error]
    public class AccountController : Controller
    {


        public String p_headerImage { get; set; }
        public String mailaddress { get; set; }
        public String p_URL { get; set; }
        public String p_Name { get; set; }
        public String p_Email { get; set; }
        public String p_footerImage { get; set; }
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        CommonComponent _comcomponent = new CommonComponent();
        IEMRRepository _repo;

        public AccountController()
        {
            _repo = new EMRRepository();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [AllowAnonymous]
        public ActionResult Test(string returnUrl)
        {

            return View();
        }
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                ViewBag.Browser = false;
                if (Request.Headers["User-Agent"].ToString().Contains("Edge") || Request.Headers["User-Agent"].ToString().Contains("Firefox"))
                {
                    ViewBag.Browser = true;
                }
                ViewBag.ReturnUrl = returnUrl;

            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");

            }
            return View();
        }

        public string GetIPAddress()
        {
            string IPAddress = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;

        }

        private async Task<bool> RecaptchaIsValid(string captchaResponse)
        {

            try
            {
                //WriteToFile("Before connectionFeature");
                var connectionFeature = GetIPAddress();
                var requestUrl =
                    String.Format(
                        "capcha url",
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
                // PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

                throw;
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl, FormCollection formCollection)
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


                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                //string strpassword = Encryptdata(model.Password);
                //string newpassword = Decryptdata(strpassword);
                //  EMRDBContext db = new EMRDBContext();
                string strMessage = "";

                if (Session["loginfailure"] != null)
                {
                    loginfailure = int.Parse(Session["loginfailure"].ToString());
                }
                if (loginfailure < 3 || await RecaptchaIsValid(Request.Form["g-recaptcha-response"]))
                {
                    var docList = _repo.Login(model.UserName, model.Password); //db.Doctor.Where(x => (x.email.Equals(model.UserName) || x.phone_number.Equals(model.UserName)) && x.password == model.Password).FirstOrDefault();

                    if (docList != null)
                    {
                        Session["loginfailure"] = 0;
                        docList.password = null;
                        var filePathRecord = _repo.GetFilePath(docList.docid);
                        System.Web.Security.FormsAuthentication.SetAuthCookie(docList.name, true);
                        CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                        serializeModel.DocId = docList.docid;
                        serializeModel.Name = docList.name;
                        serializeModel.Email = docList.email;
                        serializeModel.DoctorName = docList.name;
                        serializeModel.DoctorPhone = docList.phone_number;
                        if (filePathRecord != null && filePathRecord != "")
                        {
                            serializeModel.ImgPath = filePathRecord;
                        }

                        Session["DocSessionData"] = serializeModel;
                        if (Url.IsLocalUrl(returnUrl))
                        {

                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Appointments");
                        }

                    }
                    else
                    {
                        #region check Session
                        loginfailure = 0;
                        if (Session["loginfailure"] != null)
                        {
                            loginfailure = int.Parse(Session["loginfailure"].ToString());
                        }
                        loginfailure = loginfailure + 1;
                        Session["loginfailure"] = loginfailure;
                        ViewBag.LoginFailure = loginfailure;
                        #endregion
                        ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                        return View();
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
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");


            }
            return View();
        }


        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
        /// <summary>
        /// Function is used to Decrypt the password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            try
            {

                // Require that the user has already logged in via username/password or external login
                if (!await SignInManager.HasBeenVerifiedAsync())
                {
                    return View("Error");
                }
                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View("Error");
        }
        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // The following code protects for brute force attacks against the two factor codes. 
                // If a user enters incorrect codes for a specified amount of time then the user account 
                // will be locked out for a specified amount of time. 
                // You can configure the account lockout settings in IdentityConfig
                var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                }
            }

            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View(model);
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult FAQS()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            try
            {
                ViewBag.SpecialityList = _comcomponent.GetSpecialityList();
                ViewBag.HospitalList = _comcomponent.GetHospitalList();
                ViewBag.Status = 0;
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(DoctorViewModel model, PlaceViewModel oModel)
        {
            try
            {
               
               // model.date_of_birth = new DateTime(model.date_of_birth.Year, model.date_of_birth.Month, model.date_of_birth.Day);
                if (ModelState.IsValid)
                {
                    Random randomclass = new Random();
                    model.OTP = randomclass.Next(100000, 999999);
                    Mapper.CreateMap<DoctorViewModel, Doctor>();
                    Doctor oDoctor = Mapper.Map<Doctor>(model);
                    Guid result = _repo.AddDoctor(oDoctor);
                    if (!result.Equals(Guid.Empty))
                    {
                        SendEmailSMS('S', model.phone_number, model.email, model.OTP, model.name);
                        Session["OTP"] = model.OTP;
                        Session["MobileNo"] = model.phone_number;
                        Session["Email"] = model.email;
                        Session["DocId"] = result;
                        Session["Name"] = model.name;
                        oModel.city = model.PlaceViewModel.city;
                        oModel.pincode = model.PlaceViewModel.pincode;
                        oModel.license_number = model.PlaceViewModel.license_number;

                        Mapper.CreateMap<PlaceViewModel, Places_of_Practice>();
                        Places_of_Practice Places_of_Practice = Mapper.Map<Places_of_Practice>(oModel);
                        Places_of_Practice.docid = result;
                        if (_repo.CompleteRegistration(Places_of_Practice, oModel.licence_copy))
                        {
                            ViewBag.Status = 1;
                            //   return View();
                        }
                        else
                        {
                            ViewBag.SpecialityList = _comcomponent.GetSpecialityList();
                            ViewBag.HospitalList = _comcomponent.GetHospitalList();
                            ViewBag.Status = -2;
                            //  return View("Register");
                        }
                        // ViewBag.Status = 1;
                        return View();
                    }
                }
                ViewBag.SpecialityList = _comcomponent.GetSpecialityList();
                ViewBag.HospitalList = _comcomponent.GetHospitalList();
                ViewBag.Status = 0;
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult VerifyOTP(string OTP)
        {
            try
            {
                if (OTP.Equals(Session["OTP"].ToString()))
                {
                    Doctor oDoctor = new Doctor();
                    oDoctor.docid = Guid.Parse(Session["DocId"].ToString());
                    oDoctor.IsApproved = 1;
                    if (_repo.UpdateDoctorStatus(oDoctor))
                    {

                        ViewBag.Status = 3;
                        SendEmailSMS('C', Session["MobileNo"].ToString(), Session["Email"].ToString(), 0, Session["Name"].ToString());
                        return View("Register");
                    }
                }
                else
                {
                    ViewBag.SpecialityList = _comcomponent.GetSpecialityList();
                    ViewBag.HospitalList = _comcomponent.GetHospitalList();
                    ViewBag.Status = -1;
                    return View("Register");
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            ViewBag.SpecialityList = _comcomponent.GetSpecialityList();
            ViewBag.HospitalList = _comcomponent.GetHospitalList();
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CompleteRegistration(PlaceViewModel oModel)
        {
            try
            {
                oModel.docid = Guid.Parse(Session["DocId"].ToString());
                Mapper.CreateMap<PlaceViewModel, Places_of_Practice>();
                Places_of_Practice model = Mapper.Map<Places_of_Practice>(oModel);
                if (_repo.CompleteRegistration(model, oModel.licence_copy))
                {
                    ViewBag.Status = 3;
                    SendEmailSMS('C', Session["MobileNo"].ToString(), Session["Email"].ToString(), 0, Session["Name"].ToString());
                    return View("Register");
                }
                else
                {
                    ViewBag.Status = -2;
                    return View("Register");
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UploadFile()
        {

            string str = "";
            int stat = 500;
            try
            {
                var body = HttpContext.Request.InputStream;
                string fileName = HttpContext.Request.Headers["X_FILENAME"].ToString().ToLower();
                string extension = System.IO.Path.GetExtension(fileName);
                //var extension = fileName.EndsWith("png") ? "png" : (fileName.EndsWith("jpg") || fileName.EndsWith("jpeg") ? "jpeg" : "gif");
                //using (FileStream fs = System.IO.File.Create(string.Format(@"D:\{0}.{1}", Guid.NewGuid(), extension)))
                //var path = Path.Combine(Microsoft.Net.Http.Server.MapPath("~/App_Data/Images"), fileName);
                str = string.Format(@"\Images\LicenseCopy\{0}{1}", Guid.NewGuid(), extension);
                string path = Server.MapPath("~/") + str;
                using (System.IO.FileStream fs = System.IO.File.Create(path))
                {
                    await body.CopyToAsync(fs);
                }
                str = str.Replace("\\", "/");
                stat = 200;
            }
            catch (Exception ex)
            {
                str = ex.Message;
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            var model = new
            {
                status = stat,
                path = stat == 200 ? System.IO.Path.GetFileName(str) : str
            };
            return Json(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoesMobileExist(string phone_number)
        {
            try
            {
                return Json(_repo.DoesMobileExist(phone_number));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return Json(_repo.DoesMobileExist(phone_number));
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoesEmailExist(string email)
        {
            try
            {
                return Json(_repo.DoesEmailExist(email));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return Json(_repo.DoesEmailExist(email));
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult ResendVerificationCode()
        {
            bool res1 = false;
            try
            {
                res1 = SendEmailSMS('S', Session["MobileNo"].ToString(), Session["Email"].ToString(), int.Parse(Session["OTP"].ToString()), Session["Name"].ToString());
                //return Json(res1);
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            //bool res = SendEmailSMS('S', Session["MobileNo"].ToString(), Session["Email"].ToString(), int.Parse(Session["OTP"].ToString()));
            return Json(res1);
        }

        private bool SendEmailSMS(char flag, string strMobileNo, string strEmailId, int rno, string FirstName)
        {
            bool result = false;
            bool bEmail = false;
            string sms_content = "";
            ClsSendSMS sms = new ClsSendSMS();
            try
            {
                //ViewBag.SmsCode = rno.ToString();
                //WriteToFile(rno.ToString() + "   " + DateTime.Now.ToString());
                //ViewState["smscode"] = rno.ToString();
                //ViewState["password"] = txtpass.Text;
                if (flag == 'S') //send code via SMS
                {
                    string bodysignup1 = string.Empty;
                    using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\SigUpStep_1.html")))
                    {
                        bodysignup1 = reader.ReadToEnd();
                    }

                    bodysignup1 = bodysignup1.Replace("{Title}", Convert.ToString(FirstName));
                    bodysignup1 = bodysignup1.Replace("{rno}", Convert.ToString(rno));
                    bodysignup1 = bodysignup1.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");

                    sms_content = "The verification code for registration at MyHealthRecord is " + rno + ".";
                    //   sms.BeforeSMSsend();
                    result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                    //sms_content = "In order to complete the registration process, we need to confirm your email address.<br/>Kindly use this code to verify your email address: " + rno;
                    sms_content = "Thank you for your interest in MyHealthRecord. Kindly use this code to verify your email address ";
                    bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Verification Code", bodysignup1, strEmailId, true);
                    result = result || bEmail;

                }
                else if (flag == 'C') //send code via SMS
                {
                    string bodysignup2 = string.Empty;
                    using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\SigUpStep_1.html")))
                    {
                        bodysignup2 = reader.ReadToEnd();
                    }
                    bodysignup2 = bodysignup2.Replace("{Title}", Convert.ToString(FirstName));
                    bodysignup2 = bodysignup2.Replace("{rno}", "");
                    bodysignup2 = bodysignup2.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");

                    sms_content = "Thanks for registering with us, Team MyHealthRecord";
                    //   sms.BeforeSMSsend();
                    result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                    sms_content = "You have been successfully registered with MyHealthRecord."
                       + "<br/>Please note that, the request, for account approval will be sent to our  Administrator, who will verify and approve your account.Once the account approved, you can avail our service.";

                    bodysignup2 = bodysignup2.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Successful Registration", bodysignup2, strEmailId, true);
                    result = result || bEmail;
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return result;
        }
        private bool PHRMSendEmailSMS(string strMobileNo, string strEmailId, string UserName, string Password)
        {
            bool result = false;
            bool bEmail = false;
            string sms_content = "";
            ClsSendSMS sms = new ClsSendSMS();
            try
            {
                string bodyPatReg = string.Empty;
                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\PatientReg.html")))
                {
                    bodyPatReg = reader.ReadToEnd();
                }
                bodyPatReg = bodyPatReg.Replace("{Title}", "Dear Sir/Madam,");
                bodyPatReg = bodyPatReg.Replace("{p_username}", Convert.ToString(UserName));
                bodyPatReg = bodyPatReg.Replace("{p_password}", Convert.ToString(Password));
                bodyPatReg = bodyPatReg.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");

                sms_content = "You have been successfully registered with MyHealthRecord. Your Username " + UserName + " and Password " + Password + ".";
                //  sms.BeforeSMSsend();
                result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                //sms_content = "You have been successfully registered with PHRMS. Your Username " + UserName + " and Password " + Password + ".";

                sms_content = "You have been successfully registered with MyHealthRecord. You can access your health record at http://practice.myhealthrecord.nhp.gov.in";
                bodyPatReg = bodyPatReg.Replace("{messagephrms}", sms_content);
                bEmail = EMailer.SendEmail("MyHealthRecord - Auto Registration", bodyPatReg, strEmailId, true);
                result = result || bEmail;
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return result;
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoesEmailOrMobileExist(string UserName)
        {
            try
            {
                return Json(_repo.DoesEmailOrMobileExist(UserName));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return Json(_repo.DoesEmailOrMobileExist(UserName));
        }



        private async Task<bool> SendVerificationCode(char flag, string strMobileNo, string strEmailId)
        {


            bool result = false;
            string sms_content = "";
            bool bEmail = false;
            ClsSendSMS sms = new ClsSendSMS();
            try
            {
                System.Random randomclass = new System.Random();
                long rno = randomclass.Next(100000, 999999);
                //ViewBag.SmsCode = rno.ToString();
                TempData["SmsCode"] = rno.ToString();
                //WriteToFile(rno.ToString() + "   " + DateTime.Now.ToString());
                //ViewState["smscode"] = rno.ToString();
                //ViewState["password"] = txtpass.Text;
                if (flag == 'F')
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\OTPChangePwd.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Title}", "Hi there!");
                    body = body.Replace("{rno}", Convert.ToString(rno));
                    body = body.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");

                    sms_content = "The verification code for changing password of your account is " + rno;
                    //     sms.BeforeSMSsend();
                    result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                    //sms_content = "The verification code for changing password of your account" + rno;
                    sms_content = "The verification code for changing the password of your account is";
                    body = body.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Verification Code", body, strEmailId, true);
                    result = result || bEmail;
                }
                if (flag == 'L')
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\OTPChangePwd.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Title}", "Hi there!");
                    body = body.Replace("{rno}", Convert.ToString(rno));
                    body = body.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");

                    sms_content = "The verification code for Account Login at MyHealthRecord is " + rno;
                    // sms.BeforeSMSsend();
                    result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                    sms_content = "The verification code for Account Login at MyHealthRecord is";
                    body = body.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Verification Code", body, strEmailId, true);
                    result = result || bEmail;
                }
                if (flag == 'S')
                {
                    string bodysucc = string.Empty;
                    using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\PwdChgSuccess.html")))
                    {
                        bodysucc = reader.ReadToEnd();
                    }

                    bodysucc = bodysucc.Replace("{Title}", "Hi there!");
                    bodysucc = bodysucc.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");
                    sms_content = "Your password has been changed successfully at MyHealthRecord.";
                    //    sms.BeforeSMSsend();
                    result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                    sms_content = "Your password has been changed successfully.";

                    bodysucc = bodysucc.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Password changed successfully", bodysucc, strEmailId, true);
                    result = result || bEmail;
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return result;
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        /// <summary>
        /// /////////////////////////
        /// LoginOTP
        /// </summary>
        /// <returns></returns>
        //


        //
        // GET: /Account/ForgotPassword
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
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }
        //



        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult LoginOTP(EMRViewModels.ForgotPasswordModel model)
        {
            string rtvalue = "0";
            try
            {
                if (ModelState.IsValid)
                {
                    model = _repo.VerifyUsernameforPasswordChange(model);
                    if (model.UserName != null && model.MobileNo != null)
                    {
                        TempData["UserId"] = model.Id;
                        SendVerificationCode('L', model.MobileNo, model.Email);
                        //return Json("1");
                        rtvalue = "1";
                    }
                    else
                    {
                        rtvalue = "0";
                        //return Json("0");
                    }
                }
                else
                {
                    rtvalue = "0";
                    //return Json("0");
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return Json(rtvalue);
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public string VerifyLoginOTP(string OTP)
        {
            string ret = "0";
            try
            {
                ViewBag.OTP = OTP;
                int pstatus = FPProcessStatus.SysFailure;
                if (!string.IsNullOrEmpty(OTP))
                {
                    if (TempData["SmsCode"] != null && OTP.Equals(TempData["SmsCode"].ToString()))
                    {
                        Guid DocId = ((Guid)TempData["UserId"]);
                        var docList = _repo.LoginByOTP(DocId); //db.Doctor.Where(x => (x.email.Equals(model.UserName) || x.phone_number.Equals(model.UserName)) && x.password == model.Password).FirstOrDefault();
                        //var docList = _repo.Login(model.UserName, model.Password); //db.Doctor.Where(x => (x.email.Equals(model.UserName) || x.phone_number.Equals(model.UserName)) && x.password == model.Password).FirstOrDefault();
                        if (docList != null)
                        {
                            var filePathRecord = _repo.GetFilePath(docList.docid);
                            System.Web.Security.FormsAuthentication.SetAuthCookie(docList.name, true);
                            CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                            serializeModel.DocId = docList.docid;
                            serializeModel.Name = docList.name;
                            serializeModel.Email = docList.email;
                            serializeModel.DoctorName = docList.name;
                            serializeModel.DoctorPhone = docList.phone_number;
                            if (filePathRecord != null && filePathRecord != "")
                            {
                                serializeModel.ImgPath = filePathRecord;
                            }

                            Session["DocSessionData"] = serializeModel;

                            ret = "1";

                        }
                    }
                    else
                    {

                        TempData.Keep("SmsCode");
                        //return "0";
                        ret = "0";
                    }
                }
                else
                {

                    TempData.Keep("SmsCode");
                    //return "0";
                    ret = "0";
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return (ret);
        }
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            try
            {
                ViewBag.Status = 0;
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }
        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult ForgotPassword(EMRViewModels.ForgotPasswordModel model)
        {
            string rtvalue = "0";
            try
            {
                if (ModelState.IsValid)
                {
                    model = _repo.VerifyUsernameforPasswordChange(model);
                    if (model.UserName != null && model.MobileNo != null)
                    {
                        TempData["UserId"] = model.Id;
                        SendVerificationCode('F', model.MobileNo, model.Email);
                        //return Json("1");
                        rtvalue = "1";
                    }
                    else
                    {
                        rtvalue = "0";
                        //return Json("0");
                    }
                }
                else
                {
                    rtvalue = "0";
                    //return Json("0");
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return Json(rtvalue);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public string VerifySecretCode(string OTP)
        {
            string ret = "0";
            try
            {
                ViewBag.OTP = OTP;
                int pstatus = FPProcessStatus.SysFailure;
                if (!string.IsNullOrEmpty(OTP))
                {
                    if (TempData["SmsCode"] != null && OTP.Equals(TempData["SmsCode"].ToString()))
                    {
                        //return "1";
                        ret = "1";
                    }
                    else
                    {

                        TempData.Keep("SmsCode");
                        //return "0";
                        ret = "0";
                    }
                }
                else
                {

                    TempData.Keep("SmsCode");
                    //return "0";
                    ret = "0";
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return (ret);
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public string SetPassword(string Password)
        {
            try
            {
                if (!string.IsNullOrEmpty(Password))
                {
                    bool result = _repo.ResetPassword(Guid.Parse(TempData["UserId"].ToString()), Password);

                    var Docinfo = _repo.DoctorDetail(Guid.Parse(TempData["UserId"].ToString()));

                    SendVerificationCode('S', Docinfo.phone_number, Docinfo.email);

                    return "1";
                }
                else
                {
                    TempData.Keep("UserId");
                    return "0";
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
                TempData.Keep("UserId");
                return "0";

            }

        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ContactUs()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ContactUs(ContactusViewModel ContactusViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _repo.AddContactUs(ContactusViewModel);
                    ModelState.Clear();
                }
                else
                {
                    //ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                    return View(ContactusViewModel);
                }

            }

            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }




        [HttpGet]

        public ActionResult Feedback()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> UploadFileRevised()
        {
            string str = "";
            int stat = 500;
            try
            {
                var body = HttpContext.Request.InputStream;
                string fileName = HttpContext.Request.Headers["X_FILENAME"].ToString().ToLower();
                string extension = System.IO.Path.GetExtension(fileName);
                //var extension = fileName.EndsWith("png") ? "png" : (fileName.EndsWith("jpg") || fileName.EndsWith("jpeg") ? "jpeg" : "gif");
                //using (FileStream fs = System.IO.File.Create(string.Format(@"D:\{0}.{1}", Guid.NewGuid(), extension)))
                //var path = Path.Combine(Microsoft.Net.Http.Server.MapPath("~/App_Data/Images"), fileName);
                str = string.Format(@"\Images\Feedback\{0}{1}", Guid.NewGuid().ToString(), extension);
                string path = Server.MapPath("~/") + str;
                using (System.IO.FileStream fs = System.IO.File.Create(path))
                {
                    await body.CopyToAsync(fs);
                }
                str = str.Replace("\\", "/");
                stat = 200;
            }
            catch (Exception ex)
            {
                str = ex.Message;
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            var model = new
            {
                status = stat,
                path = stat == 200 ? System.IO.Path.GetFileName(str) : str
            };
            return Json(model);
        }



        [HttpPost]
        [AllowAnonymous]
        public int SendFeedback(FeedBackParams objFeed)
        {
            try
            {

                List<string> rpath = new List<string>();

                string rootpath = HttpContext.Server.MapPath("~/images");

                if (!string.IsNullOrEmpty(objFeed.path))
                {

                    string[] values = objFeed.path.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        rpath.Add(values[i].ToString());
                    }

                }
                return EMailer.SendEmailWithAttachmentFeed(objFeed.strSubject, objFeed.strFeedback, "support_phrms@cdac.in", true, rpath, rootpath) ? 1 : 0;
            }
            catch (Exception ex)
            {

                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return 0;
        }


        public class FeedBackParams
        {
            public string strFeedback { get; set; }
            public string strSubject { get; set; }
            public List<string> lstAttachment { get; set; }
            public string path { get; set; }
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {

                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            try
            {
                return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
            }
            catch (Exception ex)
            {

                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }
        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            try
            {
                Session.Clear();
                Session.Abandon();

                Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                //create new sessionID
                SessionIDManager manager = new SessionIDManager();
                manager.RemoveSessionID(System.Web.HttpContext.Current);
                var newId = manager.CreateSessionID(System.Web.HttpContext.Current);
                var isRedirected = true;
                var isAdded = true;
                manager.SaveSessionID(System.Web.HttpContext.Current, newId, out isRedirected, out isAdded);

                //redirect so that the response.cookies call above actually enacts
                Response.Redirect("/Account/Login");

                FormsAuthentication.SignOut();
                //  Response.Redirect("/Account/Login");
                // return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {

                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Account");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        [HandleError]
        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}