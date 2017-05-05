using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;



using Microsoft.AspNet.Identity;


//namespace PHRMSEMR.Models
namespace PHRMSEMR.Controllers
{
    public static class MessageType
    {

        public static string Error = "Error";

        public static string Information = "Information";

    }
    public class Common
    {
        public static bool CreateLog(string Query, String MessageType = "", string ControllerName = "")
        {
            try
            {            
                String FileName = string.Empty;
                if (DateTime.Now.Hour > 11)
                    FileName = String.Format("DB_{0}{1}{2}_evening.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                else
                    FileName = String.Format("DB_{0}{1}{2}_evening.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                //string BasePath = AppSetting.Setting["BaseFolderPath"];// @"D:\amrik@arethos.com\BCA\QSL\QSLAPP\Documents";// AppSetting.GetKeyValue("BaseFolderPath");

                string BasePath = HttpContext.Current.Server.MapPath("~/ApplicationLogs");// System.Configuration.ConfigurationManager.AppSettings["BaseFolderPath"].ToString();
                string DBLogFolder = BasePath + "\\Logs";
                if (ControllerName != "")
                {
                    DBLogFolder = DBLogFolder + "\\" + ControllerName;
                }
                if (MessageType != "")
                {
                    DBLogFolder = DBLogFolder + "\\" + MessageType;
                }
                if (!Directory.Exists(DBLogFolder))
                    Directory.CreateDirectory(DBLogFolder);
               
                    System.IO.File.AppendAllText(DBLogFolder + "\\" + FileName, Query);
                //Mail Code
                bool result = false;
                bool bEmail = false;
                string bodysignup1 = string.Empty;
                using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("\\App_Data\\Templates\\Bugs.html")))
                {
                    bodysignup1 = reader.ReadToEnd();
                }

                bodysignup1 = bodysignup1.Replace("{Title}", "Welcome to MyHealthRecord");
                bodysignup1 = bodysignup1.Replace("{p_footerImage}", "Copyright 2016-17 MyHealthRecord. All Rights Reserved");

                bodysignup1 = bodysignup1.Replace("{messagephrms}", Query);
                bEmail = EMailer.SendEmail("MyHealthRecord - Bug - " + ControllerName, bodysignup1, "", true);
                result = result || bEmail;
             }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public static string ExecptionMessage(Exception ex)
        {
            string Result = "";
            try
            {
                Result = string.Format("{0}\r\n\r{1}\r\n\r\n{2}\r\n\r{3}\r\n\r{4}\r\n{5}\r\n{6}\r\n\r", "", "ERROR OCCURRED", "DATE & TIME: " + DateTime.Now.ToString("MM-dd-yyyy") + " " + DateTime.Now.ToLongTimeString(), "SOURCE: " + ex.Source, "METHOD: " + ex.TargetSite, "ERROR: " + ex.Message, "STACKTRACE: " + ex.StackTrace);
            }
            catch (Exception)
            {


            }
            return Result;
        }

    }
}