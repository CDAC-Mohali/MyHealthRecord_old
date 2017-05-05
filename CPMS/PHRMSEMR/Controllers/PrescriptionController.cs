using EMRLib.DAL;
using EMRViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PHRMSEMR.FilterConfig;
using PagedList;
using PHRMSEMR.Models;

namespace PHRMSEMR.Controllers
{
    [Error]
    [AuthorizationFilter]
    public class PrescriptionController : Controller
    {
        IEMRRepository _repo;

        public PrescriptionController()
        {
            _repo = new EMRRepository();
        }
        // GET: Prescription
        public ActionResult Index()
        {
            try
            {
              return View();
            }
           
             catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return View();
        }
        public ActionResult CloseSession()
        {
            var obj = ((CustomPrincipalSerializeModel)Session["DocSessionData"]);
            obj.DocPatientId = 0;
            obj.IsDetailActive = false;
            obj.PhrmsUserId = new Guid();
            Session["DocSessionData"] = (CustomPrincipalSerializeModel)obj;
            return RedirectToAction("Index", "Patient");
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetImmunizationTypes(string strImmmn)
        {
            try
            {
            return Json(_repo.GetImmunizationMaster(strImmmn));
           }
           catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json("");
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetLabTestMaster(string str)
        {
            try
            {
                return Json(_repo.GetLabTestMaster(str));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json("");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetProcedureMaster(string str)
        {
            try
            { 
            return Json(_repo.GetProcedureMaster(str));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json("");
        }

        [HttpPost]
        public JsonResult GetAllergyTypes(string str)
        {
            try
            { 
            return Json(_repo.GetAllergyMaster(str));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json("");
        }

        [HttpPost]
        public JsonResult GetProbleType(string str)
        {
            try
            {
               return Json(_repo.GetProbleMaster(str));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json("");
        }


        [HttpPost]
        public JsonResult GetMedicationMaster(string str)
        {
            try
            { 
            return Json(_repo.GetMedicationMaster(str));
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json("");
        }

        [HttpPost]
        public JsonResult SaveEMRComplete(EMRViewModels.EMRComplete oEMRComplete)
        {
            try
            {
                var obj = ((CustomPrincipalSerializeModel)Session["DocSessionData"]);
                oEMRComplete.DocId = obj.DocId;
                oEMRComplete.UserId = obj.PhrmsUserId;
                oEMRComplete.SourceId = 2;
                return Json(_repo.SaveEMRComplete(oEMRComplete) ? 1 : 0);
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");

            }
            return Json(0);
        }
        public string RenderViewToString<T>(string viewPath, T model)
        {
            ViewData.Model = model;
            using (var writer = new StringWriter())
            {
                var view = new WebFormView(ControllerContext, viewPath);
                var vdd = new ViewDataDictionary<T>(model);
                var viewCxt = new ViewContext(ControllerContext, view, vdd,
                                            new TempDataDictionary(), writer);
                viewCxt.View.Render(viewCxt, writer);
                return writer.ToString();
            }
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public ActionResult PreviewEMR(EMRViewModels.EMRComplete oEMRComplete)
        {
            if (Request.IsAjaxRequest())
            {
                Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
                long DocPatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocPatientId;
                oEMRComplete.DoctorDetail = _repo.DoctorDetail(DocId);
                oEMRComplete.PatientDetail = _repo.EMRPatientDetail(DocPatientId);
                oEMRComplete.PlaceViewModel = _repo.PlaceViewModel(DocId);
                if (oEMRComplete.Medications != null)
                    foreach (var Item in oEMRComplete.Medications)
                    {
                        Item.MedicineName = _repo.GetMedicationName(Item.MedicineType);
                        Item.strFrequency = _repo.GetFrequencyName(Item.Frequency);
                        Item.strDosValue = _repo.GetDosageValue(Item.DosValue);
                        Item.strDosUnit = _repo.GetDosageUnit(Item.DosUnit);
                        Item.strRoute = _repo.GetMedicineRoute(Item.Route);
                    }
                if (oEMRComplete.Allergies != null)
                    foreach (var Item in oEMRComplete.Allergies)
                    {
                        Item.AllergyName = _repo.GetAlleryType(Item.AllergyType);
                        Item.strDuration = _repo.GetAllergyDuration(Item.DurationId);
                        Item.strSeverity = _repo.GetAllergySeverity(Item.Severity);
                    }
                if (oEMRComplete.Problem != null)
                    foreach (var Item in oEMRComplete.Problem)
                    {
                        Item.ProblemName = _repo.GetProblemName(Item.ConditionType);                   
                    }

                if (oEMRComplete.Advice != null)
                    foreach (var Item in oEMRComplete.Advice)
                    {

                        if (ReportParameters.Immunizations == Item.ModuleId)
                            Item.Name = _repo.GetImmunization(Item.TypeId);
                        else if (ReportParameters.Tests == Item.ModuleId)
                            Item.Name = _repo.GetLabTest(Item.TypeId);
                        else if (ReportParameters.Procedures == Item.ModuleId)
                            Item.Name = _repo.GetProcedureParameters(Item.TypeId);
                    }
                return Json(new { error = true, message = RenderRazorViewToString("_Preview", oEMRComplete) });
            }
            return PartialView("_Preview", new SuperViewModel());
        }
        public SuperPatientDetails GetData()
        {
            SuperPatientDetails oSuperPatientDetails = new SuperPatientDetails();
            if (Session["DocSessionData"] != null && ((CustomPrincipalSerializeModel)Session["DocSessionData"]).IsDetailActive == true)
            {
                Guid PatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).PhrmsUserId;
                oSuperPatientDetails.PersonalViewModel = _repo.GetPersonalInformation(PatientId);

                oSuperPatientDetails.EmergencyViewModel = _repo.GetEmergencyInformation(PatientId);

                oSuperPatientDetails.EmployerViewModel = _repo.GetEmployerInformation(PatientId);

                oSuperPatientDetails.InsuranceViewModel = _repo.GetInsuranceInformation(PatientId);

                oSuperPatientDetails.LegalViewModel = _repo.GetLegalInformation(PatientId);

                oSuperPatientDetails.PreferencesViewModel = _repo.GetPreferences(PatientId);

                oSuperPatientDetails.AllergyViewModel = _repo.GetAllergiesCompleteList(PatientId);

                oSuperPatientDetails.HealthConditionViewModel = _repo.GetHealthConditionExportableList(PatientId);

                oSuperPatientDetails.MedicationViewModel = _repo.GetMedicationCompleteList(PatientId);

                oSuperPatientDetails.ImmunizationViewModel = _repo.GetImmunizationCompleteList(PatientId);

                oSuperPatientDetails.LabTestViewModel = _repo.GetLabTestCompleteList(PatientId);

                oSuperPatientDetails.ProceduresViewModel = _repo.GetProceduresCompleteList(PatientId);

                oSuperPatientDetails.ActivitiesViewModel = _repo.GetActivitiesCompleteList(PatientId);

                oSuperPatientDetails.BloodPressureAndPulseViewModel = _repo.GetBloodPressureAndPulseCompleteList(PatientId);

                oSuperPatientDetails.WeightViewModel = _repo.GetWeightsCompleteList(PatientId);

                oSuperPatientDetails.BloodGlucoseViewModel = _repo.GetBloodGlucoseCompleteList(PatientId);
            }
            return oSuperPatientDetails;
            //}
        }
        public ActionResult CheckOTP(string OTP, int Type)
        {

            var obj = ((CustomPrincipalSerializeModel)Session["DocSessionData"]);
            bool Result = _repo.CheckPHRMSOTPShare(obj.PhrmsUserId, OTP, Type);
            if (Result)
            {
                obj.IsDetailActive = true;
            }
            else
            {
                obj.IsDetailActive = false;
            }
            Session["DocSessionData"] = (CustomPrincipalSerializeModel)obj;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_PatientDetail", GetData());
            }
            return RedirectToAction("Index", "Prescription");
        }
        public ActionResult PatientDetail()
        {
            
            return PartialView("_PatientDetail", GetData());
            //}
        }

        public List<EprescriptionViewModel> GetPrescriptionList()
        {
            List<EprescriptionViewModel> list = null;
            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
            Guid PatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).PhrmsUserId;
            list = _repo.GetPrescriptionList(DocId, PatientId);
            return list;
        }


        public ActionResult GetPreviousVisitData()
        {
            List<DoctorUserMappingViewModel> list = null;
            Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
            Guid PatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).PhrmsUserId;
            list = _repo.GetPreviousVisitData(PatientId, DocId);
            return PartialView("_PreviousVisits", list);
        }

        public ActionResult ViewDetail(Guid Id)
        {
            try
            {
                var list = _repo.ViewDetal(Id);
                return View(list);
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Prescription");
            }
           return View();
        }


      
        public JsonResult GetBPandPulseData()
        {
            try
            {

                Guid PatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).PhrmsUserId;

                var abc = _repo.GetBPandPulseData(PatientId);
                return Json(abc);
            }

            catch (Exception ex)
            {
              return null;
            }
        }


        public JsonResult GetGlucoseData()
        {
            try
            {
                Guid PatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).PhrmsUserId;
                var result= _repo.GetGlucoseData(PatientId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public JsonResult GetHealthTip()
        //{
        //    string str = "";
        //    try
        //    {
        //        Random random = new Random();
        //        int num = random.Next(1, 99);
        //        str = _repo.GetHealthTip(num);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return Json(str);
        //}

    }
}