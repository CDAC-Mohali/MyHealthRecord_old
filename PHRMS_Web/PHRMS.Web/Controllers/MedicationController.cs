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
    public class MedicationController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oMedicationComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;
        public MedicationController(CatalogService objMedicationComponent)
        {
            oMedicationComponent = objMedicationComponent;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
            }
            return View();
        }

        public IActionResult AddMedication()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetMedicationGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var records = oMedicationComponent.GetMedicationGridList(userId, page, limit, sortBy, direction, searchString, out total);
                return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetRoutes()
        {
            try
            {
                return Json(oMedicationComponent.GetRoutes());
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetDosageValues()
        {
            try
            {
                return Json(oMedicationComponent.GetDosageValues());
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetDosageUnits()
        {
            try
            {
                return Json(oMedicationComponent.GetDosageUnits());
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetFrequencies()
        {
            try
            {
                return Json(oMedicationComponent.GetFrequencies());
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult SaveMedication([FromBody]MedicationViewModel obj)
        {
            try
            {

                string FileName = "";
                if (HttpContext.Session.GetString("MedicationFileName") != null)
                    FileName = HttpContext.Session.GetString("MedicationFileName");
                if (FileName != "")
                    obj.lstFiles.Add(FileName);
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                HttpContext.Session.Remove("MedicationFileName");

                return Json(oMedicationComponent.AddMedicine(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult DeleteMedicine([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(oMedicationComponent.DeleteMedicine(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetMedicineById([FromBody]Guid oGuid)
        {

            try
            {
                return Json(oMedicationComponent.GetMedicineById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetMedicationMaster([FromBody]string str)
        {
            try
            {
                HttpContext.Session.Remove("MedicationFileName");
                string[] arr = str.Split(',');
                return Json(oMedicationComponent.GetMedicationMaster(arr[0], int.Parse(arr[1])));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return Json("");
            }
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var lstMedications = oMedicationComponent.GetMedicationExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
                System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
                dicModels.Add("MedicineName", "Medication Name");
                dicModels.Add("strPrescribedDate", "Prescribed Date");
                dicModels.Add("Strength", "Strength");
                dicModels.Add("strTakingMedicine", "Still Taking Medication?");
                var result = new ExcelFileResult(lstMedications, dicModels);
                dicModels = null;
                lstMedications = null;

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
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Medication");
                return null;
            }
        }
    }
}
