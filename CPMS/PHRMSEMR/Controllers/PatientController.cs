using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using EMRLib.DAL;
using PHRMSEMR.Models;
using EMRViewModels;
using static PHRMSEMR.FilterConfig;
using System.Threading;
using System.IO;

namespace PHRMSEMR.Controllers
{
    [AuthorizationFilter]
    public class PatientController : Controller
    {
        IEMRRepository _repo;

        public PatientController()
        {
            _repo = new EMRRepository();
        }

        public ActionResult RegisterNewPatient()
        {
            TempData["NewRegister"] = true;
            return RedirectToAction("Index");
        }

        public ActionResult Index(string Keyword = "", int page = 1)
        {
            ViewBag.NewRegister = TempData["NewRegister"];
            IPagedList<EMRViewModels.DocPatientDetailsViewModel> oPatList = null;
            //ViewBag.DeleteMessage = TempData["DeleteMessage"];
            try
            {
                if (Keyword == null || Keyword == "")
                {
                    oPatList = _repo.GetPatientsList(page, ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId);

                }
                else
                {
                    oPatList = _repo.GetPatientsListSearch(page, ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId, Keyword);

                }

                if (Request.IsAjaxRequest() == true)
                {
                    return PartialView("_PatientGrid", oPatList); //for searching in grid
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

        [HttpPost]
        public JsonResult CheckPatientByDoc(EMRViewModels.DocPatientDetailsViewModel oDocPatientDetailsViewModel)
        {
            try
            {
                oDocPatientDetailsViewModel.DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
                EMRViewModels.DocPatientDetailsViewModel oModel = _repo.CheckPatientByDoc(oDocPatientDetailsViewModel);
                return Json(new { response = oModel, status = oModel == null ? 0 : 1 });
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Patient");

            }
            return Json(1);
        }


        [HttpPost]

        public JsonResult GetPatientById(long PatId)
        {
            try
            {
                Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
                EMRViewModels.DocPatientDetailsViewModel oModel = _repo.GetPatientById(DocId, PatId);
                return Json(new { response = oModel, status = oModel == null ? 0 : 1 });
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Patient");

            }
            return Json(1);
        }
        private bool PHRMSendEmailSMS(string strMobileNo, string strEmailId, string UserName, string Password, string FirstName)
        {
            bool result = false;
            bool bEmail = false;
            string sms_content = "";
            ClsSendSMS sms = new ClsSendSMS();
            try
            {
                string bodyPatReg = string.Empty;
                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\PatientReg.html")))
                {
                    bodyPatReg = reader.ReadToEnd();
                }
                bodyPatReg = bodyPatReg.Replace("{Title}", Convert.ToString(FirstName));
                bodyPatReg = bodyPatReg.Replace("{p_username}", Convert.ToString(UserName));
                bodyPatReg = bodyPatReg.Replace("{p_password}", Convert.ToString(Password));
                bodyPatReg = bodyPatReg.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");


                sms_content = "You have been successfully registered with MyHealthRecord. Your Username : " + UserName + " and Password : " + Password + ".";
                //   sms.BeforeSMSsend();
                result = ClsSendSMS.sendInfiniSMS(strMobileNo, sms_content);
                if (strEmailId != null && strEmailId != "'")
                {
                    sms_content = "You have been successfully registered with MyHealthRecord.";
                    bodyPatReg = bodyPatReg.Replace("{messagephrms}", sms_content);
                    bEmail = EMailer.SendEmail("MyHealthRecord - Auto Registration", bodyPatReg, strEmailId, true);
                }
                result = result || bEmail;
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Patient");

            }
            return result;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SavePatientDetails(EMRViewModels.DocPatientDetailsViewModel model)
        {
            try
            {
                model.DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;
                UsersViewModel Status;
                Guid PhrmsUserId = _repo.SavePatientDetails(model, out Status);

                if (Status.Status && Status.IsEmailSend == true)
                {
                    Thread thread = new Thread(() => PHRMSendEmailSMS(model.PhoneNumber, model.EmailAddress, Status.MobileNo, Status.Password, model.FirstName));
                    thread.Start();

                }
                if (Status.Status)
                {
                    CustomPrincipalSerializeModel serializeModel = ((CustomPrincipalSerializeModel)Session["DocSessionData"]);
                    if (!PhrmsUserId.Equals(Guid.Empty))
                        serializeModel.PhrmsUserId = PhrmsUserId;
                    serializeModel.Name = model.FirstName + " " + model.LastName;
                    serializeModel.Phone = model.PhoneNumber;
                    serializeModel.Email = (model.EmailAddress == null ? "" : model.EmailAddress);
                    serializeModel.Gender = model.Gender;
                    serializeModel.StrDOB = model.strDOB;
                    serializeModel.DocPatientId = Status.DocPatientId;
                    Session["DocSessionData"] = serializeModel;
                    return Json(1);
                }

                else
                    return Json(0);
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Patient");

            }
            return Json(1);
        }
        public ActionResult PatientDetail()
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
            return PartialView("_PatientDetail", oSuperPatientDetails);
            //}
        }
    }


}