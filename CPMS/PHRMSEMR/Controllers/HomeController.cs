using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PHRMSEMR.Models;
using static PHRMSEMR.FilterConfig;

namespace PHRMSEMR.Controllers
{
    [Error]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Error()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Home");

            }
            return View();
        }
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Home");

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}