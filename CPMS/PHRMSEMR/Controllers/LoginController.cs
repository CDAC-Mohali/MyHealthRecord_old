using EMRLib.DAL;
using EMRViewModels;
using PHRMSEMR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static PHRMSEMR.FilterConfig;

namespace PHRMSEMR.Controllers
{
    [Error]
    public class LoginController : Controller
    {
        // GET: Login
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            try
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            catch(Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Login");

            }
            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            try
            { 
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            EMRDBContext db = new EMRDBContext();
           var docList = db.Doctor.Where(x => x.email == model.UserName || x.phone_number == model.UserName && x.password == model.Password).FirstOrDefault();
            if (docList != null)
            {
                FormsAuthentication.SetAuthCookie(docList.name, true);
                CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                serializeModel.DocId = docList.docid;
                serializeModel.Name = docList.name;
                serializeModel.Email = docList.email;
                Session["DocSessionData"] = serializeModel;
                if (Url.IsLocalUrl(returnUrl))
                {

                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                db = null;
                ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                return View();
            }
          }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Login");

            }
            return View();
        }

    }
}