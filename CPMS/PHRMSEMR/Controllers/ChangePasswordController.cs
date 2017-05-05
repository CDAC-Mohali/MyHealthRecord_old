using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMRLib.DAL;
using EMRViewModels;
using static PHRMSEMR.FilterConfig;
using PHRMSEMR.Models;

namespace PHRMSEMR.Controllers
{
    [AuthorizationFilter]
    public class ChangePasswordController : Controller
    {
        IEMRRepository _repo;
        public ChangePasswordController()
        {
            _repo = new EMRRepository();
        }

        // GET: ChangePassword
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "ChangePassword");
                //write exception
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangePasswordModel model)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    // EMRDBContext db = new EMRDBContext();
                    CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["DocSessionData"]);

                    if (_repo.ChangePassword(oCustomPrincipalSerializeModel.DocId, model.NewPassword, model.OldPassword))
                    {

                        return RedirectToAction("Index", "Appointments");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "ChangePassword");
                //write exception
            }
            return View(model);
        }
    }
}