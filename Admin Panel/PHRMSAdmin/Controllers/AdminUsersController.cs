
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
    public class AdminUsersController : Controller
    {
        #region AdminUsers

        public ActionResult Index(int page = 1, string Keyword = "")
        {
            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<AdminUsersModel> oTaskList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    // oTaskList = GetTaskList(page, oPHRMSDBContext, oTaskList, TaskName);
                    oTaskList = Mapper.Map<List<AdminUsers>, List<AdminUsersModel>>(oPHRMSDBContext.AdminUsers.Where(s => Keyword == "" ? true : (s.FirstName.ToLower().Trim().Contains(Keyword.ToLower().Trim()) || s.LastName.ToLower().Trim().Contains(Keyword.ToLower().Trim()))).ToList()).OrderBy(s => s.UserName).ToList().ToPagedList(page, AppSetting.PageSize);

                    if (page > 1 && oTaskList.Count == 0)
                    {
                        oTaskList = Mapper.Map<List<AdminUsers>, List<AdminUsersModel>>(oPHRMSDBContext.AdminUsers.Where(s => Keyword == "" ? true : (s.FirstName.ToLower().Trim().Contains(Keyword.ToLower().Trim()))).ToList()).OrderBy(s => s.UserName).ToList().ToPagedList(page - 1, AppSetting.PageSize);

                    }
                    if (Request.IsAjaxRequest() == true)
                    {
                        return PartialView("_AdminUserGrid", oTaskList); //for grid view
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


        public ActionResult GetAdminUser(int Id = 0)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            AdminUsersModel oGet = new AdminUsersModel();
            try
            {

                oGet = Mapper.Map<AdminUsersModel>(oPHRMSDBContext.AdminUsers.Where(s => s.AdminUserId == Id).FirstOrDefault());

                if (oGet == null)
                    oGet = new AdminUsersModel();
            }
            catch (Exception ex)
            {
                //      Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oGet);
        }

        public ActionResult AddAdminUser()
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            AdminUsersModel oAdd = new AdminUsersModel();
            #region
            List<SelectListItem> oRoleList = new List<SelectListItem>();
            oPHRMSDBContext.Role.ToList().ForEach(s => oRoleList.Add(new SelectListItem() { Text = s.RoleName, Value = s.RoleId.ToString() }));
            ViewBag.RoleList = oRoleList;
            List<SelectListItem> oHospitalList = new List<SelectListItem>();
            oPHRMSDBContext.MedicalColleges.ToList().ForEach(s => oHospitalList.Add(new SelectListItem() { Text = s.MedicalCollegeName, Value = s.MedicalCollegeId.ToString() }));
            ViewBag.HospitalList = oHospitalList;
            #endregion
            try
            {
                oAdd = new AdminUsersModel();
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
        public ActionResult AddAdminUser(AdminUsersModel oAdminUsersModel)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {

                AdminUsers oCreateTask = Mapper.Map<AdminUsers>(oAdminUsersModel);
                oCreateTask.DateCreated = DateTime.Now;
                oCreateTask.IsSuperAdmin = false;
                oCreateTask.PhoneNumber = oAdminUsersModel.PhoneNumber;
                oCreateTask.MedicalCollegeId = oAdminUsersModel.MedicalCollegeId;
                oPHRMSDBContext.AdminUsers.Add(oCreateTask);
                oPHRMSDBContext.SaveChanges();
                //AppSetting.SetAllSettingProperties();// SetAllSettingProperties
                oAdminUsersModel.AdminUserId = oCreateTask.AdminUserId;
                oCreateTask = null;
                UserRoleMapping oUserRoleMapping = new UserRoleMapping();
                oUserRoleMapping.RoleId = oAdminUsersModel.RoleId;
                oUserRoleMapping.AdminUserId = oAdminUsersModel.AdminUserId;
                oPHRMSDBContext.UserRoleMapping.Add(oUserRoleMapping);
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

        public ActionResult UpdateAdminUser(int Id = 0)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            AdminUsersModel oGet = new AdminUsersModel();
            #region
            List<SelectListItem> oRoleList = new List<SelectListItem>();
            oPHRMSDBContext.Role.ToList().ForEach(s => oRoleList.Add(new SelectListItem() { Text = s.RoleName, Value = s.RoleId.ToString() }));
            ViewBag.RoleList = oRoleList;
            List<SelectListItem> oHospitalList = new List<SelectListItem>();
            oPHRMSDBContext.MedicalColleges.ToList().ForEach(s => oHospitalList.Add(new SelectListItem() { Text = s.MedicalCollegeName, Value = s.MedicalCollegeId.ToString() }));
            ViewBag.HospitalList = oHospitalList;
            #endregion
            try
            {

                oGet = Mapper.Map<AdminUsersModel>(oPHRMSDBContext.AdminUsers.Where(s => s.AdminUserId == Id).FirstOrDefault());
                oGet.RoleId = oPHRMSDBContext.UserRoleMapping.Where(s => s.AdminUserId == Id).FirstOrDefault().RoleId;
                if (oGet == null)
                    oGet = new AdminUsersModel();
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
        public ActionResult UpdateAdminUser(AdminUsersModel oUpdateTask)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {


                AdminUsers oCreate = oPHRMSDBContext.AdminUsers.Where(s => s.AdminUserId == oUpdateTask.AdminUserId).FirstOrDefault();
                if (oCreate != null)
                {
                    oCreate.UserName = oUpdateTask.UserName;
                    oCreate.Active = oUpdateTask.Active;
                    oCreate.MedicalCollegeId = oUpdateTask.MedicalCollegeId;
                    oCreate.FirstName = oUpdateTask.FirstName;
                    oCreate.LastName = oUpdateTask.LastName;
                    oCreate.EmailAddress = oUpdateTask.EmailAddress;
                    oCreate.PhoneNumber = oUpdateTask.PhoneNumber;
                    oPHRMSDBContext.SaveChanges();
                    UserRoleMapping oUserRoleMapping = oPHRMSDBContext.UserRoleMapping.Where(s => s.AdminUserId == oCreate.AdminUserId).FirstOrDefault();
                    if (oUserRoleMapping == null)
                        oUserRoleMapping = new UserRoleMapping();
                    oUserRoleMapping.RoleId = oUpdateTask.RoleId;
                    oUserRoleMapping.AdminUserId = oCreate.AdminUserId;
                    if (oUserRoleMapping.UserRoleId == 0)
                        oPHRMSDBContext.UserRoleMapping.Add(oUserRoleMapping);
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
        public ActionResult DeleteAdminUser(int Id, int pageNumber)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                AdminUsers oDelete = oPHRMSDBContext.AdminUsers.Where(s => s.AdminUserId == Id).FirstOrDefault();
                UserRoleMapping oUserRole = oPHRMSDBContext.UserRoleMapping.Where(s => s.AdminUserId == Id).FirstOrDefault();
                if (oUserRole != null)
                {
                    oPHRMSDBContext.UserRoleMapping.Remove(oUserRole);
                    oPHRMSDBContext.SaveChanges();
                }
                if (oDelete != null)
                {
                    oPHRMSDBContext.AdminUsers.Remove(oDelete);
                    oPHRMSDBContext.SaveChanges();
                }
                oDelete = null;
                oUserRole = null;
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
        #endregion
    }
}