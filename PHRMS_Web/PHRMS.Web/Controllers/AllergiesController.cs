using System;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using PHRMS.Web.Models;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    //[Authorize]
   
    [SessionExpire]
    public class AllergiesController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oAllergyComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public AllergiesController(CatalogService objAllergyComponent)
        {
            oAllergyComponent = objAllergyComponent;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
               PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
            }
            return View();
        }

        public IActionResult AddAllergy()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");

            }
            return View();
        }

        //[HttpGet]
        //public JsonResult GetPlayers(int? page, int? limit, string sortBy, string direction, string searchString = null)
        //{
        //    int total;
        //    var records = new GridModel().GetPlayers(page, limit, sortBy, direction, searchString, out total);
        //    return Json(new { records, total });
        //}

        [HttpGet]
        public JsonResult GetAllergiesGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {


                int total;
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var records = oAllergyComponent.GetAllergiesGridList(userId, page, limit, sortBy, direction, searchString, out total);
                return Json(new { records, total });
            
             }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }
          }

        [HttpPost]
        public JsonResult GetAllergySeverities()
        {
            try
            {

                return Json(oAllergyComponent.GetAllergySeverities());
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetAllergyDurationList()
        {
            try
            {

            
            return Json(oAllergyComponent.GetAllergyDurationList());
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }

        }

        [HttpPost]
        public JsonResult SaveAllergy([FromBody]AllergyViewModel obj)
        {
            try
            {

            
            obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oAllergyComponent.AddAllergies(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult DeleteAllergy([FromBody]Guid oGuid)
        {
            try
            {


                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(oAllergyComponent.DeleteAllergy(oGuid, userId));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetAllergyById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oAllergyComponent.GetAllergyById(oGuid));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetAllergyTypes([FromBody]string str)
        {
            try
            {

            
            return Json(oAllergyComponent.GetAllergyMaster(str));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return Json("");
            }
        }

        public IActionResult ExportExcel()
        {
            try
            {            
            var lstAllergies = oAllergyComponent.GetAllergiesExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            Dictionary<string, string> dicModels = new Dictionary<string, string>();
            dicModels.Add("AllergyName", "Allergy Name");
            dicModels.Add("strStill_Have", "Still Have Allergy?");
            dicModels.Add("strDuration", "From");
            var result = new ExcelFileResult(lstAllergies, dicModels);
            dicModels = null;
            lstAllergies = null;

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

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Allergies");
                return null;
            }
        }
    }
}