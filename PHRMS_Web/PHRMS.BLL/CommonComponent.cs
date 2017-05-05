using PHRMS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public string GetProfilePercentage(Guid userid)
        {
            try
            {
                return _repository.GetProfilePercentage(userid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetUserCount()
        {
            return _repository.GetUserCount();
        }
        public Users GetUserDetailsForOpenEMR(Guid Id)
        {
            return _repository.GetUserDetailsForOpenEMR(Id);
        }
    }

    public class CommonComponent
    {
      

        #region DynamicDictionary
        public class DynamicDictionary : DynamicObject
        {
            Dictionary<string, object> dict;

            public DynamicDictionary(Dictionary<string, object> dict)
            {
                this.dict = dict;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                dict[binder.Name] = value;
                return true;
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return dict.TryGetValue(binder.Name, out result);
            }
        }
        #endregion

        public static string CalculateAge(DateTime bday)
        {
            string strAge = "0 years";
            try
            {
                if (bday != null)
                {
                    DateTime today = DateTime.Today;
                    int age = today.Year - bday.Year;
                    if (bday > today.AddYears(-age)) age--;
                    strAge = age.ToString() + " years";
                }
            }
            catch (Exception)
            {
            }
            return strAge;
        }
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
            GmailHost = ""; /*GmailHost = "smtp.gmail.com";*/
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
                CatalogService.WriteErrorLog("From Send(Email): " + ex.Message);
            }
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
                CatalogService.WriteErrorLog("From Send(Email): " + ex.Message);
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
                CatalogService.WriteErrorLog("From Send(Email): " + ex.Message);
                return false;
            }
            return true;
        }

        public static bool SendEmailWithAttachment(string strSubject, string strBody, string strToEmail, bool strIsHtml,List<string> lstAttachments, string strRoot)
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
                CatalogService.WriteErrorLog("From Send(Email): " + ex.Message);
                return false;
            }
            return true;
        }
    }

    public static class DisplayNameHelper
    {
        public static string GetDisplayName(object obj, string propertyName)
        {
            if (obj == null) return null;
            return GetDisplayName(obj.GetType(), propertyName);

        }

        public static string GetDisplayName(Type type, string propertyName)
        {
            var property = type.GetProperty(propertyName);
            if (property == null) return null;

            return GetDisplayName(property);
        }

        public static string GetDisplayName(PropertyInfo property)
        {
            var attrName = GetAttributeDisplayName(property);
            if (!string.IsNullOrEmpty(attrName))
                return attrName;

            var metaName = GetMetaDisplayName(property);
            if (!string.IsNullOrEmpty(metaName))
                return metaName;

            //return property.Name.ToString(CultureInfo.InvariantCulture);
            return "";
        }

        private static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return null;
            var displayNameAttribute = atts[0] as DisplayAttribute;
            return displayNameAttribute != null ? displayNameAttribute.GetName() : null;
        }

        private static string GetMetaDisplayName(PropertyInfo property)
        {
            if (property.DeclaringType != null)
            {
                var atts = property.DeclaringType.GetCustomAttributes(
                    typeof(MetadataTypeAttribute), true);
                if (atts.Length == 0)
                    return null;

                var metaAttr = atts[0] as MetadataTypeAttribute;
                if (metaAttr != null)
                {
                    var metaProperty =
                        metaAttr.MetadataClassType.GetProperty(property.Name);
                    return metaProperty == null ? null : GetAttributeDisplayName(metaProperty);
                }
            }
            return null;
        }




        }
}
