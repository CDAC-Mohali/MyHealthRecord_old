using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using PHRMS.BLL;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Media;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    public class CommonController : Controller
    {
        private CatalogService _repository;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public CommonController(CatalogService repository, IHostingEnvironment appHostingEnvironment)
        {
            _repository = repository;
            _appHostingEnvironment = appHostingEnvironment;
        }

        [SessionExpire]
        public JsonResult GetDoctorsList()
        {
            try
            {
                return Json(_repository.GetDoctorsList(Guid.Parse(HttpContext.Session.GetString("UserId"))));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }

        /// <summary>
        /// This method is used to get list of pincodes from PinCodes table 
        /// </summary>
        /// <param name="strPostalCode">partial pincode.</param>
        /// <returns></returns>
        /// 
        [SessionExpire]
        public JsonResult GetPostalCodesFromMaster([FromBody]string strPostalCode)
        {
            try
            {
                return Json(_repository.GetPostalCodesFromMaster(strPostalCode).ToArray());
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult IsPhoneOrEmail(string notification)
        {
            try
            {


                Regex phoneRegex = new Regex(@"^([0-9\(\)\/\+ \-]*)$");
                Regex emailRegex = new Regex("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");

                return Json(emailRegex.IsMatch(notification) || phoneRegex.IsMatch(notification));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }

        [HttpPost]
        [SessionExpire]
        public String SaveFileFromDigiLocker(string user_ref, string doc_id, string doc_url)
        {

            return "SUCCESS";
        }

        [HttpPost]
        [SessionExpire]
        public string GetProfilePercentage()
        {
            try
            {


                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return (_repository.GetProfilePercentage(userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return "";
            }
        }
        [HttpPost]
        [SessionExpire]
        public JsonResult GetUserActivityExportableList()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(_repository.GetUserActivityExportableList(userId, 10));
            }

            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }

        [HttpGet]
        [SessionExpire]
        public void ExportPdf(string strModules)
        {
            try
            {
                //var res = oAllergyComponent.GetPersonalInfoDictionary(Guid.Parse(HttpContext.Session.GetString("UserId")));
                var configuration = new ReportConfiguration();
                //configuration.PageOrientation = PageSize.A4.Rotate();
                configuration.LogoPathLeft
                    = _appHostingEnvironment.WebRootPath + @"\Images\Digital_india_pdf_logo.png";
                configuration.LogoPathRight
                    = _appHostingEnvironment.WebRootPath + @"\Images\cdac_logo.png";
                configuration.LogImageScalePercent = 50;
                configuration.ReportTitle = "MyHealthRecord";
                configuration.ReportSubTitle = "Health Record Details"; //"Printed on " + DateTime.Now.ToString("dd/MM/yyyy");
                                                                        //configuration.dictTempProfileInfo = oAllergyComponent.GetCompleteDictionary("1,2,3,4,5", Guid.Parse(HttpContext.Session.GetString("UserId")));
                var report = new PdfTabularReport();
                report.ReportConfiguration = configuration;
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var r1 = _repository.GetDataRowDictionary(strModules, userId);
                var r2 = _repository.GetDisplayColumnsDictionary(strModules);
                var stream = report.GetPdf(r1, r2, _repository.GetCompleteDictionary(strModules, userId));
                Response.Clear();
                Response.ContentType = "application/pdf";
                //Response.Headers.Add("content-disposition","attachment;filename=" + HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf");
                string fullname = HttpContext.Session.GetString("FullName");
                var fstname = fullname.Split(' ');
                string firstName = fstname[0];
                string lastName = fstname[1];
                var lstName = lastName.Substring(0, 1).ToUpper();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                Response.Headers.Add("content-disposition", "attachment;filename=" + "PHRMS_" + firstName + lstName + "_" + date + "_HealthHistory.pdf");
                byte[] content = stream.ToArray();
                Response.Body.Write(content, 0, content.Length);
                //Response.BinaryWrite(stream.ToArray());
                Response.Body.Flush();
                Response.Body.Dispose();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");

            }
        }




        //  [HttpPost]
        [SessionExpire]
        public JsonResult SendHealthReport([FromBody]ShareHealthPars ShareHealthPars)
        {
            try
            {
                Guid UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                //  var stream = report.GetPdf(_repository.GetDataRowDictionary(strModules, userId), _repository.GetDisplayColumnsDictionary(strModules), _repository.GetCompleteDictionary(strModules, userId));

                string status = _repository.AddUserShareRecordEntry(UserId, ShareHealthPars.strChecks, ShareHealthPars.ValidUpto, ShareHealthPars.EmailAddress, ShareHealthPars.PhoneNumber, ShareHealthPars.Query);
                if (status != "")
                {
                    System.Threading.Thread thread1 = new System.Threading.Thread(delegate ()
                    {
                        CatalogService.sendInfiniSMS(ShareHealthPars.PhoneNumber, "The Pin for share health report is " + status);
                    });
                    thread1.IsBackground = true;
                    thread1.Start();
                    var Path = @HttpContext.Request.Host.Value.ToString() + "/share";
                    if (!Path.Contains("http"))
                    {
                        Path = "http://" + Path;
                    }

                    string bodysignup1 = string.Empty;
                    using (StreamReader reader = new StreamReader(_appHostingEnvironment.WebRootPath + "\\Templates\\ShareReport.html"))
                    {
                        bodysignup1 = reader.ReadToEnd();
                    }
                    bodysignup1 = bodysignup1.Replace("{Title}", "Hi there!");
                    //bodysignup1 = bodysignup1.Replace("{rno}", Convert.ToString(status));
                    bodysignup1 = bodysignup1.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");
                    string sms_content = "You can check your healthrecord report at : <a href='" + Path + "'>" + Path + "</a >.<br/> The pin for share health report is " + status + ".";
                    bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                    EMailer.SendEmail("Health History", bodysignup1, ShareHealthPars.EmailAddress, true);

                    //EMailer.SendEmail("Health History", "Please check the report at given link : <a href='" + Path + "'>" + Path +"</a > Your Pin for share health report is " + status, ShareHealthPars.EmailAddress, true);
                }
                return Json("");
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }
        [HttpPost]
        [SessionExpire]
        public JsonResult SavePdf([FromBody]string strModules)
        {
            try
            {
                //var res = oAllergyComponent.GetPersonalInfoDictionary(Guid.Parse(HttpContext.Session.GetString("UserId")));
                var configuration = new ReportConfiguration();
                //configuration.PageOrientation = PageSize.A4.Rotate();
                configuration.LogoPathLeft
                    = _appHostingEnvironment.WebRootPath + @"\Images\Digital_india_pdf_logo.png";
                configuration.LogoPathRight
                    = _appHostingEnvironment.WebRootPath + @"\Images\cdac_logo.png";
                configuration.LogImageScalePercent = 50;
                configuration.ReportTitle = "MyHealthRecord";
                configuration.ReportSubTitle = "Health Record Details"; //"Printed on " + DateTime.Now.ToString("dd/MM/yyyy");
                                                                        //configuration.dictTempProfileInfo = oAllergyComponent.GetCompleteDictionary("1,2,3,4,5", Guid.Parse(HttpContext.Session.GetString("UserId")));
                var report = new PdfTabularReport();
                report.ReportConfiguration = configuration;
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var stream = report.GetPdf(_repository.GetDataRowDictionary(strModules, userId), _repository.GetDisplayColumnsDictionary(strModules), _repository.GetCompleteDictionary(strModules, userId));
                //Response.Clear();
                //Response.ContentType = "application/pdf";
                //Response.Headers.Add("content-disposition",
                //"attachment;filename=" + HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf");
                string fName = HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf";
                byte[] content = stream.ToArray();
                System.IO.File.WriteAllBytes(_appHostingEnvironment.WebRootPath + "\\share\\" + fName, content);
                var Path = @HttpContext.Request.Host.Value.ToString() + "/share/";
                if (Path.Contains("localhost"))
                {
                    Path = "http://" + Path;
                }
                EMailer.SendEmail("Health History", Path + fName, "himanshu0619@gmail.com", true);
                return Json(fName);
                //Response.Body.Write(content, 0, content.Length);
                //Response.Body.Flush();
                //Response.Body.Dispose();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }

        [HttpPost]
        [SessionExpire]
        public JsonResult GetFirstAttachment([FromBody] CompFilePars oPars)
        {
            try
            {
                return Json(_repository.GetFirstAttachment(oPars.fileType, Guid.Parse(oPars.id)));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Common");
                return Json("");
            }
        }
    }
    public class CompFilePars
    {
        public int fileType { get; set; }
        public string id { get; set; }
    }
    public class ShareHealthPars
    {
        public string strChecks { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int ValidUpto { get; set; }
        public string Query { get; set; }
    }
}
