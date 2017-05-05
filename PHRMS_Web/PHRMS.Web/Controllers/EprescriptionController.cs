using System;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Hosting;
//using Microsoft.AspNet.Authorization;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    //[Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EprescriptionController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oPrescriptionComponent;

        private readonly IHostingEnvironment _appHostingEnvironment;
        [SessionExpire]
        public ActionResult ShowReport(Guid EPrescriptionId)
        {
            try { 
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var Result = oPrescriptionComponent.GetPrescriptionReport(userId, EPrescriptionId);
            //    if (Result != null && !Result.Status.Contains("Expired"))
            return View(Result);

                 }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return null;
            }

        }

        public EprescriptionController(CatalogService objPrescriptionComponent)
        {
            try
            {

            oPrescriptionComponent = objPrescriptionComponent;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
              
            }

        }

        [SessionExpire]
        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
               
            }
            return View();
        }
        public IActionResult ShareIndex()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");

            }
            return View();
        }
        [HttpGet]
        [SessionExpire]
        public JsonResult GetSharePrescriptionGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var records = oPrescriptionComponent.GetSharePrescriptionGridList(userId, page, limit, sortBy, direction, searchString, out total);
                return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return Json("");
            }

        }

        [HttpGet]
        [SessionExpire]
        public JsonResult GetPrescriptionGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            { 
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oPrescriptionComponent.GetPrescriptionGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return Json("");
            }

        }

        [HttpPost]
        [SessionExpire]
        public JsonResult SavePrescription([FromBody]EprescriptionViewModel obj)
        {
            try { 
            obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oPrescriptionComponent.AddPrescription(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return Json("");
            }
        }

        [HttpPost]
        [SessionExpire]
        public JsonResult DeletePrescription([FromBody]Guid oGuid)
        {
            try
            {

            
            var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oPrescriptionComponent.DeletePrescription(oGuid, userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return Json("");
            }
        }

        [HttpPost]
        [SessionExpire]
        public JsonResult GetPrescriptionById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oPrescriptionComponent.GetPrescriptionById(oGuid));
            }     
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return Json("");
            }
}

        [HttpPost]
        [SessionExpire]
        public JsonResult GetPersonById()
        {
            try
            {
                Data.PersonalInformation obj = new Data.PersonalInformation();
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var final = Json(oPrescriptionComponent.GetPersonById(obj));
                return final;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return null;
            }
        }

        [SessionExpire]
        public ActionResult ViewDetail(Guid EPrescriptionId)
        {
            try
            {
                return View(oPrescriptionComponent.ViewDetal(EPrescriptionId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return null;
            }

        }
        [SessionExpire]
        public IActionResult Export()
        {
            try { 
            var lstProcedures = oPrescriptionComponent.GetPrescriptionExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
            dicModels.Add("DocName", "Doctors Name");
            dicModels.Add("ClinicName", "Clinic Name");
            dicModels.Add("strPresDate", "Prescription Date");
            var result = new ExcelFileResult(lstProcedures, dicModels);
            dicModels = null;
            lstProcedures = null;

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
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
                return null;
            }
        }

        [HttpPost]
        [SessionExpire]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var body = HttpContext.Request.Body;
                var fileName = HttpContext.Request.Headers["X_FILENAME"].ToString();
                var extension = fileName.EndsWith("png") ? "png" : "jpeg";
                //using (FileStream fs = System.IO.File.Create(string.Format(@"D:\{0}.{1}", Guid.NewGuid(), extension)))
                //var path = Path.Combine(Microsoft.Net.Http.Server.MapPath("~/App_Data/Images"), fileName);
                using (FileStream fs = System.IO.File.Create(string.Format(@"D:\phr_images\eprescription_images\{0}.{1}", Guid.NewGuid(), extension)))
                {
                    await body.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Eprescription");
            }
            return new HttpStatusCodeResult(200);
        }


    }
}
