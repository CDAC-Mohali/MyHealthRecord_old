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
    [Error]
    [AuthorizationFilter]
    public class DashboardController : Controller
    {
        IEMRRepository _repo;
        public DashboardController()
        {
            _repo = new EMRRepository();
        }
        // GET: Dashboard

        public ActionResult Index()
        {
            //var sched = new DHXScheduler(this) { InitialDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) };
            //sched.Extensions.Add(SchedulerExtensions.Extension.PDF);

            ////load data initially
            //sched.LoadData = true;

            ////save client-side changes
            //sched.EnableDataprocessor = true;
            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;

            return View(_repo.AllGetEvents(DocId));

        }


        //public ContentResult Data()
        //{
        //    Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
        //    var data = new SchedulerAjaxData(_repo.AllGetEvents(DocId));
        //    return data;
        //}
        [HttpPost]
        public ActionResult LoadDataForAppointments(DateTime date)
        {
            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
            var data = _repo.LoadDataForAppointments(DocId, date);
            return Json(data);
        }
    }
}