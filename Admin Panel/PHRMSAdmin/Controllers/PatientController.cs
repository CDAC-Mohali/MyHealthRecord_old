
using AutoMapper;
using PagedList;
using PHRMSAdmin.DALayer;
using PHRMSAdmin.Library;
using PHRMSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHRMSAdmin.Controllers
{
    public static class AppSetting
    {
        public static int PageSize = 10;
    }
    [AdminAuthorizationFilter]
    public class PatientController : Controller
    {
        
        public ActionResult Index(int page = 1, string Keyword = "")
        {
            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<PatientModel> oGetList = null;
            try
            {

                oGetList = GetList(page, oPHRMSDBContext, oGetList, Keyword);

                if (Request.IsAjaxRequest() == true)
                {
                    return PartialView("_PatientGrid", oGetList); //for searching in grid
                }
                else
                {
                    return View("Index", oGetList);
                }


            }
            catch (Exception ex)
            {
                //    Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oGetList);
        }
        private static IPagedList<PatientModel> GetList(int page, PHRMSDBContext oPHRMSDBContext, IPagedList<PatientModel> oGetList, string FirstName)
        {
            oGetList = Mapper.Map<List<Users>, List<PatientModel>>(oPHRMSDBContext.Users.Where(s => FirstName == "" ? true : (s.FirstName.ToLower().Trim().Contains(FirstName.ToLower().Trim()) || s.LastName.ToLower().Trim().Contains(FirstName.ToLower().Trim()))).ToList()).OrderBy(s => s.FirstName).ToList().ToPagedList(page, AppSetting.PageSize);

            if (page > 1 && oGetList.Count() == 0)
            {
                oGetList = Mapper.Map<List<Users>, List<PatientModel>>(oPHRMSDBContext.Users.Where(s => FirstName == "" ? true : (s.FirstName.ToLower().Trim().Contains(FirstName.ToLower().Trim()))).ToList()).OrderBy(s => s.FirstName).ToList().ToPagedList(page, AppSetting.PageSize-1);
            }

            return oGetList;
        }
        public ViewResult Details(Guid id)
        {
            try
            {
                PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
                PatientModel oPatientModel = AutoMapper.Mapper.Map<Users, PatientModel>(oPHRMSDBContext.Users.Find(id));
                return View(oPatientModel);
            }
            catch (Exception)
            {

              
            }

            return View(new PatientModel());
        }

        
    }
}