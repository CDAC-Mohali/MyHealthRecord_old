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
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [SessionExpire]
    public class ProfileController : Controller
    {

        private CatalogService _repository;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public ProfileController(CatalogService repository, IHostingEnvironment appHostingEnvironment)
        {
            _repository = repository;
            _appHostingEnvironment = appHostingEnvironment;
        }

        // GET: /<controller>/
        public IActionResult UserProfile()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return View(); 
            }
        }

        // GET: /<controller>/
        public IActionResult ChangePicture()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return View();
            }
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {
                ViewBag.RelationshipList = _repository.GetRelationshipList();
            ViewBag.StatesList = _repository.GetStatesList();
            ViewBag.DisablitiesList = _repository.GetDisablitiesList();
            ViewBag.BloodGroupsList = _repository.GetBloodGroupsList();
            ViewBag.GendersList = _repository.GetGendersList();
            ViewBag.PreferredHospitalList = _repository.GetPreferredHospitalList();
            return View(_repository.GetProfileDetails(Guid.Parse(HttpContext.Session.GetString("UserId"))));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePersonalInformation(PersonalViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                ViewBag.StatesList = _repository.GetStatesList();
                ViewBag.DisablitiesList = _repository.GetDisablitiesList();
                ViewBag.BloodGroupsList = _repository.GetBloodGroupsList();
                ViewBag.GendersList = _repository.GetGendersList();
                if (ModelState.IsValid)
                {
                    model = await _repository.SavePersonalInformation(model);
                    if (model != null)
                    {
                        HttpContext.Session.SetString("FirstName", model.FirstName);
                        HttpContext.Session.SetString("FullName", model.FirstName + " " + model.LastName);
                    }
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
            }
            return Json(new { error = model.StatusCode, view = RenderPartialViewToString("_personalInformation", model) });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveEmergencyInformation(EmergencyViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            ViewBag.StatesList = _repository.GetStatesList();
            ViewBag.RelationshipList = _repository.GetRelationshipList();
            if (ModelState.IsValid)
            {
                model = await _repository.SaveEmergencyInformation(model);
            }
            return Json(new { error = model.StatusCode, view = RenderPartialViewToString("_emergencyInformation", model) });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveEmployerInformation(EmployerViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            ViewBag.StatesList = _repository.GetStatesList();
            if (ModelState.IsValid)
            {
                model = await _repository.SaveEmployerInformation(model);
            }
            return Json(new { error = model.StatusCode, view = RenderPartialViewToString("_employerInformation", model) });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveInsuranceInformation(InsuranceViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                if (ModelState.IsValid)
                {
                    model = await _repository.SaveInsuranceInformation(model);
                }
                return Json(new { error = model.StatusCode, view = RenderPartialViewToString("_insuranceInformation", model) });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SavePreferencesInformation(PreferencesViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            ViewBag.PreferredHospitalList = _repository.GetPreferredHospitalList();
            if (ModelState.IsValid)
            {
                model = await _repository.SavePreferencesInformation(model);
            }
            return Json(new { error = model.StatusCode, view = RenderPartialViewToString("_prefrences", model) });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveLegalInformation(LegalViewModel model)
        {
            try
            {
                model.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            if (ModelState.IsValid)
            {
                model = await _repository.SaveLegalInformation(model);
            }
            return Json(new { error = model.StatusCode, view = RenderPartialViewToString("_legalInformation", model) });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            try
            {
                if (string.IsNullOrEmpty(viewName))
                viewName = ActionContext.ActionDescriptor.Name;

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                var engine = Resolver.GetService(typeof(ICompositeViewEngine))
                    as ICompositeViewEngine;
                ViewEngineResult viewResult = engine.FindPartialView(ActionContext, viewName);

                ViewContext viewContext = new ViewContext(
                    ActionContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    sw,
                    new HtmlHelperOptions() //Added this parameter in
                );

                //Everything is async now!
                var t = viewResult.View.RenderAsync(viewContext);
                t.Wait();

                return sw.GetStringBuilder().ToString();
            }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ChangePicture(string imgpath)
        {
            try
            {
                bool result = _repository.SetProfilePic(imgpath, Guid.Parse(HttpContext.Session.GetString("UserId")));
            if (result)
            {
                HttpContext.Session.SetString("UserImgPath", imgpath);
            }
            return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
                return null;
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            string str = "";
            int stat = 500;
            try
            {
                var body = HttpContext.Request.Body;
                var fileName = HttpContext.Request.Headers["X_FILENAME"].ToString().ToLower();
                string extension = Path.GetExtension(fileName);
                //var extension = fileName.EndsWith("png") ? "png" : (fileName.EndsWith("jpg") || fileName.EndsWith("jpeg") ? "jpeg" : "gif");
                //using (FileStream fs = System.IO.File.Create(string.Format(@"D:\{0}.{1}", Guid.NewGuid(), extension)))
                //var path = Path.Combine(Microsoft.Net.Http.Server.MapPath("~/App_Data/Images"), fileName);
                str = string.Format(@"\Images\ProfilePics\Custom\{0}{1}", Guid.NewGuid(), extension);
                using (FileStream fs = System.IO.File.Create(_appHostingEnvironment.WebRootPath + str))
                {
                    await body.CopyToAsync(fs);
                }
                str = str.Replace("\\", "/");
                stat = 200;
            }
            catch (Exception ex)
            {
                str = ex.Message;
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
            }
            var model = new
            {
                status = stat,
                path = str
            };
            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> UploadFileRevised()
        {
            string str = "";
            int stat = 500;
            var fileName = "";
            try
            {
                var fileType = HttpContext.Request.Headers["X_FILETYPE"];
                if (!String.IsNullOrEmpty(fileType))
                {
                    var body = HttpContext.Request.Body;
                    fileName = HttpContext.Request.Headers["X_FILENAME"];
                    string extension = Path.GetExtension(fileName);
                    FileViewModel oModel = new FileViewModel();

                    if (int.Parse(fileType) == (int)FileType.DisablityCert)
                    {
                        oModel.FileType = FileType.DisablityCert;
                        str = string.Format(@"\Images\DisabilityCert\{0}{1}", Guid.NewGuid().ToString(), extension);
                        oModel.FileName = Path.GetFileName(str);
                        oModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                        stat = _repository.SaveFilePath(oModel) ? 200 : 500;
                    }
                    else if (int.Parse(fileType) == (int)FileType.LabReport)
                    {
                        str = string.Format(@"\Images\LabReports\{0}{1}", Guid.NewGuid().ToString(), extension);
                    }
                    else if (int.Parse(fileType) == (int)FileType.Feedback)
                    {
                        str = string.Format(@"\Images\Feedback\{0}{1}", Guid.NewGuid().ToString(), extension);
                    }
                    else
                    {
                        oModel.FileType = FileType.ProfilePic;
                        str = string.Format(@"\Images\ProfilePics\Custom\{0}{1}", Guid.NewGuid().ToString(), extension);
                        oModel.FileName = Path.GetFileName(str);
                        oModel.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                        stat = _repository.SaveFilePath(oModel) ? 200 : 500;
                    }
                    using (FileStream fs = System.IO.File.Create(_appHostingEnvironment.WebRootPath + str))
                    {
                        await body.CopyToAsync(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Profile");
            }
            var model = new
            {
                status = stat,
                path = str.Replace(@"\", @"/"),
                filename = fileName
            };
            return Json(model);
        }

         //27 March 2017
        public JsonResult GetPatientDetails()
        {
            try
            {
                Guid PatientId = (Guid.Parse(HttpContext.Session.GetString("UserId")));
               var res = _repository.GetPatientDetails(PatientId);
            
                return Json(res);
            }

            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
