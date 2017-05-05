using System;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Authorization;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using Microsoft.AspNet.Hosting;
using System.Text.RegularExpressions;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    //[Authorize]
    [SessionExpire]
    public class HealthConditionController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oHealthConditionComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;
        public HealthConditionController(CatalogService objHealthConditionComponent)
        {
            oHealthConditionComponent = objHealthConditionComponent;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");

            }

            return View();
        }

        public IActionResult AddHealthCondition()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");

            }
            return View();
        }

        [HttpGet]
        public JsonResult GetHealthConditionGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try { 
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oHealthConditionComponent.GetHealthConditionGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult SaveHealthCondition([FromBody]HealthConditionViewModel obj)
        {
            try {

                if (!string.IsNullOrEmpty(obj.Provider))
                { 
                     var tagWithoutClosingRegex = new Regex(@"<[^>]+>");
                     var hasTags = tagWithoutClosingRegex.IsMatch(obj.Provider);
                    if (!hasTags)
                    {
                        obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                        return Json(oHealthConditionComponent.AddHealthCondition(obj) ? 1 : 0);

                       
                    }
                    else
                    {
                       return Json("The field cannot contain html tags");
                    }
                }
                return Json("The field cannot contain html tags");



                //    obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                //return Json(oHealthConditionComponent.AddHealthCondition(obj) ? 1 : 0);

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult DeleteHealthCondition([FromBody]Guid oGuid)
        {
            try { 
            var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oHealthConditionComponent.DeleteHealthCondition(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetHealthConditionById([FromBody]Guid oGuid)
        {
            try { 
            return Json(oHealthConditionComponent.GetHealthConditionById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetHealthConditionTypes([FromBody]string str)
        {
            try { 
            return Json(oHealthConditionComponent.GetHealthConditionMaster(str));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
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
                using (FileStream fs = System.IO.File.Create(string.Format(@"D:\phr_images\healthcondition_images\{0}.{1}", Guid.NewGuid(), extension)))
                {
                    await body.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
            }
            return new HttpStatusCodeResult(200);
        }

        public IActionResult Export()
        {
            try
            {
                var lstHealthCondition = oHealthConditionComponent.GetHealthConditionExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
                System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
                dicModels.Add("HealthCondition", "Health Condition");
                dicModels.Add("strDiagnosisDate", "Diagnosis Date");
                dicModels.Add("strServiceDate", "Service Date");
                dicModels.Add("strStillHaveCondition", "Still have condition?");
                var result = new ExcelFileResult(lstHealthCondition, dicModels);
                dicModels = null;
                lstHealthCondition = null;

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
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "HealthCondition");
                return null;
    }
}
    }
}
