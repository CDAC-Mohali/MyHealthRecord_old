using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PHRMS.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [SessionExpire]
    public class OpenEMRController : Controller
    {
        private CatalogService oOpenEMRComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;
        private CatalogService _repository;
        int StateId = 0;
        int DistrictId = 0;

        public OpenEMRController(CatalogService repository, CatalogService objOpenEMRComponent)
        {
            oOpenEMRComponent = objOpenEMRComponent;
            _repository = repository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {
                ViewBag.OpenEMRStatesList = _repository.GetStatesOpenEMRList();
            ViewBag.DistrictListOnStateId = _repository.GetDistrictNameByStateId(StateId);
            ViewBag.SubDistrictListOnDistrictId = _repository.GetSubDistrictNameByDistrictId(DistrictId);
            return View();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "OpenEMR");
                return View();
            }
        }


        [HttpGet]
        public JsonResult GetOpenEMRGridList(int? page, int? limit, string sortBy, string direction,string city, string searchString = null)
        {
            try
            {
            if (city == "SAS Nagar (Mohali)")
            {
                city = "Mohali"; 
            }
            int total;
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            var records = oOpenEMRComponent.GetOpenEMRGridList(userId, city, page, limit, sortBy, direction,searchString, out total);
            return Json(new { records, total });
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "OpenEMR");
                return Json("");
            }
        }
        //[HttpPost]
        //public JsonResult SaveContacts([FromBody]MedicalContactRecordsViewModel obj)
        //{

        //    obj.UserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        //    return Json(oOpenEMRComponent.AddContact(obj) ? 1 : 0);
        //}

        [HttpPost]
        public JsonResult GetDistrictsById([FromBody]int id)
        {
            try
            {
                return Json(oOpenEMRComponent.GetDistrictNameByStateId(id));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "OpenEMR");
                return Json("");
            }
        }

        [HttpPost]
        public JsonResult GetSubDistrictNameByDistrictId([FromBody]int id)
        {
            try
            {
                return Json(oOpenEMRComponent.GetSubDistrictNameByDistrictId(id));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "OpenEMR");
                return Json("");
            }
        }
        //[HttpPost]
        //public JsonResult GetContactById([FromBody]Guid oGuid)
        //{
        //    return Json(oOpenEMRComponent.GetContactById(oGuid));
        //}

        [HttpGet]
        public JsonResult GetUserDetailsForOpenEMR()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return Json(oOpenEMRComponent.GetUserDetailsForOpenEMR(userId));
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "OpenEMR");
                return Json("");
            }
        }
        //[HttpGet]
        //public JsonResult AddRowToTrigger()
        //{
        //    oOpenEMRComponent.AddRowToTrigger();
        //    return Json("Success");
        //}

    }
}
