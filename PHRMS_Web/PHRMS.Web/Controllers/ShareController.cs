using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewEngines;
using Microsoft.AspNet.Mvc.ViewFeatures;
using PHRMS.BLL;
using PHRMS.ViewModels;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{

    public class ShareController : Controller
    {

        private CatalogService _repository;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public ShareController(CatalogService repository)
        {
            _repository = repository;

        }
        [SessionExpire]
        public ActionResult ShowReportData(string strModules)
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var Result = _repository.GetReport(userId, strModules);
                return View(Result);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Share");
                return null;
            }
        }
        public ActionResult Index()
        {
            try
            {
                ViewBag.Status = TempData["StatusMessage"];
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Share");
                return null;
            }
        }

        public ActionResult ShowReport(string Password)
        {
            try
            {
                if (Password == null || Password == "")
                    return RedirectToAction("Index");
                var Result = _repository.GetShareReport(Password);
                if (Result != null && !Result.Status.Contains("Expired"))
                    return View(Result);
                else
                {
                    if (Result != null)
                        TempData["StatusMessage"] = Result.Status;

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Share");
                return null;
            }
        }
        [HttpPost]
        public JsonResult SaveFeedBack([FromBody] ShareFeedBack oShareFeedBack)
        {
            try
            {
                string DoctorName = "";
                var Result = _repository.SaveFeedBack(oShareFeedBack, out DoctorName);
                if (Result)
                {
                    try
                    {

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = "{\"userID\":\"" + oShareFeedBack.UserId + "\"," +
                                "\"Message\":\"Response received from Dr." + DoctorName + "\"," +
                                          "\"Module\":\"Share\"}";

                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                return Json(Result);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Share");
            }
            return Json("");
        }
    }



}
