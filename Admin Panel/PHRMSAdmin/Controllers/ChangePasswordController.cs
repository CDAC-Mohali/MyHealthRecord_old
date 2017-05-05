using PHRMSAdmin.DALayer;
using PHRMSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHRMSAdmin.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult Index()
        {
            return View();
        }
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangePasswordModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PHRMSDBContext  db = new PHRMSDBContext();
                    CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                    var user = db.AdminUsers.FirstOrDefault(m => m.AdminUserId == oCustomPrincipalSerializeModel.AdminUserId && m.Password.Equals(model.OldPassword));
                    if (user != null)
                    {
                        user.Password = model.NewPassword;
                        db.SaveChanges();
                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomValidation", "Invalid Credentials. Please try again.");
                       // ViewBag.Error = "Old Password is not correct, Please Try Again";
                       // ModelState["OldPassword"].Value = new ValueProviderResult(string.Empty, string.Empty, ModelState["OldPassword"].Value.Culture);
                       // ModelState["NewPassword"].Value = new ValueProviderResult(string.Empty, string.Empty, ModelState["NewPassword"].Value.Culture);
                      //  ModelState["ConfirmPassword"].Value = new ValueProviderResult(string.Empty, string.Empty, ModelState["ConfirmPassword"].Value.Culture); 
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                //write exception
            }

            return View(model);
        }
    }
}