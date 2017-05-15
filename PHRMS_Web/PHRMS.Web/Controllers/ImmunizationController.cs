using System;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.PlatformAbstractions;
using System.Collections.Generic;
using Microsoft.AspNet.Hosting;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [SessionExpire]
    public class ImmunizationController : Controller
    {
        private CatalogService oImmunizationComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;
        public ImmunizationController(CatalogService objImmunizationComponent)
        {
            oImmunizationComponent = objImmunizationComponent;
        }
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");

            }
            return View();
        }
        //GET: /<controller>/
        [HttpGet]
        public JsonResult GetImmunizationGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try { 
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oImmunizationComponent.GetImmunizationGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");
                return Json("");
            }
        }




        //To add a new row into the immunization table
        public IActionResult AddImmunization()
        {
            try
            {

            } 
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");

            }
            return View();
        }

        [HttpPost]
        public JsonResult SaveImmunization([FromBody]ImmunizationViewModel obj)
        {
            try { 
            obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oImmunizationComponent.AddImmunization(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");
                return Json("");
            }
        }

        //To delete a row into the immunization table


        [HttpPost]
        public JsonResult DeleteImmunization([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(oImmunizationComponent.DeleteImmunization(oGuid, userId));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetImmunizationById([FromBody]Guid oGuid)
        {
            try { 
            return Json(oImmunizationComponent.GetImmunizationById(oGuid));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetImmunizationTypes([FromBody]string strImmmn)
        {
            try
            {
                return Json(oImmunizationComponent.GetImmunizationMaster(strImmmn));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");
                return Json("");
            }
        }

        public IActionResult Export()
        {
            try { 
            var lstImmunization = oImmunizationComponent.GetlstImmunizationExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
            dicModels.Add("ImmunizationName", "Immunization");
            dicModels.Add("strImmunizationDate", "Taken On");
            dicModels.Add("Comments", "Comments");
            var result = new ExcelFileResult(lstImmunization, dicModels);
            dicModels = null;
            lstImmunization = null;

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

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Immunization");
                return null;
            }
        }
    }
}
