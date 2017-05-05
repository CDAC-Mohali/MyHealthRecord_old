
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
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
        public void Export()
        {
            CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            List<DocPatientDetailsViewModel> list = GetList(oPHRMSDBContext, "", oCustomPrincipalSerializeModel.MedicalCollegeId.Value);
            if (list.Count > 0)
            {
                //List<DataType> list = new List<DataType>();
                //list.Add(new DataType { Date = "s", ID = "sss", Description = "sssss" });
                //list.Add(new DataType { Date = "s", ID = "sss", Description = "sssss" });
                //list.Add(new DataType { Date = "s", ID = "sss", Description = "sssss" });
                //list.Add(new DataType { Date = "s", ID = "sss", Description = "sssss" });
                StringWriter sw = new StringWriter();

                //First line for column names
                sw.WriteLine("\"First Name\",\"Last Name\",\"DOB\",\"Gender\",\"State\"");

                foreach (DocPatientDetailsViewModel item in list)
                {
                    sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"",
                                               item.FirstName,
                                               item.LastName,
                                               item.DOB,
                                               item.Gender, item.strState));
                }

                Response.AddHeader("Content-Disposition", "attachment; filename=Report.csv");
                Response.ContentType = "text/csv";
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
                Response.Write(sw);
                Response.End();
            }


        }
        private List<DocPatientDetailsViewModel> GetList(PHRMSDBContext oPHRMSDBContext, string FirstName, long MedicalCollegeId)
        {
            List<DocPatientDetailsViewModel> res = (from m in oPHRMSDBContext.DocPatientDetails    // your starting point - table in the "from" statement
                                                    join q in oPHRMSDBContext.Doctor
                                                    on m.DocId equals q.docid
                                                    where q.MedicalCollegeId == MedicalCollegeId && ((m.FirstName == "" ? (true) : (m.FirstName.Contains(FirstName))))
                                                    select new DocPatientDetailsViewModel
                                                    {
                                                        DocPatientId = m.DocPatientId,
                                                        FirstName = m.FirstName,
                                                        LastName = m.LastName,
                                                        DOB = m.DOB.Value,
                                                        Gender = m.Gender,
                                                        strState = oPHRMSDBContext.States.Where(s => s.Id == m.State).FirstOrDefault().Name,

                                                    }).ToList();

            //  List<DocPatientDetailsViewModel> oGetList = (res.OrderBy(s => s.FirstName)).ToList().ToPagedList(page, AppSetting.PageSize);




            return res;
        }
    }
}