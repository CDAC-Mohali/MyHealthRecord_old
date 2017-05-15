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
    public class RoleController:Controller
    {
        #region Role
        public JsonResult doesRoleExist(string RoleName, long? RoleId)
        {
            bool Check = true;
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            if (RoleId == null)
                Check = oPHRMSDBContext.Role.Where(s => s.RoleName == RoleName).FirstOrDefault() != null ? false : true;
            else
                Check = oPHRMSDBContext.Role.Where(s => s.RoleName == RoleName && s.RoleId != RoleId).FirstOrDefault() != null ? false : true;
            return Json(Check, JsonRequestBehavior.AllowGet);
        }
        [AdminAuthorizationFilter]
        public ActionResult Index(int page = 1, string Keyword = "")
        {
            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<RoleModel> oGetList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    oGetList = GetRoleList(page, oPHRMSDBContext, oGetList, Keyword);

                    if (Request.IsAjaxRequest() == true)
                    {
                        return PartialView("_RoleGrid", oGetList); //for searching in grid
                    }
                    else
                    {
                        return View("Index", oGetList);
                    }

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
        private static IPagedList<RoleModel> GetRoleList(int page, PHRMSDBContext oPHRMSDBContext, IPagedList<RoleModel> oGetList, string RoleName)
        {

            oGetList = Mapper.Map<List<Role>, List<RoleModel>>(oPHRMSDBContext.Role.Where(m => RoleName == "" ? true : (m.RoleName.ToLower().Trim().Contains(RoleName.ToLower().Trim()))).ToList()).OrderBy(s => s.RoleName).ToList().ToPagedList(page, AppSetting.PageSize);

            if (page > 1 && oGetList.Count == 0)
            {
                oGetList = Mapper.Map<List<Role>, List<RoleModel>>(oPHRMSDBContext.Role.Where(m => RoleName == "" ? true : (m.RoleName.ToLower().Trim().Contains(RoleName.ToLower().Trim()))).ToList()).OrderBy(s => s.RoleName).ToList().ToPagedList(page - 1, AppSetting.PageSize);
            }

            return oGetList;
        }

        public ActionResult GetRole(long Id)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            List<RoleTaskMappingModel> oGet = new List<RoleTaskMappingModel>();
            try
            {
                //  var ss = oPHRMSDBContext.RoleTaskMapping.Where(s => s.RoleId == Id).ToList();
                oGet = Mapper.Map<List<RoleTaskMapping>, List<RoleTaskMappingModel>>(oPHRMSDBContext.RoleTaskMapping.Where(s => s.RoleId == Id).ToList());
                if (oGet == null || oGet.Count == 0)
                {
                    oGet = new List<RoleTaskMappingModel>();
                    RoleTaskMappingModel oRoleTaskMappingModel = new RoleTaskMappingModel();
                    oRoleTaskMappingModel.RoleId = Id;
                    oRoleTaskMappingModel.RoleModel = new RoleModel();
                    oRoleTaskMappingModel.RoleModel.RoleId = Id;
                    oRoleTaskMappingModel.RoleModel.RoleName = oPHRMSDBContext.Role.Where(s => s.RoleId == Id).FirstOrDefault().RoleName;
                    oGet.Add(oRoleTaskMappingModel);
                }

            }
            catch (Exception ex)
            {
                //   Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oGet);
        }
        public ActionResult AddRole()
        {
            RoleModel oGet = new RoleModel();
            ViewBag.Task = CommonModel.GetTasks();
            return View(oGet);
        }

        [HttpPost]
        public ActionResult AddRole(RoleModel oRoleModel, int[] Task)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                if (ModelState.IsValid || oRoleModel.RoleName != null)
                {
                    Role oCreateRole = Mapper.Map<Role>(oRoleModel);

                    oPHRMSDBContext.Role.Add(oCreateRole);
                    oPHRMSDBContext.SaveChanges();



                    #region Assign Task To Role
                    if (Task != null)
                        foreach (var TaskId in Task)
                        {
                            RoleTaskMapping oRoleTask = new RoleTaskMapping();
                            oRoleTask.RoleId = oCreateRole.RoleId;
                            oRoleTask.TaskId = TaskId;
                            oPHRMSDBContext.RoleTaskMapping.Add(oRoleTask);
                            oPHRMSDBContext.SaveChanges();
                        }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                //    Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return RedirectToAction("Index");
        }
        public ActionResult UpdateRole(long Id)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            List<RoleTaskMappingModel> oGet = new List<RoleTaskMappingModel>();
            try
            {
                //  var ss = oPHRMSDBContext.RoleTaskMapping.Where(s => s.RoleId == Id).ToList();
                oGet = Mapper.Map<List<RoleTaskMapping>, List<RoleTaskMappingModel>>(oPHRMSDBContext.RoleTaskMapping.Where(s => s.RoleId == Id).ToList());
                if (oGet == null || oGet.Count == 0)
                {
                    oGet = new List<RoleTaskMappingModel>();
                    RoleTaskMappingModel oRoleTaskMappingModel = new RoleTaskMappingModel();
                    oRoleTaskMappingModel.RoleId = Id;
                    oRoleTaskMappingModel.RoleModel = new RoleModel();
                    oRoleTaskMappingModel.RoleModel.RoleId = Id;
                    oRoleTaskMappingModel.RoleModel.RoleName = oPHRMSDBContext.Role.Where(s => s.RoleId == Id).FirstOrDefault().RoleName;
                    oGet.Add(oRoleTaskMappingModel);
                }

                ViewBag.Task = CommonModel.GetTasks();

            }
            catch (Exception ex)
            {
                // Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oGet);
        }
        [HttpPost]
        public ActionResult UpdateRole(RoleModel oRoleModel, int[] Task)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {

                Role oRole = oPHRMSDBContext.Role.Where(s => s.RoleId == oRoleModel.RoleId).FirstOrDefault();
                if (oRole.RoleId != null)
                {
                    //Roles.DeleteRole(oRoleModel.RoleName);
                    //   Roles.CreateRole(oRoleModel.RoleName);
                    #region Remove All Task Under Roles
                    List<RoleTaskMapping> oRoleTaskList = oPHRMSDBContext.RoleTaskMapping.Where(s => s.RoleId == oRoleModel.RoleId).ToList();
                    foreach (var Item in oRoleTaskList)
                    {
                        oPHRMSDBContext.RoleTaskMapping.Remove(Item);
                        oPHRMSDBContext.SaveChanges();
                    }
                    oRoleTaskList = null;
                    #endregion

                    #region Assign Task To Role
                    if (Task != null)
                        foreach (var TaskId in Task)
                        {
                            RoleTaskMapping oRoleTask = new RoleTaskMapping();
                            oRoleTask.RoleId = oRoleModel.RoleId;
                            oRoleTask.TaskId = TaskId;
                            oPHRMSDBContext.RoleTaskMapping.Add(oRoleTask);
                            oPHRMSDBContext.SaveChanges();
                        }

                    #endregion




                    //    ViewBag.Task = GetTaskByRoleId(oPHRMSDBContext, oRole.RoleId);
                }
            }
            catch (Exception ex)
            {
                //    Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;

            return RedirectToAction("Index");
        }


        private static List<TaskModel> GetTaskByRoleId(PHRMSDBContext oPHRMSDBContext, long RoleId)
        {
            List<TaskModel> Result = (from m in oPHRMSDBContext.Tasks.Where(s => s.IsActive == true)
                                      select new TaskModel
                                      {
                                          TaskId = m.TaskId,
                                          TaskName = m.TaskName,
                                          //   Ischecked = oPHRMSDBContext.RoleTaskMapping.Where(s => s.TaskId == m.ID && s.RoleId == RoleId).FirstOrDefault() != null ? true : false
                                      }).ToList();
            return Result;
        }

        [HttpPost]
        public ActionResult DeleteRole(long Id, int pageNumber)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                Role oDelete = oPHRMSDBContext.Role.Where(s => s.RoleId == Id).FirstOrDefault();

                var Delete = oPHRMSDBContext.RoleTaskMapping.Where(s => s.RoleId == oDelete.RoleId).ToList();
                foreach (var Item in Delete)
                {

                    oPHRMSDBContext.RoleTaskMapping.Remove(Item);
                }

                if (oDelete != null)
                {
                    oPHRMSDBContext.Role.Remove(oDelete);
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
                //  Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            return null;

        }
        #endregion
    }
}