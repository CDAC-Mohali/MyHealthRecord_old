using EMRLib.DAL;
using EMRViewModels;
using PagedList;
using PHRMSEMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PHRMSEMR.FilterConfig;

namespace PHRMSEMR.Controllers
{
    public class ConsultationsController : Controller
    {
        IEMRRepository _repo;
        public ConsultationsController()
        {
            _repo = new EMRRepository();
        }
        [AuthorizationFilter]
        public ActionResult Index(string Keyword = "", int page = 1)
        {
            IPagedList<EMRViewModels.DoctorUserMappingViewModel> oPatList = null;
            //ViewBag.DeleteMessage = TempData["DeleteMessage"];
            try
            {

                oPatList = _repo.GetConsultationsData(page, ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId, Keyword);




                if (Request.IsAjaxRequest() == true)
                {
                    return PartialView("_ConsultationsGrid", oPatList); //for searching in grid
                }
                else
                {
                    return View("Index", oPatList);
                }
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Patient");

            }
            return View(oPatList);
        }
    }
}