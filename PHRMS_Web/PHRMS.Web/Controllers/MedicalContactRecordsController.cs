using System;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [SessionExpire]
    public class MedicalContactRecordsController : Controller
    {
        private CatalogService oMedicalContactsComponent;
        private CatalogService _repository;
        private readonly IHostingEnvironment _appHostingEnvironment;

        public MedicalContactRecordsController(CatalogService repository, CatalogService objMedicalContactsComponent)
        {
            oMedicalContactsComponent = objMedicalContactsComponent;
            _repository = repository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            try { 
            ViewBag.StatesList = _repository.GetStatesList();
            ViewBag.ContactType = _repository.GetContactTypesList();


        }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "MedicalContactRecords");
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetContactGridList(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            try { 
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oMedicalContactsComponent.GetContactGridList(userId, page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "MedicalContactRecords");
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult SaveContacts([FromBody]MedicalContactRecordsViewModel obj)
        {
            try
            {
                obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return Json(oMedicalContactsComponent.AddContact(obj) ? 1 : 0);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "MedicalContactRecords");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult DeleteContact([FromBody]Guid oGuid)
        {
            try { 
            return Json(oMedicalContactsComponent.DeleteContact(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "MedicalContactRecords");
                return Json("");
            }

        }
        [HttpPost]
        public JsonResult GetContactById([FromBody]Guid oGuid)
        {
            try { 
            return Json(oMedicalContactsComponent.GetContactById(oGuid));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "MedicalContactRecords");
                return Json("");
            }
        }


    }
}
