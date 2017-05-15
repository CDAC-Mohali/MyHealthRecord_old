using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PHRMSEMR.FilterConfig;
using PHRMSEMR.Models;

namespace PHRMSEMR.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Register");
            }
            return View();
        }
    }
}