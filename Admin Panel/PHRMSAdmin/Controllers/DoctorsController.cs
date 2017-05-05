
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
    public class DataType
    {
        public object Date { get; internal set; }
        public object Description { get; internal set; }
        public object ID { get; internal set; }
    }
    [AdminAuthorizationFilter]
    public class DoctorsController : Controller
    {
      


        HttpWebRequest request;
        Stream dataStream;
        public ActionResult Index(int? Status, int page = 1, string Keyword = "")
        {
            if (Status == null)
            {
                if (Session["Status"] == null)
                    Status = 2;
                else
                    Status = int.Parse(Session["Status"].ToString());
            }
            else
            {
                Session["Status"] = Status;
            }
            ViewBag.DeleteMessage = TempData["DeleteMessage"];
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            IPagedList<DoctorModel> oGetList = null;
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                oGetList = GetList(page, oPHRMSDBContext, oGetList, Keyword, Status.Value, oCustomPrincipalSerializeModel.MedicalCollegeId.Value, oCustomPrincipalSerializeModel.IsSuperAdmin);

                if (Request.IsAjaxRequest() == true)
                {

                    return PartialView("_DoctorGrid", oGetList); //for searching in grid
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
        private static IPagedList<DoctorModel> GetList(int page, PHRMSDBContext oPHRMSDBContext, IPagedList<DoctorModel> oGetList, string FirstName, int Status, long MedicalCollegeId, bool IsSuperAdmin)
        {
            if (IsSuperAdmin)
            {
                oGetList = Mapper.Map<List<Doctor>, List<DoctorModel>>(oPHRMSDBContext.Doctor.Where(s => (s.IsApproved == Status && (FirstName == "" ? (true) : (s.name.Contains(FirstName))))).ToList()).OrderBy(s => s.name).ToList().ToPagedList(page, AppSetting.PageSize);

                if (page > 1 && oGetList.Count() == 0)
                {
                    oGetList = Mapper.Map<List<Doctor>, List<DoctorModel>>(oPHRMSDBContext.Doctor.Where(s => (s.IsApproved == Status && (FirstName == "" ? (true) : (s.name.Contains(FirstName))))).ToList()).OrderBy(s => s.name).ToList().ToPagedList(page, AppSetting.PageSize - 1);

                }
            }
            else
            {

                oGetList = Mapper.Map<List<Doctor>, List<DoctorModel>>(oPHRMSDBContext.Doctor.Where(s => (s.IsApproved == Status && s.MedicalCollegeId == MedicalCollegeId && (FirstName == "" ? (true) : (s.name.Contains(FirstName))))).ToList()).OrderBy(s => s.name).ToList().ToPagedList(page, AppSetting.PageSize);

                if (page > 1 && oGetList.Count() == 0)
                {
                    oGetList = Mapper.Map<List<Doctor>, List<DoctorModel>>(oPHRMSDBContext.Doctor.Where(s => (s.IsApproved == Status && s.MedicalCollegeId == MedicalCollegeId && (FirstName == "" ? (true) : (s.name.Contains(FirstName))))).ToList()).OrderBy(s => s.name).ToList().ToPagedList(page, AppSetting.PageSize - 1);

                }
            }

            return oGetList;
        }
        public ActionResult UpdateHospital(Guid docid, int IsApproved, long MedicalCollegeId, string Remarks)
        {
            try
            {

                PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();

                Doctor oDoctorModel = oPHRMSDBContext.Doctor.Find(docid);
                oDoctorModel.MedicalCollegeId = MedicalCollegeId;
                if (IsApproved != 1)
                    oDoctorModel.ApprovedDate = DateTime.Now;
                oDoctorModel.IsApproved = IsApproved;

                oPHRMSDBContext.SaveChanges();
                updateDoctorApprovalTrack(oDoctorModel, docid, IsApproved, Remarks);
                return RedirectToAction("Details", new { id = docid });
            }
            catch (Exception)
            {


            }

            return View(new DoctorModel());
        }
        public ViewResult Details(Guid id)
        {
            try
            {

                PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
                List<SelectListItem> oHospitalList = new List<SelectListItem>();
                oPHRMSDBContext.MedicalColleges.OrderBy(s => s.MedicalCollegeName).ToList().ForEach(s => oHospitalList.Add(new SelectListItem() { Text = s.MedicalCollegeName, Value = s.MedicalCollegeId.ToString() }));
                oHospitalList.Insert(0, new SelectListItem() { Text = "-select-", Value = "" });
                ViewBag.HospitalList = oHospitalList;

                List<SelectListItem> oStatusList = new List<SelectListItem>();
                //   oPHRMSDBContext.MedicalColleges.ToList().ForEach(s => oStatusList.Add(new SelectListItem() { Text = s.MedicalCollegeName, Value = s.MedicalCollegeId.ToString() }));
                oStatusList.Add(new SelectListItem() { Text = "Pending", Value = "1" });
                oStatusList.Add(new SelectListItem() { Text = "Approved", Value = "2" });
                oStatusList.Add(new SelectListItem() { Text = "Rejected", Value = "3" });

                ViewBag.StatusList = oStatusList;
                var oDoctorModel = (from m in oPHRMSDBContext.Doctor.Where(s => s.docid == id)
                                    select new DoctorModel
                                    {
                                        DocFile = oPHRMSDBContext.EMRFiles.Where(s => s.RecId == m.docid).Select(x => x.FileName).FirstOrDefault(),
                                        AadhaarNo = m.AadhaarNo,
                                        ApprovedDate = m.ApprovedDate,
                                        date_of_birth = m.date_of_birth,
                                        delete_flag = m.delete_flag,
                                        docid = m.docid,
                                        email = m.email,
                                        email_flag = m.email_flag,
                                        Gender = m.Gender,
                                        HospitalName = m.HospitalName,
                                        IsApproved = m.IsApproved,
                                        LastName = m.LastName,
                                        MedicalCollegeId = m.MedicalCollegeId,
                                        name = m.name,
                                        OtherSpeciality = m.OtherSpeciality,
                                        phone_number = m.phone_number,
                                        qualification_set = m.qualification_set,
                                        request_time = m.request_time,
                                        Speciality = m.Speciality,
                                        ClinicName = m.Places_of_Practice.FirstOrDefault().name,
                                        AddressLine1 = m.Places_of_Practice.FirstOrDefault().AddressLine1,
                                        AddressLine2 = m.Places_of_Practice.FirstOrDefault().AddressLine2,
                                        City = m.Places_of_Practice.FirstOrDefault().city,
                                        State = m.Places_of_Practice.FirstOrDefault().States.Name,
                                        PIN = m.Places_of_Practice.FirstOrDefault().pincode,
                                        license_number = m.Places_of_Practice.FirstOrDefault().license_number,
                                        //  PlaceViewModel= oPHRMSDBContext.Places_of_Practice.Where(s=>s.docid==m.docid).FirstOrDefault()
                                    }).FirstOrDefault();
                // DoctorModel oDoctorModel = AutoMapper.Mapper.Map<Doctor, DoctorModel>(oPHRMSDBContext.Doctor.Find(id));
                return View(oDoctorModel);
            }
            catch (Exception ex)
            {


            }

            return View(new DoctorModel());
        }
        public JsonResult SaveStatus(Guid DocId, int Status, string Remarks)
        {
            try
            {
                PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
                Doctor oDoctor = oPHRMSDBContext.Doctor.Find(DocId);
                oDoctor.IsApproved = Status;
                oDoctor.ApprovedDate = DateTime.Now;
                oPHRMSDBContext.SaveChanges();
                //update Doctor Approval track in database also.
                updateDoctorApprovalTrack(oDoctor, DocId, Status, Remarks);
                return Json(Status);
            }
            catch (Exception ex)
            {
            }

            return Json(0);
        }

        public void updateDoctorApprovalTrack(Doctor oDoctor, Guid DocId, int Status, string Remarks)
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            DoctorApprovalTrackModel docApprovalModal = new DoctorApprovalTrackModel();
            CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
            docApprovalModal.docId = DocId;
            docApprovalModal.userId = oCustomPrincipalSerializeModel.AdminUserId;
            docApprovalModal.remarks = Remarks;
            docApprovalModal.status = Status;
            docApprovalModal.approvalDateTime = DateTime.Now;
            Mapper.CreateMap<DoctorApprovalTrackModel, DoctorApprovalTrack>();
            DoctorApprovalTrack odocApprovalTrack = Mapper.Map<DoctorApprovalTrack>(docApprovalModal);
            oPHRMSDBContext.DoctorApprovalTrack.Add(odocApprovalTrack);
            oPHRMSDBContext.SaveChanges();

            //Send messages and email
            bool result = SendVerificationCode(oDoctor.phone_number, oDoctor.email, Status, oDoctor.name);
        }


        private bool SendVerificationCode(string strMobileNo, string strEmailId, int Status, string name)
        {
            bool result = false;
            bool bEmail = false;
            string sms_content = "";
            try
            {
                string bodysignup1 = string.Empty;
                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\Templates\\SignUpStep_1.html")))
                {
                    bodysignup1 = reader.ReadToEnd();
                }

                bodysignup1 = bodysignup1.Replace("{Title}", name);
                //bodysignup1 = bodysignup1.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");


                //  Random randomclass = new Random();
                //   Int64 rno = randomclass.Next(100000, 999999);
                if (Status == 1)
                    sms_content = "Your Account in Pending Status";
                else if (Status == 2)
                    //sms_content = "Your account has been confirmed. You can proceed with your credentials in MyHealthRecord Practice Management System.";
                    sms_content = "Your account has been approved. You can login using your credentials at http://practice.myhealthrecord.nhp.gov.in";
                else if (Status == 3)
                    sms_content = "Your account has been Rejected for MyHealthRecord Practice Management System. Please contact Administrator for further details.";

                //string sms_content = "Your verification code for registration at mSwasthya Health Portal is " + rno + ".";
                //   BeforeSMSsend();
                result = sendInfiniSMS(strMobileNo, sms_content);

                bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);
                bEmail = SendEmail("MyHealthRecord - Confirmation Message", bodysignup1, strEmailId, true);
                result = result || bEmail;
                //to set in session, code
                //HttpContext.Session.SetString("SmsCode", rno.ToString());


            }

            catch (Exception ex)
            {
            }
            return result;
        }
        public static bool sendInfiniSMS(string MobileNo, string message)
        {
            bool result = false;
            try
            {
                Stream dataStream;
                HttpWebRequest req;
                String query = "api url" + "?method=sms" +
                   "&api_key=xxx" +
                   "&to=" + MobileNo +
                   "&sender=xxx" +
                   "&message=" + message;

                req = (HttpWebRequest)WebRequest.Create(query);
                req.ProtocolVersion = HttpVersion.Version10;
                req.Method = "POST";
                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                //= new HttpWebRequest() ;
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = byteArray.Length;
                dataStream = req.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = req.GetResponse();
                String Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                result = true;
            }
            catch (Exception ex)
            {
                //WriteErrorLog("From sendSingleSMS: " + ex.Message);
            }
            return result;
        }
       
        public bool SendEmail(string strSubject, string strBody, string strToEmail, bool strIsHtml)
        {
            string GmailHost = ""; //*GmailHost = "smtp.gmail.com";
            int GmailPort = 0; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            //bool GmailSSL = true;
            string GmailUsername
                = "";
            string GmailPassword = "";

            try
            {
                System.Threading.Thread email2 = new System.Threading.Thread(delegate ()
                {
                    try
                    {
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = GmailHost;
                        smtp.Port = GmailPort;
                        //smtp.EnableSsl = GmailSSL;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);
                        using (var message = new MailMessage(GmailUsername, strToEmail))
                        {
                            message.Subject = strSubject;
                            message.Body = strBody;
                            message.IsBodyHtml = strIsHtml;
                            smtp.Send(message);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
                email2.IsBackground = true;
                email2.Start();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}