using System;
using System.IO;
using System.Text;
using System.Net;
using System.Collections.Generic;
using System.Net.Mail;

namespace PHRMSEMR.Controllers
{
    public class ClsSendSMS
    {
        //static String username = "hiedmohali";
        //static String password = "Cdac#Hied1";
        //static String senderid = "CDACMH";
        //static String message = "Greetings!\nThis is a test message from mHealth application.\nRegards,\nCDAC Mohali";
        //static String mobileNo = "98159909233";//
        //static String mobileNos = "9856XXXXX, 9856XXXXX ";
        //static String scheduledTime = "20121022 11:00:00";
        static HttpWebRequest request;
        static Stream dataStream;

        //public void mthPullSMS(string Message, string timeStamp, string operatorName, string areaCode, string mobileNumber)
        //{
        //    PullSMS oPullSMS = new PullSMS();
        //    oPullSMS.Message = Message;
        //    oPullSMS.Time_Stamp = timeStamp;
        //    oPullSMS.operatorName = operatorName;
        //    oPullSMS.areaCode = areaCode;
        //    oPullSMS.mobileNumber = mobileNumber;
        //    _repository.InsertRecPullSMS(oPullSMS);
        //    oPullSMS = null;
        //}
        public static bool sendInfiniSMS(string MobileNo, String message)
        {
            bool result = false;
            try
            {
                Stream dataStream;
                HttpWebRequest req;
                String query = "http://api-alerts.solutionsinfini.com/v4/" + "?method=sms" +
                   "&api_key=Ad0d96781e5b0167dd700c01fd9d56f57" +
                   "&to=" + MobileNo +
                   "&sender=CDACMH" +
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
        //public void BeforeSMSsend()
        //{
        //    request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
        //    request.ProtocolVersion = HttpVersion.Version10;
        //    //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
        //    ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        //    request.Method = "POST";
        //    // Console.WriteLine("Before Calling Method");
        //}

        //public static bool sendSingleSMS(String username, String password, String senderid,
        //String mobileNo, String message)
        //{
        //    bool result = false;
        //    try
        //    {
        //        String smsservicetype = "singlemsg"; //For single message.
        //        String query = "username=" + WebUtility.UrlEncode(username) +
        //            "&password=" + WebUtility.UrlEncode(password) +
        //            "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
        //            "&content=" + WebUtility.UrlEncode(message) +
        //            "&mobileno=" + WebUtility.UrlEncode(mobileNo) +
        //            "&senderid=" + WebUtility.UrlEncode(senderid);

        //        byte[] byteArray = Encoding.ASCII.GetBytes(query);
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = byteArray.Length;

        //        dataStream = request.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //        WebResponse response = request.GetResponse();
        //        String Status = ((HttpWebResponse)response).StatusDescription;
        //        dataStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);
        //        string responseFromServer = reader.ReadToEnd();
        //        //if (responseFromServer =! "402")
        //        //{
        //        // ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('Message Sent Successfully')", true);

        //        //}
        //        //else
        //        //{
        //        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('There is some problem in sending the text')", true);

        //        //}
        //        reader.Close();
        //        dataStream.Close();
        //        response.Close();

        //        result = true;
        //    }
        //    catch (Exception ex) { }
        //    return result;
        //}

        //// method for sending bulk SMS
        //public static void sendBulkSMS(String username, String password, String senderid, String mobileNos, String message)
        //{
        //    String smsservicetype = "bulkmsg"; // for bulk msg
        //    String query = "username=" + WebUtility.UrlEncode(username) +
        //        "&password=" + WebUtility.UrlEncode(password) +
        //        "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
        //        "&content=" + WebUtility.UrlEncode(message) +
        //        "&bulkmobno=" + WebUtility.UrlEncode(mobileNos) +
        //        "&senderid=" + WebUtility.UrlEncode(senderid);
        //    byte[] byteArray = Encoding.ASCII.GetBytes(query);
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = byteArray.Length;
        //    dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    WebResponse response = request.GetResponse();
        //    String Status = ((HttpWebResponse)response).StatusDescription;
        //    dataStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string responseFromServer = reader.ReadToEnd();
        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();
        //}

        ////    for sending unicode

        //public static void sendUnicodeSMS(String username, String password, String senderid, String mobileNos, String message)
        //{
        //    String finalmessage = "";

        //    String sss = "";


        //    foreach (char c in message)
        //    {

        //        int j = (int)c;

        //        sss = "&#" + j + ";";

        //        finalmessage = finalmessage + sss;

        //        Console.WriteLine("Message in method==" + finalmessage);
        //    }

        //    Console.WriteLine("Before Calling Message" + finalmessage);

        //    message = finalmessage;

        //    String smsservicetype = "unicodemsg"; // for unicode msg

        //    String query = "username=" + WebUtility.UrlEncode(username) +

        //        "&password=" + WebUtility.UrlEncode(password) +

        //        "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +

        //        "&content=" + WebUtility.UrlEncode(message) +

        //        "&bulkmobno=" + WebUtility.UrlEncode(mobileNos) +

        //        "&senderid=" + WebUtility.UrlEncode(senderid);



        //    Console.WriteLine("URL==" + query);

        //    byte[] byteArray = Encoding.ASCII.GetBytes(query);

        //    request.ContentType = "application/x-www-form-urlencoded";

        //    request.ContentLength = byteArray.Length;

        //    dataStream = request.GetRequestStream();

        //    dataStream.Write(byteArray, 0, byteArray.Length);

        //    dataStream.Close();

        //    WebResponse response = request.GetResponse();

        //    String Status = ((HttpWebResponse)response).StatusDescription;

        //    dataStream = response.GetResponseStream();

        //    StreamReader reader = new StreamReader(dataStream);

        //    string responseFromServer = reader.ReadToEnd();

        //    Console.WriteLine("response==" + responseFromServer);

        //    reader.Close();

        //    dataStream.Close();

        //    response.Close();

        //}
    }

    public class EMailer
    {
        public static string GmailUsername { get; set; }
        public static string GmailPassword { get; set; }
        public static string GmailHost { get; set; }
        public static int GmailPort { get; set; }
        public static bool GmailSSL { get; set; }

        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> lstAttachments { get; set; }
        public string strRoot { get; set; }
        public bool IsHtml { get; set; }

        static EMailer()
        {
            GmailHost = ""; //*GmailHost = "smtp.gmail.com";
            GmailPort = 0; // Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.
            GmailUsername = "";
            GmailPassword = "";
       }

        public void Send()
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = GmailHost;
                smtp.Port = GmailPort;
                smtp.EnableSsl = GmailSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);
                using (var message = new MailMessage(GmailUsername, ToEmail))
                {
                    message.Subject = Subject;
                    message.Body = Body;
                    message.IsBodyHtml = IsHtml;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static bool SendEmailWithAttachmentFeed(string strSubject, string strBody, string strToEmail, bool strIsHtml, List<string> lstAttachments, string strRoot)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = GmailHost;
                smtp.Port = GmailPort;
                smtp.EnableSsl = GmailSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);
                using (var message = new MailMessage(GmailUsername, strToEmail))
                {
                    message.Subject = strSubject;
                    message.Body = strBody;
                    message.IsBodyHtml = true;
                
                    if (lstAttachments != null && lstAttachments.Count > 0)
                    {
                        Attachment attachment = null;
                        foreach (string item in lstAttachments)
                        {
                            attachment = new Attachment(strRoot + item.Replace("//", @"\"));
                            message.Attachments.Add(attachment);
                            attachment = null;
                        }
                    }
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
             
                return false;
            }
            return true;
        }

        public void SendWithAttachments()
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = GmailHost;
                smtp.Port = GmailPort;
                smtp.EnableSsl = GmailSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(GmailUsername, GmailPassword);
                using (var message = new MailMessage(GmailUsername, ToEmail))
                {
                    message.Subject = Subject;
                    message.Body = Body;
                    message.IsBodyHtml = IsHtml;
                    if (lstAttachments != null && lstAttachments.Count > 0)
                    {
                        Attachment attachment = null;
                        foreach (string item in lstAttachments)
                        {
                            attachment = new Attachment(strRoot + item);
                            message.Attachments.Add(attachment);
                            attachment = null;
                        }
                    }
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static bool SendEmail(string strSubject, string strBody, string strToEmail, bool strIsHtml)
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
                return false;
            }
            return true;
        }

        public static bool SendEmailWithAttachment(string strSubject, string strBody, string strToEmail, bool strIsHtml, List<string> lstAttachments, string strRoot)
        {
            try
            {
                EMailer mailer = new EMailer();
                mailer.Subject = strSubject;
                mailer.Body = strBody;
                mailer.ToEmail = strToEmail;
                mailer.IsHtml = strIsHtml;
                mailer.lstAttachments = lstAttachments;
                mailer.strRoot = strRoot;
                System.Threading.Thread email2 = new System.Threading.Thread(delegate ()
                {
                    mailer.SendWithAttachments();
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
