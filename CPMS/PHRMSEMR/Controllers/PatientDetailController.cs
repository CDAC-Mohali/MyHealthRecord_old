using EMRLib.DAL;
using EMRViewModels;
using PHRMSEMR.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using static PHRMSEMR.FilterConfig;

namespace PHRMSEMR.Controllers
{
    [Error]
    [AuthorizationFilter]
    public class PatientDetailController : Controller
    {
        IEMRRepository _repo;
        public PatientDetailController()
        {
            _repo = new EMRRepository();
        }
     
      
      

        [HandleError]
        public JsonResult GenerateOTP(int Type)
        {

            try
            {
                Guid PatientId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).PhrmsUserId;
                var obj = _repo.GetPersonalInformation(PatientId);
                string status = _repo.AddPHRMSOTPShare(PatientId, Type);
                System.Threading.Thread thread1 = new System.Threading.Thread(delegate ()
                {
                    sendSMS("hiedmohali", "Cdac#Hied1", "CDACMH", obj.Cell_Phone,"The Pin for Patient Detail is " + status + ".");
                });
                thread1.IsBackground = true;
                thread1.Start();

                string sms_content = "";
                string bodysignup1 = string.Empty;
                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\PatDetail.html")))
                {
                    bodysignup1 = reader.ReadToEnd();
                }
                bodysignup1 = bodysignup1.Replace("{Title}", obj.FirstName);
                //bodysignup1 = bodysignup1.Replace("{rno}", status);
                bodysignup1 = bodysignup1.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");
                sms_content = "The Pin for Patient Detail is " + status + ".";
                 bodysignup1 = bodysignup1.Replace("{messagephrms}", sms_content);

                EMailer.SendEmail("MyHealthRecord", bodysignup1, obj.Email, true);
                return Json("1");
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "PatientDetail");

            }
            return Json("");

        }


        public JsonResult CheckPhoneRevist(string Phone, long RecodeId)
        {

            try
            {
                Guid DocId = ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId;

                var result = _repo.CheckPhoneRevist(Phone,RecodeId,DocId);
                return Json(result);
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "PatientDetail");
                return Json("1");
            }
          

        }

        public bool sendSMS(String username, String password, String senderid, String mobileNo, String message)
        {
            Stream dataStream;
            bool result = false;
            try
            {
                HttpWebRequest req;
                req = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
                req.ProtocolVersion = HttpVersion.Version10;
                //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
                ((HttpWebRequest)req).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                req.Method = "POST";

                String smsservicetype = "singlemsg"; //For single message.
                String query = "username=" + WebUtility.UrlEncode(username) +
                    "&password=" + WebUtility.UrlEncode(password) +
                    "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
                    "&content=" + WebUtility.UrlEncode(message) +
                    "&mobileno=" + WebUtility.UrlEncode(mobileNo) +
                    "&senderid=" + WebUtility.UrlEncode(senderid);

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
                //if (responseFromServer =! "402")
                //{
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('Message Sent Successfully')", true);

                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('There is some problem in sending the text')", true);

                //}
                reader.Close();
                dataStream.Close();
                response.Close();

                result = true;
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "PatientDetail");
            }
            return result;
        }

        public bool SendEmail(string strSubject, string strBody, string strToEmail, bool strIsHtml)
        {
            try
            {
                EMailer mailer = new EMailer();
                mailer.Subject = strSubject;
                mailer.Body = strBody;
                mailer.ToEmail = strToEmail;
                mailer.IsHtml = strIsHtml;
                System.Threading.Thread email2 = new System.Threading.Thread(delegate ()
                {
                    mailer.Send();
                });
                email2.IsBackground = true;
                email2.Start();
            }
            catch (Exception ex)
            {
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "PatientDetail");
                return false;
            }
            return true;
        }

    }
}