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
    public class AppointmentsController : Controller
    {
        IEMRRepository _repo;
        public AppointmentsController()
        {
            _repo = new EMRRepository();
        }
        [AuthorizationFilter]
        public ActionResult Index(int Type = 1, int page = 1, int From = 1)
        {


            if (Request.IsAjaxRequest() == true)
            {

                if (From != 2 && Session["Type"] != null)
                {
                    Type = (int)Session["Type"];
                }
            }

            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
            IPagedList<Appointment_FeesViewModel> oModel = _repo.GetAppointmentsByDate(DocId, Type, page);
            ViewBag.TotalVisits = _repo.GetTotalAppointmentByDoctor(DocId);
            // return Json(new { response = oModel, status = oModel == null ? 0 : 1 });
            Session["Type"] = Type;
            if (Request.IsAjaxRequest() == true)
            {

                return PartialView("_AppointmentsGrid", oModel); //for grid view
            }
            else
            {

                return View(oModel);
            }

        }

        public List<PatientDataforGraphModel> GetPatientDataForGraph()
        {

            CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["DocSessionData"]);
            return GetPatientData(oCustomPrincipalSerializeModel.DocId);
        }
        public List<PatientDataforGraphModel> GetPatientData(Guid docId)
        {
            EMRDBContext _db = new EMRDBContext();
            var Res = (from p in _db.DocPatientDetails
                       where p.DocId.Equals(docId)
                       orderby p.CreatedDate
                       select new PatientDataforGraphModel
                       {
                           Count = 1,
                           Date = p.CreatedDate.ToString()
                       }).ToList();

            return Res;

        }


    
        
        public ActionResult PatientRegionGraph()
        {

            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
            try
            {
                var res = _repo.PatientRegionGraph(DocId);
               var result = (from p in res                       
                           select new PatientCustomModel
                           {
                               StateName=p.StateName,
                               StateCount = p.StateCount                          
                            
                           }).ToList();
            

                return PartialView("_PatientRegionGraph", result);

            }
            catch (Exception ex)
            {
                return PartialView("_PatientRegionGraph", null);

            }
           
        }

        public ActionResult PatientGenderGraph()
        {
            try
            {
                Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
               var res = _repo.PatientGenderGraph(DocId);
                PatientCustomModel onj = new PatientCustomModel();
                onj.Count = res.Count;
                onj.MaleCount = res.MaleCount;
                onj.FeMaleCount = res.FeMaleCount;
                onj.OthersCount = res.OthersCount;
                return PartialView("_PatientGenderGraph", onj);
            }
          catch (Exception ex)
            {

                return PartialView("_PatientGenderGraph", null);
            }
        }

        public ActionResult EMRPatientDetail()
        {
            try
            {
                Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
                var res = _repo.EMRPatientDetail(DocId);
                PatientCustomModel obj = new PatientCustomModel();
                obj.Count = res.Count;
               obj.Day= res.Day;
                obj.Week = res.Week;
                obj.Month =  res.Month; ;
                return PartialView("_EMRPatientDetail", obj);
            }
            catch (Exception ex)
            {

                return PartialView("_EMRPatientDetail", null);
            }
 
        }


        public ActionResult GetPatientLastWeekDetail()
        {
            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
            try
            {
                var objects = _repo.GetPatientLastWeekDetail(DocId);
                Models.WeekModel obj = new Models.WeekModel();
                obj.Sunday = objects.Sunday;
                obj.Monday = objects.Monday;
                obj.Tuesday = objects.Tuesday;
                obj.Wednesdaty = objects.Wednesdaty;
                obj.Thursday = objects.Thursday;
                obj.Friday = objects.Friday;
                obj.Saturday = objects.Saturday;
                ViewBag.Data = obj;
                return PartialView("_PatientMapRepresentation", null);
            }
            catch (Exception ex)
            {

                return PartialView("_PatientMapRepresentation", null);
            }
        }
    }
}