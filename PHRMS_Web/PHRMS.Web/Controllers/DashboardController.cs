using System;
using Microsoft.AspNet.Mvc;
using PHRMS.ViewModels;
using PHRMS.BLL;
using Microsoft.AspNet.Http;
using System.Collections.Generic;
using Microsoft.AspNet.Hosting;
namespace PHRMS.Web.Controllers
{
    public class DashboardController : Controller
    {
        private CatalogService oDashboardComponent;
        private readonly IHostingEnvironment _appHostingEnvironment;
        public DashboardController(CatalogService objDashboardComponent)
        {
            oDashboardComponent = objDashboardComponent;
        }
        // GET: /<controller>/
        public IActionResult Dashboard()
        {
            try
            {

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                
            }

            return View();
        }
        [HttpGet]
        public DashboardAnalyticsViewModel UpdateAnalytics()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return oDashboardComponent.UpdateAnalytics(userId);

            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<BPViewModel> GetBPandPulseData()
        {
            try
            {

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return oDashboardComponent.GetBPandPulseData(userId);
            }
              
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
    }
}
        [HttpGet]
        public List<string[]> GetWeightData()
        {
            try
            {

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return oDashboardComponent.GetWeightData(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }

        }
        [HttpGet]
        public List<string[]> GetGlucoseData()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return oDashboardComponent.GetGlucoseData(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<string[]> GetActivityData()
        {
            try { 
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return oDashboardComponent.GetActivityData(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<string[]> GetLatestAllergies()
        {
            try
            {
                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return oDashboardComponent.GetLatestAllergies(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<string[]> GetLatestProcedures()
        {
            try
            {

                Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
                return oDashboardComponent.GetLatestProcedures(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<string[]> GetLatestMedications()
        {
            try { 
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return oDashboardComponent.GetLatestMedications(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<string[]> GetLatestLabs()
        {
            try { 
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return oDashboardComponent.GetLatestLabs(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }
        [HttpGet]
        public List<string[]> GetLatestImmunizations()
        {
            try { 

            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return oDashboardComponent.GetLatestImmunizations(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return null;
            }
        }

        [HttpGet]
        public string GetLatestActivities()
        {
            try { 
            Guid userId = Guid.Parse(HttpContext.Session.GetString("UserId"));
            return oDashboardComponent.GetLatestActivities(userId);
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return "";
            }
        }

        [HttpGet]
        public string GetHealthTip()
        {
            try { 
            return oDashboardComponent.GetHealthTip();
            }
            catch (Exception ex)
            {
                PHRMS.Web.Models.Common.CreateLog(_appHostingEnvironment, PHRMS.Web.Models.Common.ExecptionMessage(ex), PHRMS.Web.Models.MessageType.Error, "Dashboard");
                return "";
            }
        }

    }

    public class FeedBackParams
    {
        public string strFeedback { get; set; }
        public string strSubject { get; set; }
        public List<string> lstAttachment { get; set; }
        public string PrimaryPhone { get; set; }
        public string EmailAddress { get; set; }

    }
}
