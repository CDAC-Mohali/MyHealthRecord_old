using AutoMapper;
using PHRMSAdmin.Library;
using PHRMSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PHRMSAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Mapper.CreateMap<Users, PatientModel>();
            Mapper.CreateMap<PatientModel, Users>();

            #region AdminUsers Tasks Roles
            Mapper.CreateMap<AdminUsers, AdminUsersModel>();
            //          Mapper.CreateMap<List<Users>, List<UsersModel>>();
            Mapper.CreateMap<AdminUsersModel, AdminUsers>();
            //        Mapper.CreateMap<List<UsersModel>, List<Users>>();
            Mapper.CreateMap<Tasks, TaskModel>();
            Mapper.CreateMap<TaskModel, Tasks>();
            Mapper.CreateMap<Role, RoleModel>();
            Mapper.CreateMap<RoleModel, Role>();
            Mapper.CreateMap<RoleTaskMapping, RoleTaskMappingModel>().ForMember(s => s.RoleModel, c => c.MapFrom(m => m.Role)).ForMember(s => s.TasksModel, c => c.MapFrom(m => m.Task));
            Mapper.CreateMap<RoleTaskMappingModel, RoleTaskMapping>().ForMember(s => s.Role, c => c.MapFrom(m => m.RoleModel)).ForMember(s => s.Task, c => c.MapFrom(m => m.TasksModel));
            #endregion
            Mapper.CreateMap<DocPatientDetails, DocPatientDetailsViewModel>();
            Mapper.CreateMap<DocPatientDetailsViewModel, DocPatientDetails>();
            Mapper.CreateMap<DoctorModel, Doctor>();
            Mapper.CreateMap<Doctor, DoctorModel>();
            Mapper.CreateMap<MedicalCollegesViewModel, MedicalColleges>();
            Mapper.CreateMap<MedicalColleges, MedicalCollegesViewModel>();
            Application["Totaluser"] = 0;

        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

        }

        protected void Session_Start()
        {
            Application.Lock();
            Application["Totaluser"] = (int)Application["Totaluser"] + 1;
            Application.UnLock();
        }
    }
}
