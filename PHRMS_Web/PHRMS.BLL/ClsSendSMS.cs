
using System;
using System.IO;
using System.Text;
using System.Net;
using PHRMS.Data;
using PHRMS.Data.DataAccess;

namespace PHRMS.BLL
{
    public class ClsSendSMS
    {
        IPHRMSRepo _repository = null;

        /// <summary>
        /// Create a Catalog Service based on the passed-in repository
        /// </summary>
        /// <param name="repository">An ICatalogRepository</param>
        public ClsSendSMS(IPHRMSRepo repository)
        {
            _repository = repository;
            if (_repository == null)
            {
                throw new InvalidOperationException("Catalog Repository cannot be null");
            }
        }

        //static String username = "hiedmohali";
        //static String password = "Cdac#Hied1";
        //static String senderid = "CDACMH";
        //static String message = "Greetings!\nThis is a test message from mHealth application.\nRegards,\nCDAC Mohali";
        //static String mobileNo = "98159909233";//
        //static String mobileNos = "9856XXXXX, 9856XXXXX ";
        //static String scheduledTime = "20121022 11:00:00";
        static HttpWebRequest request;
        static Stream dataStream;

        public void mthPullSMS(string Message, string timeStamp, string operatorName, string areaCode, string mobileNumber)
        {
            PullSMS oPullSMS = new PullSMS();
            oPullSMS.Message = Message;
            oPullSMS.Time_Stamp = timeStamp;
            oPullSMS.operatorName = operatorName;
            oPullSMS.areaCode = areaCode;
            oPullSMS.mobileNumber = mobileNumber;
            _repository.InsertRecPullSMS(oPullSMS);
            oPullSMS = null;
        }

        public void BeforeSMSsend()
        {
            request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
            request.ProtocolVersion = HttpVersion.Version10;
            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            // Console.WriteLine("Before Calling Method");
        }

        public static bool sendSingleSMS(String username, String password, String senderid,
        String mobileNo, String message)
        {
            bool result = false;
            try
            {
                String smsservicetype = "singlemsg"; //For single message.
                String query = "username=" + WebUtility.UrlEncode(username) +
                    "&password=" + WebUtility.UrlEncode(password) +
                    "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
                    "&content=" + WebUtility.UrlEncode(message) +
                    "&mobileno=" + WebUtility.UrlEncode(mobileNo) +
                    "&senderid=" + WebUtility.UrlEncode(senderid);

                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
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
            catch (Exception ex) { }
            return result;
        }

        // method for sending bulk SMS
        public static void sendBulkSMS(String username, String password, String senderid, String mobileNos, String message)
        {
            String smsservicetype = "bulkmsg"; // for bulk msg
            String query = "username=" + WebUtility.UrlEncode(username) +
                "&password=" + WebUtility.UrlEncode(password) +
                "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
                "&content=" + WebUtility.UrlEncode(message) +
                "&bulkmobno=" + WebUtility.UrlEncode(mobileNos) +
                "&senderid=" + WebUtility.UrlEncode(senderid);
            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        //    for sending unicode

        public static void sendUnicodeSMS(String username, String password, String senderid, String mobileNos, String message)
        {
            String finalmessage = "";

            String sss = "";


            foreach (char c in message)
            {

                int j = (int)c;

                sss = "&#" + j + ";";

                finalmessage = finalmessage + sss;

                Console.WriteLine("Message in method==" + finalmessage);
            }

            Console.WriteLine("Before Calling Message" + finalmessage);

            message = finalmessage;

            String smsservicetype = "unicodemsg"; // for unicode msg

            String query = "username=" + WebUtility.UrlEncode(username) +

                "&password=" + WebUtility.UrlEncode(password) +

                "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +

                "&content=" + WebUtility.UrlEncode(message) +

                "&bulkmobno=" + WebUtility.UrlEncode(mobileNos) +

                "&senderid=" + WebUtility.UrlEncode(senderid);



            Console.WriteLine("URL==" + query);

            byte[] byteArray = Encoding.ASCII.GetBytes(query);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;

            dataStream = request.GetRequestStream();

            dataStream.Write(byteArray, 0, byteArray.Length);

            dataStream.Close();

            WebResponse response = request.GetResponse();

            String Status = ((HttpWebResponse)response).StatusDescription;

            dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream);

            string responseFromServer = reader.ReadToEnd();

            Console.WriteLine("response==" + responseFromServer);

            reader.Close();

            dataStream.Close();

            response.Close();

        }
    }
}
