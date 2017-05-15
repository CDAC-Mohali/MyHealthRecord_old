using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PHRMS.Data.DataAccess;
using Newtonsoft.Json;
using System.Net.Http;
using PHRMS.BLL;
using PHRMS.ViewModels;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Cors;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    public class DigiLocker
    {
        public string user_ref { get; set; }
        public string doc_id { get; set; }
        public string doc_url { get; set; }
        public string file_upload_server { get; set; }
    }
    [EnableCors("AllowAll")]
    //   [Route("api/[controller]")]
    public class AccountController : Controller
    {
        //  private readonly IHostingEnvironment _appHostingEnvironment;


        public string GetExtensionfromContentType(string ContentType)
        {
            string Extenstion = ".jpeg";
            switch (ContentType)
            {
                case "image/jpeg":
                    Extenstion = ".jpeg";
                    break;
                case "image/png":
                    Extenstion = ".png";
                    break;
                case "image/bmp":
                    Extenstion = ".bmp";
                    break;
                case "image/gif":
                    Extenstion = ".gif";
                    break;
                case "text/csv":
                    Extenstion = ".csv";
                    break;
                case "text/html":
                    Extenstion = ".html";
                    break;
                case "application/msword":
                    Extenstion = ".doc";
                    break;
                case "application/pdf":
                    Extenstion = ".pdf";
                    break;
                case "application/zip":
                    Extenstion = ".zip";
                    break;
                default:
                    Extenstion = ".jpeg";
                    break;
            }
            return Extenstion;
        }
        [HttpPost]
        [AllowAnonymous]
        //  [Route("UploadDigiLockerDocument")]
        public string UploadDigiLockerDocument([FromBody]string user_ref, [FromBody] string doc_id, [FromBody]string doc_url, string file_upload_server)
        {
            try
            {
                //WriteToFile(user_ref);
                //WriteToFile(doc_id);
                //WriteToFile(doc_url);
                //WriteToFile("Assigning New Values to variables");

                // WriteToFile(file_upload_server);
                //  WriteToFile("Here"+ _appHostingEnvironment.WebRootPath.ToString());
                JObject json = JObject.Parse(file_upload_server);
                //   WriteToFile("OKay" + _appHostingEnvironment.WebRootPath.ToString());
                user_ref = (string)json["user_ref"];
                doc_id = (string)json["doc_id"];
                doc_url = (string)json["doc_url"];
                //WriteToFile(user_ref);
                //WriteToFile(doc_id);
                //WriteToFile(doc_url);

                Guid id = Guid.NewGuid();
                using (var client = new WebClient())
                {
                    try
                    {
                        string BasePath = _appHostingEnvironment.WebRootPath;// HttpContext.Current.Server.MapPath("~/ApplicationLogs");// System.Configuration.ConfigurationManager.AppSettings["BaseFolderPath"].ToString();
                        //HttpContext.Session.SetString("MobileNo", model.MobileNo);
                        string path = BasePath + "\\Images\\LabReports\\";// + userId;// +"\\"+ user_ref;
                        //   var path = "d:\\" + user_ref;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //byte[] fileBytes = client.DownloadData(doc_url);
                        string filename = Guid.NewGuid().ToString();// + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString());

                        // string fileType = client.ResponseHeaders[HttpResponseHeader.ContentType];

                        client.DownloadFile(
                            doc_url, path + filename);//path + "\\MyFile_" + user_ref +".pdf");                        //;
                        System.IO.File.Move(path + filename, path + filename + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString()));
                        HttpContext.Session.SetString("LabReportsFileName", filename + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString()));
                        //WriteToFile(filename);
                        // System.IO.File.WriteAllBytes(path + filename, fileBytes);
                        return "SUCCESS";
                    }
                    catch (Exception ex)
                    {
                        while (ex != null)
                        {
                            Console.WriteLine(ex.Message);
                            ex = ex.InnerException;
                            WriteToFile(ex.Message.ToString());
                        }
                        return "FAILURE";
                    }
                }
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");

            }
            return "FAILURE";

        }

        [HttpPost]
        [AllowAnonymous]
        //  [Route("UploadDigiLockerDocument")]
        public string UploadDigiLockerProcedureDocument([FromBody]string user_ref, [FromBody] string doc_id, [FromBody]string doc_url, string file_upload_server)
        {
            try
            {
                //WriteToFile(user_ref);
                //WriteToFile(doc_id);
                //WriteToFile(doc_url);
                //WriteToFile("Assigning New Values to variables");

                // WriteToFile(file_upload_server);
                //  WriteToFile("Here"+ _appHostingEnvironment.WebRootPath.ToString());
                JObject json = JObject.Parse(file_upload_server);
                //   WriteToFile("OKay" + _appHostingEnvironment.WebRootPath.ToString());
                user_ref = (string)json["user_ref"];
                doc_id = (string)json["doc_id"];
                doc_url = (string)json["doc_url"];
                //WriteToFile(user_ref);
                //WriteToFile(doc_id);
                //WriteToFile(doc_url);

                Guid id = Guid.NewGuid();
                using (var client = new WebClient())
                {
                    try
                    {
                        string BasePath = _appHostingEnvironment.WebRootPath;// HttpContext.Current.Server.MapPath("~/ApplicationLogs");// System.Configuration.ConfigurationManager.AppSettings["BaseFolderPath"].ToString();
                        //HttpContext.Session.SetString("MobileNo", model.MobileNo);
                        string path = BasePath + "\\Images\\Procedures\\";// + userId;// +"\\"+ user_ref;
                        //   var path = "d:\\" + user_ref;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //byte[] fileBytes = client.DownloadData(doc_url);
                        string filename = Guid.NewGuid().ToString();// + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString());

                        // string fileType = client.ResponseHeaders[HttpResponseHeader.ContentType];

                        client.DownloadFile(
                            doc_url, path + filename);//path + "\\MyFile_" + user_ref +".pdf");                        //;
                        System.IO.File.Move(path + filename, path + filename + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString()));
                        HttpContext.Session.SetString("ProcedureFileName", filename + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString()));
                        //WriteToFile(filename);
                        // System.IO.File.WriteAllBytes(path + filename, fileBytes);
                        return "SUCCESS";
                    }
                    catch (Exception ex)
                    {
                        while (ex != null)
                        {
                            Console.WriteLine(ex.Message);
                            ex = ex.InnerException;
                            WriteToFile(ex.Message.ToString());
                        }
                        return "FAILURE";
                    }
                }
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");

            }
            return "FAILURE";

        }

        [HttpPost]
        [AllowAnonymous]
        //  [Route("UploadDigiLockerDocument")]
        public string UploadDigiLockerMedicationDocument([FromBody]string user_ref, [FromBody] string doc_id, [FromBody]string doc_url, string file_upload_server)
        {
            try
            {
                //WriteToFile(user_ref);
                //WriteToFile(doc_id);
                //WriteToFile(doc_url);
                //WriteToFile("Assigning New Values to variables");

                // WriteToFile(file_upload_server);
                //  WriteToFile("Here"+ _appHostingEnvironment.WebRootPath.ToString());
                JObject json = JObject.Parse(file_upload_server);
                //   WriteToFile("OKay" + _appHostingEnvironment.WebRootPath.ToString());
                user_ref = (string)json["user_ref"];
                doc_id = (string)json["doc_id"];
                doc_url = (string)json["doc_url"];
                //WriteToFile(user_ref);
                //WriteToFile(doc_id);
                //WriteToFile(doc_url);

                Guid id = Guid.NewGuid();
                using (var client = new WebClient())
                {
                    try
                    {
                        string BasePath = _appHostingEnvironment.WebRootPath;// HttpContext.Current.Server.MapPath("~/ApplicationLogs");// System.Configuration.ConfigurationManager.AppSettings["BaseFolderPath"].ToString();
                        //HttpContext.Session.SetString("MobileNo", model.MobileNo);
                        string path = BasePath + "\\Images\\Medidocs\\";// + userId;// +"\\"+ user_ref;
                        //   var path = "d:\\" + user_ref;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //byte[] fileBytes = client.DownloadData(doc_url);
                        string filename = Guid.NewGuid().ToString();// + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString());

                        // string fileType = client.ResponseHeaders[HttpResponseHeader.ContentType];

                        client.DownloadFile(
                            doc_url, path + filename);//path + "\\MyFile_" + user_ref +".pdf");                        //;
                        System.IO.File.Move(path + filename, path + filename + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString()));
                        HttpContext.Session.SetString("MedicationFileName", filename + GetExtensionfromContentType(client.ResponseHeaders["content-type"].ToString()));
                        //WriteToFile(filename);
                        // System.IO.File.WriteAllBytes(path + filename, fileBytes);
                        return "SUCCESS";
                    }
                    catch (Exception ex)
                    {
                        while (ex != null)
                        {
                            Console.WriteLine(ex.Message);
                            ex = ex.InnerException;
                            WriteToFile(ex.Message.ToString());
                        }
                        return "FAILURE";
                    }
                }
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");

            }
            return "FAILURE";

        }
        //public ActionResult TestExternalUrl()
        //{
        //    // In some cases you might want to pull completely different URL that is not related to your application.
        //    // You can do that by specifying full URL.
        //    string html = string.Empty;
        //    string url = @"http://localhost:55291/Account/TestExternalUrl";

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //   // request.AutomaticDecompression = DecompressionMethods.GZip;

        //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        //    using (Stream stream = response.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        html = reader.ReadToEnd();
        //    }

        //    byte[] toBytes = Encoding.ASCII.GetBytes(html);
        //    return File(toBytes, "application/pdf","test.pdf");

        //}

        private CatalogService _catalogService;

        private readonly ILogger _logger;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public AccountController(CatalogService catalogService, ILogger<AccountController> logger, IHostingEnvironment appHostingEnvironment)
        {
            _catalogService = catalogService;
            _logger = logger;
            _appHostingEnvironment = appHostingEnvironment;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult FAQS()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View();
        }
        // GET: /<controller>/
        [SessionExpire]
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View();
        }

        [SessionExpire]
        public IActionResult Dashboard()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            try
            {
                ViewBag.StatesList = _catalogService.GetStatesList();
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult ContactUs()
        {
            try
            {
                ViewBag.ContactUSStatesList = _catalogService.GetContactUSStatesList();
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View();
        }


        public IActionResult ChangePassword()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                    if (_catalogService.ChangesPassowrd(userId, model.NewPassword, model.OldPassword))
                    {
                        ModelState.AddModelError("CustomValidation", "Password Changed Successfully.");
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError("CustomValidation", "Please enter Valid Password.");
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ContactUs(ContactusViewModel ContactusViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    ViewBag.ContactUSStatesList = _catalogService.GetContactUSStatesList();
                    var result = _catalogService.AddContactUs(ContactusViewModel);
                    ModelState.Clear();
                }

            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");


            }
            return Redirect("ContactUs");
        }



        [AllowAnonymous]
        [HttpPost]
        public string SaveImages(string klk)
        {
            try
            {
                string strFileName = "";
                var bytes = Convert.FromBase64String(klk);
                strFileName = Guid.NewGuid().ToString() + ".png";
                string FilePath = _appHostingEnvironment.WebRootPath + "\\" + strFileName;
                //string fsfsf = Startup.Configuration["ImagBaseUrl"];
                //string FilePath = "E:/PHRMS-WebServices-Publish/Service_Latest/wwwroot/Images/Prescription/" + strFileName;
                //  System.IO.File.AppendAllText("E:/Test/abc.txt", FilePath);
                using (var imageFile = new FileStream(FilePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");


            }
            return "success";
        }

        [SessionExpire]
        public IActionResult ActivitySummary()
        {
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            try
            {

            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");


            }
            return View(_catalogService.GetUserActivityExportableList(userId, 100));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Verify(RegistrationViewModel oModel)
        {

            try
            {

                //   _logger.LogInformation("You are at Verify");
                oModel = _catalogService.MigrateTempRegRecord(oModel);
                if (oModel.status.Equals(RegProcessCode.Success))
                {
                    SendVerificationCode('W', oModel.MobileNo, oModel.Email, oModel.FirstName);
                }
                //    _logger.LogInformation("Process Code now is: " + oModel.status);
                HttpContext.Session.Remove("SmsCode");
                //    _logger.LogInformation("You are out of Verify");

            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");


            }

            return View("Register", oModel);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {

            try
            {
                // _logger.LogInformation("You are at Register");
                string strMessage = "";
                if (ModelState.IsValid)
                {
                    if (await RecaptchaIsValid(Request.Form["g-recaptcha-response"]))
                    {
                        //TempData["MobileNo"] = model.MobileNo;
                        //TempData["EmailId"] = model.Email;
                        //HttpContext.Session.SetString("MobileNo", model.MobileNo);
                        //HttpContext.Session.SetString("EmailId", model.Email);
                        //if (SendVerificationCode('S', model.MobileNo, model.Email))
                        //{
                        //System.Threading.Thread thread1 = new System.Threading.Thread(delegate ()
                        //{
                        SendVerificationCode('S', model.MobileNo, model.Email, model.FirstName);
                        //});
                        //thread1.IsBackground = true;
                        //thread1.Start();
                        //TempData["Id"] = model.Id;
                        //TempData["ProcessCode"] = RegProcessCode.OtpVerification;
                        model.OTP = HttpContext.Session.GetString("SmsCode");

                        model = await _catalogService.Register(model);
                        //HttpContext.Session.SetString("Id", model.Id.ToString());
                        model.status = RegProcessCode.OtpVerification;
                        //HttpContext.Session.SetString("ProcessCode", RegProcessCode.OtpVerification);
                        return View("Register", model);
                        //}
                        //else
                        //    strMessage = "We are facing problem in sending Verification Code. Please try again after some time.";
                    }
                    else
                    {
                        strMessage = "Please answer the recaptcha challenge.";
                        ViewBag.StatesList = _catalogService.GetStatesList();
                    }

                    ModelState.AddModelError("CustomValidation", strMessage);
                }
                //   _logger.LogInformation("You are moving out of Register");
                // If we got this far, something failed, redisplay form
                //return RedirectToAction("Register");

            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");


            }

            return View();
        }

        private async Task<bool> RecaptchaIsValid(string captchaResponse)
        {
            //   _logger.LogInformation("You entered of RecaptchaIsValid");
            try
            {
                //WriteToFile("Before connectionFeature");
                var connectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress;
                var requestUrl =
                    String.Format(
                        "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}",
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
                        //    _logger.LogInformation("You out of RecaptchaIsValid: Status is true");
                        return true;
                    }
                }
                //     _logger.LogInformation("You out of RecaptchaIsValid: Status is false");
                return false;
            }
            catch (Exception ex)
            {
                //     _logger.LogInformation("You got Exception at RecaptchaIsValid: " + ex.Message);
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

                throw;
            }
        }

        private bool SendVerificationCode(char flag, string strMobileNo, string strEmailId, string FName)
        {
            //   _logger.LogInformation("You entered SendVerificationCode");
            bool result = false;
            bool bEmail = false;
            string sms_content = "";


            try
            {
                Random randomclass = new Random();
                Int64 rno = randomclass.Next(100000, 999999);
                //ViewBag.SmsCode = rno.ToString();
                //WriteToFile(rno.ToString() + "   " + DateTime.Now.ToString());
                //ViewState["smscode"] = rno.ToString();
                //ViewState["password"] = txtpass.Text;
                if (flag == 'S') //send code via SMS
                {

                    string bodysignup1 = string.Empty;
                    using (StreamReader reader = new StreamReader(_appHostingEnvironment.WebRootPath + "\\Templates\\SignUpStep_1.html"))
                    {
                        bodysignup1 = reader.ReadToEnd();
                    }
                    bodysignup1 = bodysignup1.Replace("{Title}", Convert.ToString(FName));
                    bodysignup1 = bodysignup1.Replace("{rno}", Convert.ToString(rno));
                    sms_content = "The verification code for registration at MyHealthRecord is" + " " + rno + ".";
                  //  _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS( strMobileNo, sms_content);
                    //   CatalogService.WriteErrorLog("flag-s " + "sendSingleSMS: " + result.ToString());
                    sms_content = "The verification code for registration at MyHealthRecord is";
                    bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Verification Code", bodysignup1, strEmailId, true);
                    // CatalogService.WriteErrorLog("flag-s " + "SendEmail: " + result.ToString());
                    result = result || bEmail;
                    HttpContext.Session.SetString("SmsCode", rno.ToString());
                    //HttpContext.Session.SetString("SmsCode", rno.ToString());
                    ////----Insert entry into AlertTypelog on SMS or Email alert
                    //objvacc.p_userid = Convert.ToInt64(ViewState["uid"]);
                    //objvacc.p_childid = Convert.ToInt64(ViewState["uid"]);//Convert.ToInt64(row[5]);
                    //objvacc.p_AlertDate = System.DateTime.Now;
                    //objvacc.p_contact = txtmobile.Text;
                    //objvacc.p_alertType = 'S';
                    //objvacc.P_AppID = 1;
                    //objvacc.MthAlertTypeLog();
                    HttpContext.Session.SetString("Name", FName);
                }
                else if (flag == 'R') //resend code via SMS
                {
                    string bodysignup1 = string.Empty;
                    using (StreamReader reader = new StreamReader(_appHostingEnvironment.WebRootPath + "\\Templates\\ResendVerfCode.html"))
                    {
                        bodysignup1 = reader.ReadToEnd();
                    }
                    string nmee = HttpContext.Session.GetString("Name");
                    bodysignup1 = bodysignup1.Replace("{Title}", nmee);
                    if (HttpContext.Session.GetString("SmsCode") != null)
                        sms_content = "The verification code for registration at MyHealthRecord is " + " " + HttpContext.Session.GetString("SmsCode") + ".";
                    else
                    {
                        string strOtp = _catalogService.FetchOTPFromTemp(strMobileNo);
                        sms_content = "The verification code for registration at MyHealthRecord is " + " " + strOtp + ".";
                        HttpContext.Session.SetString("SmsCode", strOtp);
                    }
                  //  _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS( strMobileNo, sms_content);


                    sms_content = "The verification code for registration at MyHealthRecord is" + " " + HttpContext.Session.GetString("SmsCode") + ".";
                    bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Verification Code", bodysignup1, strEmailId, true);
                    //      CatalogService.WriteErrorLog("flag-R " + "SendEmail: " + result.ToString());
                    result = result || bEmail;
                    //TempData.Keep("SmsCode");
                    ////----Insert entry into AlertTypelog on SMS or Email alert
                    //objvacc.p_userid = Convert.ToInt64(ViewState["uid"]);
                    //objvacc.p_childid = Convert.ToInt64(ViewState["uid"]);//Convert.ToInt64(row[5]);
                    //objvacc.p_AlertDate = System.DateTime.Now;
                    //objvacc.p_contact = txtmobile.Text;
                    //objvacc.p_alertType = 'S';
                    //objvacc.P_AppID = 1;
                    //objvacc.MthAlertTypeLog();
                }
                else if (flag == 'W') //resend code via SMS
                {
                    string bodysignup1 = string.Empty;
                    using (StreamReader reader = new StreamReader(_appHostingEnvironment.WebRootPath + "\\Templates\\SuccessReg.html"))
                    {
                        bodysignup1 = reader.ReadToEnd();
                    }
                    string nme = HttpContext.Session.GetString("Name");

                    bodysignup1 = bodysignup1.Replace("{Title}", nme);
                    sms_content = "Thank you for registering at MyHealthRecord. You can now access your health record at http://myhealthrecord.nhp.gov.in.";
                   // _catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS( strMobileNo, sms_content);
                    sms_content = "Thank you for registering at MyHealthRecord. You can now access your health record at http://myhealthrecord.nhp.gov.in.";
                    bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                    result = result | EMailer.SendEmail("Registration Successful", bodysignup1, strEmailId, true);
                    ////----Insert entry into AlertTypelog on SMS or Email alert
                    //objvacc.p_userid = Convert.ToInt64(ViewState["uid"]);
                    //objvacc.p_childid = Convert.ToInt64(ViewState["uid"]);//Convert.ToInt64(row[5]);
                    //objvacc.p_AlertDate = System.DateTime.Now;
                    //objvacc.p_contact = txtmobile.Text;
                    //objvacc.p_alertType = 'S';
                    //objvacc.P_AppID = 1;
                    //objvacc.MthAlertTypeLog();
                }
                else if (flag == 'F')
                {
                    sms_content = "The verification code for changing the password at MyHealthRecord is " + rno + ".";
                    //_catalogService.BeforeSMSsend();
                    result = CatalogService.sendInfiniSMS( strMobileNo, sms_content);
                    HttpContext.Session.SetString("SmsCode", rno.ToString());
                }
            }
            catch (Exception ex)
            {
                //   CatalogService.WriteErrorLog("From SendVerificationCode: " + ex.Message);
                //     _logger.LogInformation("You got Exception at SendVerificationCode: " + ex.Message);
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            //   _logger.LogInformation("You are out of SendVerificationCode");
            return result;
        }

        protected void WriteToFile(string msg)
        {
            try
            {
                // Compose a string that consists of three lines.
                string lines = msg + DateTime.Now.ToString();
                string dirPath = _appHostingEnvironment.WebRootPath + "/TestFiles";
                bool exists = System.IO.Directory.Exists(dirPath);

                if (!exists)
                    System.IO.Directory.CreateDirectory(dirPath);
                // Write the string to a file.
                using (System.IO.StreamWriter w = System.IO.File.AppendText(dirPath + "/OTP.txt"))
                {
                    w.WriteLine(lines);
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
        }

        [HttpPost]
        public JsonResult ResendVerificationCode([FromBody]ContactInfo info)
        {
            bool res = false;
            string code = "";

            try
            {


                res = SendVerificationCode('R', info.strMobileNo, info.strEmailId, info.FirstName);
                code = HttpContext.Session.GetString("SmsCode");
                //TempData.Keep("SmsCode"); TempData.Keep("MobileNo"); TempData.Keep("EmailId");

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }
            return Json(new { res, code });
        }

        [HttpPost]
        public JsonResult DoesMobileExistForOTP(string OTPMobileNo)
        {
            try
            {
                return Json(_catalogService.DoesMobileExistForOTP(OTPMobileNo));
            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");
                return Json("");
            }

        }
        [HttpPost]
        public JsonResult DoesMobileExist(string MobileNo)
        {
            try
            {
                return Json(_catalogService.DoesMobileExist(MobileNo));
            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");
                return Json("");
            }

        }
        [HttpPost]
        public JsonResult DoesMobileExistMedical([FromBody]FeedBackParams obj)
        {

            try
            {

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                bool result = _catalogService.DoesMobileExistMedical(userId, obj.PrimaryPhone, obj.EmailAddress);


                if (result == true)
                {
                    return Json("1");
                }
                else
                {
                    return Json("0");
                }


            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

                return Json("0");
            }
        }

        [HttpPost]
        public JsonResult DoesEmailExist(string Email)
        {
            try
            {
                return Json(_catalogService.DoesEmailExist(Email));
            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");
                return Json("");
            }


        }

        [HttpPost]
        public JsonResult DoesEmailOrMobileExist(string UserName)
        {

            try
            {
                System.Text.RegularExpressions.Regex phoneRegex = new System.Text.RegularExpressions.Regex(@"^([0-9\(\)\/\+ \-]*)$");
                System.Text.RegularExpressions.Regex emailRegex = new System.Text.RegularExpressions.Regex("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

                if (emailRegex.IsMatch(UserName) || phoneRegex.IsMatch(UserName))
                {
                    if (_catalogService.DoesEmailOrMobileExist(UserName))
                        return Json(true);
                    else
                        return Json("This User Id is not registered with PHRMS!");
                }
                else
                    return Json("Not a valid User Id!");


            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

                return Json("Not a valid User Id!");
            }
        }

        [SessionExpire]
        public IActionResult Feedback()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }

            return View();
        }

        [HttpPost]
        [SessionExpire]
        public int SendFeedback([FromBody]FeedBackParams objFeed)
        {
            try
            {
                return EMailer.SendEmailWithAttachment(objFeed.strSubject, objFeed.strFeedback, "", true, objFeed.lstAttachment, _appHostingEnvironment.WebRootPath) ? 1 : 0;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");
                return 0;
            }

        }
        [HttpGet]
        public List<NotificationViewModel> GetNotifications()
        {
            List<NotificationViewModel> list = new List<NotificationViewModel>();
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                list = _catalogService.GetNotifications(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }

            return list;
        }

        public IActionResult Notification()
        {
            List<NotificationViewModel> list = new List<NotificationViewModel>();
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                list = _catalogService.GetNotifications(userId);
                //_catalogService.updateNotificationAfterViewedByUser(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Account");

            }

            return View(list);
        }
    }

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

    public class ContactInfo
    {
        public string strMobileNo { get; set; }
        public string strEmailId { get; set; }
        public string FirstName { get; set; }
    }


}
