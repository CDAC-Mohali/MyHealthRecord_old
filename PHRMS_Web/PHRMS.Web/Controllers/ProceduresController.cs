using System;
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
    public class ProceduresController : Controller
    {
        //private IPHRMSRepo _phrmsRepo;
        private CatalogService oCatalogService;
        private readonly IHostingEnvironment _appHostingEnvironment;
        public ProceduresController(CatalogService objCatalogService, IHostingEnvironment appHostingEnvironment)
        {
            oCatalogService = objCatalogService;
            _appHostingEnvironment = appHostingEnvironment;
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
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return View();
            }

        }

        public IActionResult AddAllergy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetProcedureMaster([FromBody]string str)
        {
            try
            {
                HttpContext.Session.Remove("ProcedureFileName");
                return Json(oCatalogService.GetProcedureMaster(str));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return null;
            }
        }


        [HttpGet]
        public JsonResult GetProceduresGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try
            {
                int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oCatalogService.GetProceduresGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult SaveProcedure([FromBody]ProceduresViewModel objModel)
        {
            try
            {
                string FileName = "";
                if (HttpContext.Session.GetString("ProcedureFileName") != null)
                    FileName = HttpContext.Session.GetString("ProcedureFileName");
                if (FileName != "")
                    objModel.lstFiles.Add(FileName);
                objModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                HttpContext.Session.Remove("ProcedureFileName");

              
            return Json(oCatalogService.SaveProcedure(objModel) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult DeleteProcedure([FromBody]Guid oGuid)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oCatalogService.DeleteProcedure(oGuid,userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetProcedureById([FromBody]Guid oGuid)
        {
            try
            {
            
                return Json(oCatalogService.GetProcedureById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
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
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
            }
            var model = new
            {
                status = stat,
                path = str.Replace(@"\",@"/"),
                name = strFileName
            };
            return Json(model);
        }

        public IActionResult Export()
        {
            try
            {
                var lstProcedures = oCatalogService.GetProceduresExportableList(Guid.Parse(HttpContext.Session.GetString("UserId")));
            System.Collections.Generic.Dictionary<string, string> dicModels = new System.Collections.Generic.Dictionary<string, string>();
            dicModels.Add("ProcedureName", "Procedure Name");
            dicModels.Add("SurgeonName", "Diagnosed by Doctor/Hospital");
            dicModels.Add("strEndDate", "Date of Procedure");
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
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Procedures");
                return null;
            }
        }
    }
}
