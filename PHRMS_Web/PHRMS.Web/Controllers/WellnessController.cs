using System;
using System.Web.Helpers;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [SessionExpire]
    public class WellnessController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oWellnessComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;
        public WellnessController(CatalogService objWellnessComponent,IHostingEnvironment appHostingEnvironment)
        {
            oWellnessComponent = objWellnessComponent;
            _appHostingEnvironment = appHostingEnvironment;
        }

        // GET: /<controller>/
        public IActionResult bloodpressure()
        {
            try
            {
              return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return View();
           
        }
        public IActionResult glucose()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return View();
        }
        public IActionResult AddWellness()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return View();
        }
        public IActionResult activity()
        {
            try
            {
                 return View(oWellnessComponent.GetActivitiesMaster());
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return View(oWellnessComponent.GetActivitiesMaster());
            }
           
        }
        public IActionResult sleep()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return View();
        }
        public IActionResult temperature()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return View();
        }
        public IActionResult weight()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetBPAndPulseRecordsGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            { 
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oWellnessComponent.GetBPAndPulseRecordsGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpGet]
        public JsonResult GetSleepGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oWellnessComponent.GetSleepGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpGet]
        public JsonResult GetTemperatureGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oWellnessComponent.GetTemperatureGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpGet]
        public JsonResult GetWeightGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oWellnessComponent.GetWeightGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpGet]
        public JsonResult GetBloodGlucoseRecordsGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
             int total;
             Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oWellnessComponent.GetBloodGlucoseRecordsGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }


        [HttpGet]
        public JsonResult GetActivitiesRecordsGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oWellnessComponent.GetActivitiesRecordsGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
             }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult SaveBloodPressure([FromBody]BloodPressureAndPulseViewModel obj)
        {
            try
            {
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(oWellnessComponent.AddBloodPressure(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult SaveSleep([FromBody]SleepViewModel obj)
        {
            try
            {
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.AddSleep(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult SaveTemperature([FromBody]TemperatureViewModel obj)
        {
            try
            {
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.AddTemperature(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
         }

        [HttpPost]
        public JsonResult SaveWeight([FromBody]WeightViewModel obj)
        {
            try
            {
            obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.AddWeight(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        public JsonResult SaveActivities([FromBody]ActivitiesViewModel obj)
        {
            try
            {
            obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.AddActivity(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult SaveBloodGlucose([FromBody]BloodGlucoseViewModel obj)
        {
            try
            {
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.AddBloodGlucose(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult DeleteBloodPressureAndPulse([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.DeleteBloodPressureAndPulse(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult DeleteSleep([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.DeleteSleep(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult DeleteTemperature([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.DeleteTemperature(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult DeleteWeight([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.DeleteWeight(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult DeleteBloodGlucose([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.DeleteBloodGlucose(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }

        [HttpPost]
        public JsonResult DeleteActivities([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oWellnessComponent.DeleteActivities(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json(0);
            }
        }


        [HttpPost]
        public JsonResult UpdateBloodPressureAndPulse([FromBody]BloodPressureAndPulseViewModel oGuid)
        {
            try
            {
                return Json(oWellnessComponent.UpdateBloodPressureAndPulse(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult UpdateActivity([FromBody]ActivitiesViewModel oGuid)
        {
            try
            {
                return Json(oWellnessComponent.UpdateActivity(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult UpdateSleep([FromBody]SleepViewModel oGuid)
        {
            try
            {
                return Json(oWellnessComponent.UpdateSleep(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult UpdateTemperature([FromBody]TemperatureViewModel oGuid)
        {
            try
            {
                return Json(oWellnessComponent.UpdateTemperature(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult UpdateWeight([FromBody]WeightViewModel oGuid)
        {
            try
            {
                return Json(oWellnessComponent.UpdateWeight(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }


        [HttpPost]
        public JsonResult UpdateBloodGlucose([FromBody]BloodGlucoseViewModel oGuid)
        {
            try
            {
                return Json(oWellnessComponent.UpdateBloodGlucose(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetBloodPressureById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oWellnessComponent.GetBloodPressureById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetBloodGlucoseById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oWellnessComponent.GetBloodGlucoseById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetActivitiesById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oWellnessComponent.GetActivitiesById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetSleepById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oWellnessComponent.GetSleepById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetTemperatureById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oWellnessComponent.GetTemperatureById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }


        [HttpPost]
        public JsonResult GetWeightById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oWellnessComponent.GetWeightById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return Json("");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                Console.Write("Reached Here");
                var body = HttpContext.Request.Body;
                var fileName = HttpContext.Request.Headers["X_FILENAME"].ToString();
                var extension = fileName.EndsWith("png") ? "png" : "jpeg";
                //using (FileStream fs = System.IO.File.Create(string.Format(@"D:\{0}.{1}", Guid.NewGuid(), extension)))
                //var path = Path.Combine(Microsoft.Net.Http.Server.MapPath("~/App_Data/Images"), fileName);
                using (FileStream fs = System.IO.File.Create(string.Format(@"D:\phr_images\wellness_images\{0}.{1}", Guid.NewGuid(), extension)))
                {
                    await body.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
            }
            return new HttpStatusCodeResult(200);
        }

        public IActionResult ExportWeightsData()
        {
            try
            {
            var lstWeights = oWellnessComponent.GetWeightExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
            dicModels.Add("Result", "Weight (in Kg)");
            dicModels.Add("Goal", "Height (in cm)");
            dicModels.Add("Comments", "Comments");
            dicModels.Add("strCollectionDate", "Date");
            var result = new ExcelFileResult(lstWeights, dicModels);
            dicModels = null;
            lstWeights = null;

                Response.Clear();
                Response.ContentType = "application/excel";
                //Response.Headers.Add("content-disposition","attachment;filename=" + HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf");
                string fullname = HttpContext.Session.GetString("FullName");
                var fstname = fullname.Split(' ');
                string firstName = fstname[0];
                string lastName = fstname[1];
                var lstName = lastName.Substring(0, 1).ToUpper();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                Response.Headers.Add("content-disposition", "attachment;filename=" + "PHRMS_" + firstName + lstName + "_" + date + "_HealthHistory.xlsx");


                return result;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return null;
            }
           
        }

        public IActionResult ActivitiesDataExport()
        {
            try
            {
                var lstActivities = oWellnessComponent.GetActivitiesExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
            dicModels.Add("ActivityName", "Activity Name");
            dicModels.Add("PathName", "Path Name");
            dicModels.Add("FinishTime", "Total Time");
            dicModels.Add("Distance", "Distance");
            dicModels.Add("strCollectionDate", "Date");
            var result = new ExcelFileResult(lstActivities, dicModels);
            dicModels = null;
            lstActivities = null;

                Response.Clear();
                Response.ContentType = "application/excel";
                //Response.Headers.Add("content-disposition","attachment;filename=" + HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf");
                string fullname = HttpContext.Session.GetString("FullName");
                var fstname = fullname.Split(' ');
                string firstName = fstname[0];
                string lastName = fstname[1];
                var lstName = lastName.Substring(0, 1).ToUpper();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                Response.Headers.Add("content-disposition", "attachment;filename=" + "PHRMS_" + firstName + lstName + "_" + date + "_HealthHistory.xlsx");


                return result;
           }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return null;
            }
      }

        public ReportConfiguration GetPdfReportConfiguration()
        {
            try
            {
                var configuration = new ReportConfiguration();
            //configuration.PageOrientation = PageSize.A4.Rotate();
            configuration.LogoPathLeft
                = _appHostingEnvironment.WebRootPath + @"\Images\Digital_india_pdf_logo.png";
            configuration.LogoPathRight
                = _appHostingEnvironment.WebRootPath + @"\Images\cdac_logo.png";
            configuration.LogImageScalePercent = 50;
            configuration.ReportTitle = "Health Record Details";
            configuration.ReportSubTitle = "Printed on " + DateTime.Now.ToString("dd/MM/yyyy");
            configuration.dictProfileInfo = oWellnessComponent.GetBasicPersonalInfoDictionary(Guid.Parse(HttpContext.Session.GetString("UserId")));
            return configuration;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return null;
            }
        }

        public IActionResult ExportBloodGlucoseData()
        {
            try
            {
                var lstBloodGlucose = oWellnessComponent.GetBloodGlucoseExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
            dicModels.Add("Result", "Result");
            dicModels.Add("ValueType", "Value Type");
            dicModels.Add("Comments", "Comments");
            dicModels.Add("strCollectionDate", "Date");
            var result = new ExcelFileResult(lstBloodGlucose, dicModels);
            dicModels = null;
            lstBloodGlucose = null;

                Response.Clear();
                Response.ContentType = "application/excel";
                //Response.Headers.Add("content-disposition","attachment;filename=" + HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf");
                string fullname = HttpContext.Session.GetString("FullName");
                var fstname = fullname.Split(' ');
                string firstName = fstname[0];
                string lastName = fstname[1];
                var lstName = lastName.Substring(0, 1).ToUpper();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                Response.Headers.Add("content-disposition", "attachment;filename=" + "PHRMS_" + firstName + lstName + "_" + date + "_HealthHistory.xlsx");

                return result;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return null;
            }
        }

        public IActionResult ExportBPData()
        {
            try
            {
                var lstBloodPressureAndPulse = oWellnessComponent.GetBloodPressureAndPulseExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
                System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
                dicModels.Add("ResSystolic", "Result Systolic");
                dicModels.Add("ResDiastolic", "Result Diastolic");
                dicModels.Add("ResPulse", "Result Pulse");
                dicModels.Add("Comments", "Comments");
                dicModels.Add("strCollectionDate", "Date");
                var result = new ExcelFileResult(lstBloodPressureAndPulse, dicModels);
                dicModels = null;
                lstBloodPressureAndPulse = null;

                Response.Clear();
                Response.ContentType = "application/excel";
                //Response.Headers.Add("content-disposition","attachment;filename=" + HttpContext.Session.GetString("FullName").Replace(" ", "_") + "_HealthHistory.pdf");
                string fullname = HttpContext.Session.GetString("FullName");
                var fstname = fullname.Split(' ');
                string firstName = fstname[0];
                string lastName = fstname[1];
                var lstName = lastName.Substring(0, 1).ToUpper();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                Response.Headers.Add("content-disposition", "attachment;filename=" + "PHRMS_" + firstName + lstName + "_" + date + "_HealthHistory.xlsx");

                return result;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetBMIForGraph()
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var res = oWellnessComponent.GetBMIForGraph(userId);
            return Json(res);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Wellness");
                return null;
            }
        }
    }
}
