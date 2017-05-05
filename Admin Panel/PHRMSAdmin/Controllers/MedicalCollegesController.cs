
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
    [AdminAuthorizationFilter]
    public class MedicalCollegesController : Controller
    {
        public ActionResult Index(int page = 1, string Keyword = "")
        {
            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<MedicalCollegesViewModel> oTaskList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    // oTaskList = GetTaskList(page, oPHRMSDBContext, oTaskList, TaskName);
                    oTaskList = Mapper.Map<List<MedicalColleges>, List<MedicalCollegesViewModel>>(oPHRMSDBContext.MedicalColleges.Where(s => Keyword == "" ? true : (s.MedicalCollegeName.ToLower().Trim().Contains(Keyword.ToLower().Trim()))).ToList()).OrderBy(s => s.MedicalCollegeName).ToList().ToPagedList(page, AppSetting.PageSize);

                    if (page > 1 && oTaskList.Count == 0)
                    {
                        oTaskList = Mapper.Map<List<MedicalColleges>, List<MedicalCollegesViewModel>>(oPHRMSDBContext.MedicalColleges.Where(s => Keyword == "" ? true : (s.MedicalCollegeName.ToLower().Trim().Contains(Keyword.ToLower().Trim()))).ToList()).OrderBy(s => s.MedicalCollegeName).ToList().ToPagedList(page - 1, AppSetting.PageSize);
                    }
                    if (Request.IsAjaxRequest() == true)
                    {
                        return PartialView("_MedicalCollgesGrid", oTaskList); //for grid view
                    }
                    else
                    {
                        return View("Index", oTaskList);
                    }

                }
            }
            catch (Exception ex)
            {
                //Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oTaskList);
        }


        public ActionResult GetMedicalColleges(int Id = 0)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            MedicalCollegesViewModel oGet = new MedicalCollegesViewModel();
            try
            {

                oGet = Mapper.Map<MedicalCollegesViewModel>(oPHRMSDBContext.MedicalColleges.Where(s => s.MedicalCollegeId == Id).FirstOrDefault());

                if (oGet == null)
                    oGet = new MedicalCollegesViewModel();
            }
            catch (Exception ex)
            {
                //      Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oGet);
        }

        public ActionResult AddMedicalColleges()
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            MedicalCollegesViewModel oAdd = new MedicalCollegesViewModel();

            try
            {
                oAdd = new MedicalCollegesViewModel();
            }
            catch (Exception ex)
            {
                // Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oAdd);
        }

        [HttpPost]
        public ActionResult AddMedicalColleges(MedicalCollegesViewModel oMedicalCollegesViewModel)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {

                MedicalColleges oCreateTask = Mapper.Map<MedicalColleges>(oMedicalCollegesViewModel);

                oPHRMSDBContext.MedicalColleges.Add(oCreateTask);
                oPHRMSDBContext.SaveChanges();


            }
            catch (Exception ex)
            {
                //         Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return RedirectToAction("Index");
        }

        public ActionResult UpdateMedicalCollges(int Id = 0)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            MedicalCollegesViewModel oGet = new MedicalCollegesViewModel();

            try
            {

                oGet = Mapper.Map<MedicalCollegesViewModel>(oPHRMSDBContext.MedicalColleges.Where(s => s.MedicalCollegeId == Id).FirstOrDefault());
                if (oGet == null)
                    oGet = new MedicalCollegesViewModel();
            }
            catch (Exception ex)
            {
                //Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oGet);
        }

        [HttpPost]
        public ActionResult UpdateMedicalCollges(MedicalCollegesViewModel oUpdateTask)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {


                MedicalColleges oCreate = oPHRMSDBContext.MedicalColleges.Where(s => s.MedicalCollegeId == oUpdateTask.MedicalCollegeId).FirstOrDefault();
                if (oCreate != null)
                {
                    oCreate.MedicalCollegeName = oUpdateTask.MedicalCollegeName;
                    oCreate.State = oUpdateTask.State;

                    oPHRMSDBContext.SaveChanges();

                    oCreate = null;
                }

            }
            catch (Exception ex)
            {
                //            Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteMedicalCollges(int Id, int pageNumber)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                MedicalColleges oDelete = oPHRMSDBContext.MedicalColleges.Where(s => s.MedicalCollegeId == Id).FirstOrDefault();

                if (oDelete != null)
                {
                    oPHRMSDBContext.MedicalColleges.Remove(oDelete);
                    oPHRMSDBContext.SaveChanges();
                }
                oDelete = null;

                return RedirectToAction("Index", new { page = pageNumber });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    TempData["DeleteMessage"] = true;
                    return RedirectToAction("Index", new { page = pageNumber });
                }
                //   Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            return RedirectToAction("Index", new { page = pageNumber });
        }
    }
}