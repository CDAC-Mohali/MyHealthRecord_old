using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.AspNet.Hosting;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [SessionExpire]
    public class LabController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oLabComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public LabController(CatalogService objMedicationComponent, IHostingEnvironment appHostingEnvironment)
        {
            oLabComponent = objMedicationComponent;
            _appHostingEnvironment = appHostingEnvironment;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");

            }

            return View();
        }

        [HttpGet]
        public JsonResult GetLabTestResultGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                var records = oLabComponent.GetLabTestResultGridList(userId, page, limit, sortBy, direction, searchString, out total);
                return Json(new { records, total });
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult SaveTest([FromBody]LabTestViewModel obj)
        {
            try
            {
                string FileName = "";
                if (HttpContext.Session.GetString("LabReportsFileName") != null)
                    FileName = HttpContext.Session.GetString("LabReportsFileName");
                if (FileName != "")
                    obj.lstFiles.Add(FileName);
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                HttpContext.Session.Remove("LabReportsFileName");
                return Json(oLabComponent.AddTest(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
                return Json("");
            }
        }


        [HttpPost]
        public JsonResult DeleteResult([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(oLabComponent.DeleteResult(oGuid, userId));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetResultById([FromBody]Guid oGuid)
        {
            try
            {
                return Json(oLabComponent.GetResultById(oGuid));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetLabTestMaster([FromBody]string str)
        {
            try
            {
                HttpContext.Session.Remove("LabReportsFileName");
                return Json(oLabComponent.GetLabTestMaster(str));
            }
            catch (Exception ex)
            {

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
                return Json("");
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            string str = "";
            string strFileName = "";
            int stat = 500;
            try
            {
                var body = HttpContext.Request.Body;
                var fileName = HttpContext.Request.Headers["X_FILENAME"].ToString();
                var dir = HttpContext.Request.Headers["X_DIRECTORY"].ToString();
                var extension = fileName.EndsWith("png") ? ".png" : (fileName.EndsWith("jpg") || fileName.EndsWith("jpeg") ? ".jpeg" : ".gif");
                strFileName = Guid.NewGuid().ToString() + extension;
                str = string.Format(@"Images\{0}\{1}", dir, strFileName);
                using (FileStream fs = System.IO.File.Create(_appHostingEnvironment.WebRootPath + "\\" + str))
                {
                    await body.CopyToAsync(fs);
                }
                stat = 200;
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
            }
            var model = new
            {
                status = stat,
                path = str.Replace(@"\", @"/"),
                name = strFileName
            };
            return Json(model);
        }

        public IActionResult ExportExcel()
        {
            try
            {
                var lstLabTest = oLabComponent.GetLabTestExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
                System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
                dicModels.Add("TestName", "Test Name");
                dicModels.Add("Result", "Result");
                dicModels.Add("Unit", "Unit");
                dicModels.Add("strPerformedDate", "Performed Date");
                var result = new ExcelFileResult(lstLabTest, dicModels);
                dicModels = null;
                lstLabTest = null;

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

                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Lab");
                return null;
            }
        }
    }
}
