using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PagedList;
using PHRMSAdmin.DALayer;
using PHRMSAdmin.Models;
using System.Web.Mvc;
using PHRMSAdmin.Library;

namespace PHRMSAdmin.Controllers
{
     [AdminAuthorizationFilter]
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index(int page = 1)
        {
            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<NotificationModel> oGetList = null;
            try
            {
                if (ModelState.IsValid)
                {
                    oGetList = GetNotificationList(page, oPHRMSDBContext, oGetList);
                    List<NotificationModel> lst = oGetList.ToList();
                   foreach( var item in lst)
                    {
                        item.strCreatedDate = GetDateCustomTimeString(item.CreatedDate);
                        if(item.UserTypeId ==1)
                        {
                            Doctor doc = oPHRMSDBContext.Doctor.Where(s => s.docid == item.ReferenceId).FirstOrDefault();
                            item.userName = doc.name;
                        }
                        else if (item.UserTypeId == 2)
                        {
                            Users user = oPHRMSDBContext.Users.Where(s => s.UserId == item.ReferenceId).FirstOrDefault();
                            item.userName = user.FirstName +" "+ user.LastName;
                        }
                    }
                    if (Request.IsAjaxRequest() == true)
                    {
                        return PartialView("_NotificationGrid", oGetList); 
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
        private static IPagedList<NotificationModel> GetNotificationList(int page, PHRMSDBContext oPHRMSDBContext, IPagedList<NotificationModel> oGetList)
        {
            Mapper.CreateMap<Notification, NotificationModel>();
            oGetList = Mapper.Map<List<Notification>, List<NotificationModel>>(oPHRMSDBContext.Notification.ToList()).ToPagedList(page, AppSetting.PageSize);
            if (page > 1 && oGetList.Count() == 0)
            {
                oGetList = Mapper.Map<List<Notification>, List<NotificationModel>>(oPHRMSDBContext.Notification.ToList()).ToPagedList(page, AppSetting.PageSize-1);
            }
            return oGetList;
        }
        public ViewResult Details(long id)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            NotificationModel oNotificationModel = null;
            try
            {
                 oNotificationModel = AutoMapper.Mapper.Map<Notification, NotificationModel>(oPHRMSDBContext.Notification.Find(id));
                oNotificationModel.strCreatedDate = GetDateCustomTimeString(oNotificationModel.CreatedDate);
                if(oNotificationModel.UserTypeId == 1)
                {
                    Doctor doc = oPHRMSDBContext.Doctor.Where(s => s.docid == oNotificationModel.ReferenceId).FirstOrDefault();
                    if(doc!=null)
                    {
                        oNotificationModel.userName = doc.name;
                    }
                    
                }
                else if (oNotificationModel.UserTypeId == 2)
                {
                    Users user = oPHRMSDBContext.Users.Where(s => s.UserId == oNotificationModel.ReferenceId).FirstOrDefault();
                    if (user!=null)
                    {
                        oNotificationModel.userName = user.FirstName + " "+ user.LastName;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return View(oNotificationModel);
        }
        public ActionResult AddNotification()
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            NotificationModel oNotificationModel = new NotificationModel();
            return View(oNotificationModel);
        }

        [HttpPost]
        public ActionResult AddNotification(NotificationModel oNotificationModel)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                Mapper.CreateMap<NotificationModel, Notification>();
                Notification oCreateTask = Mapper.Map<Notification>(oNotificationModel);
                if (oCreateTask.UserTypeId == 1)
                {
                    oCreateTask.MessageTypeId = 1;
                    oCreateTask.Status = 3;
                    oCreateTask.CreatedDate = DateTime.Now;
                    var list = oPHRMSDBContext.Doctor.ToList();
                    foreach (Doctor doc in list)
                    {
                        oCreateTask.ReferenceId = doc.docid;
                        oPHRMSDBContext.Notification.Add(oCreateTask);
                        oPHRMSDBContext.SaveChanges();
                    }
                }
                else //for patients
                {
                    oCreateTask.MessageTypeId = 1;
                    oCreateTask.Status = 3;
                    oCreateTask.CreatedDate = DateTime.Now;
                    var list = oPHRMSDBContext.Users.ToList();
                    foreach (Users user in list)
                    {
                        oCreateTask.ReferenceId = user.UserId;
                        oPHRMSDBContext.Notification.Add(oCreateTask);
                        oPHRMSDBContext.SaveChanges();
                    }
                }
                
            }
            catch (Exception ex)
            {
                //         Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "AppSetting");

            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return RedirectToAction("Index");
        }

        //To get Date out of DateTime for view purpose
        public string GetDateCustomTimeString(DateTime oDate)
        {
            try
            {
                return (oDate != null && oDate != DateTime.MinValue) ? oDate.ToString("dd/MM/yyyy") : "-";
            }
            catch (Exception)
            {

                return "";
            }
        }

    }
}