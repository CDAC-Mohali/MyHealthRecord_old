
using AutoMapper;
using PagedList;
using PHRMSAdmin.DALayer;
using PHRMSAdmin.Library;
using PHRMSAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace PHRMSAdmin.Controllers
{
    [AdminAuthorizationFilter]
    public class PatientDoctorController : Controller
    {
        public ActionResult Index(int page = 1, string Keyword = "")
        {

            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<DocPatientDetailsViewModel> oGetList = null;
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                oGetList = GetList(page, oPHRMSDBContext, oGetList, Keyword, oCustomPrincipalSerializeModel.MedicalCollegeId.Value);

                if (Request.IsAjaxRequest() == true)
                {
                    return PartialView("_PatientDoctorGrid", oGetList); //for searching in grid
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
        private static IPagedList<DocPatientDetailsViewModel> GetList(int page, PHRMSDBContext oPHRMSDBContext, IPagedList<DocPatientDetailsViewModel> oGetList, string FirstName, long MedicalCollegeId)
        {
            var res = (from m in oPHRMSDBContext.DocPatientDetails    // your starting point - table in the "from" statement
                       join q in oPHRMSDBContext.Doctor
                       on m.DocId equals q.docid
                       where q.MedicalCollegeId == MedicalCollegeId && ((m.FirstName == "" ? (true) : (m.FirstName.Contains(FirstName))))
                       select new DocPatientDetailsViewModel
                       {
                           DocPatientId=m.DocPatientId,
                           FirstName = m.FirstName,
                           LastName = m.LastName,
                           DOB = m.DOB.Value,
                           Gender = m.Gender,
                           strState = oPHRMSDBContext.States.Where(s => s.Id == m.State).FirstOrDefault().Name
                       }).ToList();

            oGetList = (res.OrderBy(s => s.FirstName)).ToList().ToPagedList(page, AppSetting.PageSize);




            return oGetList;
        }
        public ViewResult Details(long id)
        {
            try
            {

                PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
                var res = (from m in oPHRMSDBContext.DocPatientDetails    // your starting point - table in the "from" statement


                           where m.DocPatientId == id
                           select new DocPatientDetailsViewModel
                           {
                               FirstName = m.FirstName,
                               LastName = m.LastName,
                               DOB = m.DOB.Value,
                               Gender = m.Gender,
                               PhoneNumber = m.PhoneNumber,
                               EmailAddress = m.EmailAddress,
                               Address1 = m.Address1,
                               Address2 = m.Address2,
                               City_Vill_Town = m.City_Vill_Town,
                               District = m.District,
                               AadhaarNumber = m.AadhaarNumber,
                               strState = oPHRMSDBContext.States.Where(s => s.Id == m.State).FirstOrDefault().Name
                           }).FirstOrDefault();
                return View(res);
            }
            catch (Exception ex)
            {


            }

            return View(new DoctorModel());
        }
    }
}