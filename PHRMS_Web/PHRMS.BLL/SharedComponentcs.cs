using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc.Rendering;
using System.Net;
using System.Text;
using System.IO;
using System.Data;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        static List<SelectListItem> StatesList;
        static List<SelectListItem> RelationList;
        static List<SelectListItem> BloodGroupsList;
        static List<SelectListItem> DisabilitiesList;
        static List<SelectListItem> GendersList;
        static List<SelectListItem> ContactType;
        static List<SelectListItem> OpenEMRStatesList;
        static List<SelectListItem> DistrictListOnStateId;
        static List<SelectListItem> SubDistrictListOnDistrictId;
        static List<SelectListItem> PreferredHospital;
        public static string WebRootPath { get; set; }


        public IEnumerable<SelectListItem> GetStatesList()
        {
            if (StatesList == null)
            {
                StatesList = _repository.GetStatesList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                StatesList.Insert(0, new SelectListItem { Text = "---Select State---", Value = "" });
            }

            return StatesList;
        }

        public IEnumerable<SelectListItem> GetPreferredHospitalList()
        {
            if (PreferredHospital == null)
            {
                PreferredHospital = new List<SelectListItem>();
                PreferredHospital.Insert(0, new SelectListItem { Text = "Public", Value = "Public" });
                PreferredHospital.Insert(1, new SelectListItem { Text = "Private", Value = "Private" });
            }

            return PreferredHospital;
        }

        public IEnumerable<SelectListItem> GetStatesOpenEMRList()
        {
            if (OpenEMRStatesList == null)
            {
                OpenEMRStatesList = _repository.GetStatesOpenEMRList().Select(x => new SelectListItem { Text = x.StateName, Value = x.StateId.ToString() }).ToList();
                OpenEMRStatesList.Insert(0, new SelectListItem { Text = "---Select State---", Value = "0" });
            }

            return OpenEMRStatesList;
        }

        public IEnumerable<SelectListItem> GetDistrictNameByStateId(int StateId)
        {

            if (DistrictListOnStateId == null)
            {
                DistrictListOnStateId = _repository.GetDistrictNameByStateId(StateId).Select(x => new SelectListItem { Text = x.DistrictName, Value = x.DistrictId.ToString() }).ToList();
                DistrictListOnStateId.Insert(0, new SelectListItem { Text = "---Select Districts---", Value = "0" });
            }
            DistrictListOnStateId = _repository.GetDistrictNameByStateId(StateId).Select(x => new SelectListItem { Text = x.DistrictName, Value = x.DistrictId.ToString() }).ToList();
            return DistrictListOnStateId;
        }

        public IEnumerable<SelectListItem> GetSubDistrictNameByDistrictId(int DistrictId)
        {

            if (SubDistrictListOnDistrictId == null)
            {
                SubDistrictListOnDistrictId = _repository.GetSubDistrictNameByDistrictId(DistrictId).Select(x => new SelectListItem { Text = x.SubDistrictsName, Value = x.SubDistrictsId.ToString() }).ToList();
                SubDistrictListOnDistrictId.Insert(0, new SelectListItem { Text = "---Select Sub-Districts---", Value = "0" });
            }
            SubDistrictListOnDistrictId = _repository.GetSubDistrictNameByDistrictId(DistrictId).Select(x => new SelectListItem { Text = x.SubDistrictsName, Value = x.SubDistrictsId.ToString() }).ToList();
            return SubDistrictListOnDistrictId;
        }


        public IEnumerable<SelectListItem> GetContactTypesList()
        {
            if (ContactType == null)
            {
                ContactType = _repository.GetContactTypesList().Select(x => new SelectListItem { Text = x.MedContType, Value = x.Id.ToString() }).ToList();
                ContactType.Insert(0, new SelectListItem { Text = "---Select Speciality---", Value = "0" });
            }

            return ContactType;
        }
        public IEnumerable<SelectListItem> GetRelationshipList()
        {
            if (RelationList == null)
            {
                RelationList = _repository.GetRelationshipList().Select(x => new SelectListItem { Text = x.Relation, Value = x.Id.ToString() }).ToList();
                RelationList.Insert(0, new SelectListItem { Text = "---Select Relation---", Value = "0" });
            }

            return RelationList;
        }



        public IEnumerable<SelectListItem> GetDisablitiesList()
        {
            if (DisabilitiesList == null)
            {
                DisabilitiesList = _repository.GetDisabilityTypesList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                DisabilitiesList.Insert(0, new SelectListItem { Text = "Do Not Specify", Value = "0" });
            }

            return DisabilitiesList;
        }

        public IEnumerable<SelectListItem> GetBloodGroupsList()
        {
            if (BloodGroupsList == null)
            {
                BloodGroupsList = _repository.GetBloodGroupsList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                BloodGroupsList.Insert(0, new SelectListItem { Text = "Do Not Specify", Value = "0" });
            }

            return BloodGroupsList;
        }

        public IEnumerable<SelectListItem> GetGendersList()
        {
            if (GendersList == null)
            {
                var data = new[]{
                 new SelectListItem{ Value="U",Text="Do Not Specify"},
                 new SelectListItem{ Value="M",Text="Male"},
                 new SelectListItem{ Value="F",Text="Female"}
             };
                GendersList = data.ToList();
            }

            return GendersList;
        }

        public string GetYesorNo(bool val)
        {
            return val ? "Yes" : "No";
        }

        public string GetEmailByUserId(string strUserId)
        {
            if (strUserId != null)
            {
                Guid userId = Guid.Parse(strUserId);
                return _repository.GetEmailByUserId(userId);
            }
            else
                return "";
        }



        public bool SetProfilePic(string path, Guid userID)
        {
            FileViewModel oModel = new FileViewModel();
            oModel.FileType = FileType.ProfilePic;
            oModel.FileName = Path.GetFileName(path);
            oModel.UserId = userID;
            if (_repository.SoftDeleteFileIfExists(oModel))
            {
                _repository.SaveFileDetails(oModel);
            }
            return _repository.SetProfilePic(path, userID);
        }

        public bool SaveFilePath(FileViewModel oFileViewModel)
        {
            bool res = false;
            try
            {
                if (_repository.SoftDeleteFileIfExists(oFileViewModel))
                {
                    res = _repository.SaveFileDetails(oFileViewModel);
                }
            }
            catch (Exception)
            {
            }
            return res;
        }

        #region Autosuggests

        public List<MedicalContactRecordsViewModel> GetDoctorsList(Guid UserId)
        {
            return _repository.GetDoctorsList(UserId);
        }
        public List<string> GetPostalCodesFromMaster(string strPostalCode)
        {
            return _repository.GetPostalCodesFromMaster(strPostalCode);
        }
        #endregion

        #region ClsSendSms
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

        //static String username = "hiedmohali";
        //static String password = "Cdac#Hied1";
        //static String senderid = "CDACMH";
        //static String message = "Greetings!\nThis is a test message from mHealth application.\nRegards,\nCDAC Mohali";
        //static String mobileNo = "98159909233";//
        //static String mobileNos = "9856XXXXX, 9856XXXXX ";
        //static String scheduledTime = "20121022 11:00:00";
     //   static HttpWebRequest request;
     //   static Stream dataStream;

     //   public void BeforeSMSsend()
     //   {
     //       request = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
     //       request.ProtocolVersion = HttpVersion.Version10;
     //       //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
     //       ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
     //       request.Method = "POST";
     //       // Console.WriteLine("Before Calling Method");
     //   }

     //   public static bool sendSingleSMS(String username, String password, String senderid,
     //   String mobileNo, String message)
     //   {
     //       bool result = false;
     //       try
     //       {
     //           String smsservicetype = "singlemsg"; //For single message.
     //           String query = "username=" + WebUtility.UrlEncode(username) +
     //               "&password=" + WebUtility.UrlEncode(password) +
     //               "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
     //               "&content=" + WebUtility.UrlEncode(message) +
     //               "&mobileno=" + WebUtility.UrlEncode(mobileNo) +
     //               "&senderid=" + WebUtility.UrlEncode(senderid);

     //           byte[] byteArray = Encoding.ASCII.GetBytes(query);
     //           request.ContentType = "application/x-www-form-urlencoded";
     //           request.ContentLength = byteArray.Length;

     //           dataStream = request.GetRequestStream();
     //           dataStream.Write(byteArray, 0, byteArray.Length);
     //           dataStream.Close();
     //           WebResponse response = request.GetResponse();
     //           String Status = ((HttpWebResponse)response).StatusDescription;
     //           dataStream = response.GetResponseStream();
     //           StreamReader reader = new StreamReader(dataStream);
     //           string responseFromServer = reader.ReadToEnd();
     //           //if (responseFromServer =! "402")
     //           //{
     //           // ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('Message Sent Successfully')", true);

     //           //}
     //           //else
     //           //{
     //           //    ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('There is some problem in sending the text')", true);

     //           //}
     //           reader.Close();
     //           dataStream.Close();
     //           response.Close();

     //           result = true;
     //       }
     //       catch (Exception ex)
     //       {
     //           WriteErrorLog("From sendSingleSMS: " + ex.Message);
     //       }
     //       return result;
     //   }


     //   public static bool sendSMS(String username, String password, String senderid,
     //String mobileNo, String message)
     //   {
     //       bool result = false;
     //       try
     //       {
     //           HttpWebRequest req;
     //           req = (HttpWebRequest)WebRequest.Create("http://msdgweb.mgov.gov.in/esms/sendsmsrequest");
     //           req.ProtocolVersion = HttpVersion.Version10;
     //           //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
     //           ((HttpWebRequest)req).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
     //           req.Method = "POST";

     //           String smsservicetype = "singlemsg"; //For single message.
     //           String query = "username=" + WebUtility.UrlEncode(username) +
     //               "&password=" + WebUtility.UrlEncode(password) +
     //               "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
     //               "&content=" + WebUtility.UrlEncode(message) +
     //               "&mobileno=" + WebUtility.UrlEncode(mobileNo) +
     //               "&senderid=" + WebUtility.UrlEncode(senderid);

     //           byte[] byteArray = Encoding.ASCII.GetBytes(query);
     //           //= new HttpWebRequest() ;
     //           req.ContentType = "application/x-www-form-urlencoded";
     //           req.ContentLength = byteArray.Length;

     //           dataStream = req.GetRequestStream();
     //           dataStream.Write(byteArray, 0, byteArray.Length);
     //           dataStream.Close();
     //           WebResponse response = req.GetResponse();
     //           String Status = ((HttpWebResponse)response).StatusDescription;
     //           dataStream = response.GetResponseStream();
     //           StreamReader reader = new StreamReader(dataStream);
     //           string responseFromServer = reader.ReadToEnd();
     //           //if (responseFromServer =! "402")
     //           //{
     //           // ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('Message Sent Successfully')", true);

     //           //}
     //           //else
     //           //{
     //           //    ScriptManager.RegisterStartupScript(this, this.GetType(), "aa", "alert('There is some problem in sending the text')", true);

     //           //}
     //           reader.Close();
     //           dataStream.Close();
     //           response.Close();

     //           result = true;
     //       }
     //       catch (Exception ex)
     //       {
     //           WriteErrorLog("From sendSingleSMS: " + ex.Message);
     //       }
     //       return result;
     //   }
     //   // method for sending bulk SMS
     //   public static void sendBulkSMS(String username, String password, String senderid, String mobileNos, String message)
     //   {
     //       String smsservicetype = "bulkmsg"; // for bulk msg
     //       String query = "username=" + WebUtility.UrlEncode(username) +
     //           "&password=" + WebUtility.UrlEncode(password) +
     //           "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +
     //           "&content=" + WebUtility.UrlEncode(message) +
     //           "&bulkmobno=" + WebUtility.UrlEncode(mobileNos) +
     //           "&senderid=" + WebUtility.UrlEncode(senderid);
     //       byte[] byteArray = Encoding.ASCII.GetBytes(query);
     //       request.ContentType = "application/x-www-form-urlencoded";
     //       request.ContentLength = byteArray.Length;
     //       dataStream = request.GetRequestStream();
     //       dataStream.Write(byteArray, 0, byteArray.Length);
     //       dataStream.Close();
     //       WebResponse response = request.GetResponse();
     //       String Status = ((HttpWebResponse)response).StatusDescription;
     //       dataStream = response.GetResponseStream();
     //       StreamReader reader = new StreamReader(dataStream);
     //       string responseFromServer = reader.ReadToEnd();
     //       reader.Close();
     //       dataStream.Close();
     //       response.Close();
     //   }

     //   //    for sending unicode

     //   public static void sendUnicodeSMS(String username, String password, String senderid, String mobileNos, String message)
     //   {
     //       String finalmessage = "";

     //       String sss = "";


     //       foreach (char c in message)
     //       {

     //           int j = (int)c;

     //           sss = "&#" + j + ";";

     //           finalmessage = finalmessage + sss;

     //           Console.WriteLine("Message in method==" + finalmessage);
     //       }

     //       Console.WriteLine("Before Calling Message" + finalmessage);

     //       message = finalmessage;

     //       String smsservicetype = "unicodemsg"; // for unicode msg

     //       String query = "username=" + WebUtility.UrlEncode(username) +

     //           "&password=" + WebUtility.UrlEncode(password) +

     //           "&smsservicetype=" + WebUtility.UrlEncode(smsservicetype) +

     //           "&content=" + WebUtility.UrlEncode(message) +

     //           "&bulkmobno=" + WebUtility.UrlEncode(mobileNos) +

     //           "&senderid=" + WebUtility.UrlEncode(senderid);



     //       Console.WriteLine("URL==" + query);

     //       byte[] byteArray = Encoding.ASCII.GetBytes(query);

     //       request.ContentType = "application/x-www-form-urlencoded";

     //       request.ContentLength = byteArray.Length;

     //       dataStream = request.GetRequestStream();

     //       dataStream.Write(byteArray, 0, byteArray.Length);

     //       dataStream.Close();

     //       WebResponse response = request.GetResponse();

     //       String Status = ((HttpWebResponse)response).StatusDescription;

     //       dataStream = response.GetResponseStream();

     //       StreamReader reader = new StreamReader(dataStream);

     //       string responseFromServer = reader.ReadToEnd();

     //       Console.WriteLine("response==" + responseFromServer);

     //       reader.Close();

     //       dataStream.Close();

     //       response.Close();

     //   }
        #endregion

        public static void WriteErrorLog(string msg)
        {
            // Compose a string that consists of three lines.
            string lines = msg + " " + DateTime.Now.ToString();
            string dirPath = WebRootPath + "/TestFiles";
            bool exists = System.IO.Directory.Exists(dirPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(dirPath);
            // Write the string to a file.
            using (System.IO.StreamWriter w = System.IO.File.AppendText(dirPath + "/ErrorLog.txt"))
            {
                w.WriteLine(lines);
            }
        }
        public SuperReportShareViewModel GetReport(Guid UserId, string strChecks)
        {

            SuperReportShareViewModel oSuperReportShareViewModel = new SuperReportShareViewModel();


            foreach (string item in strChecks.Split(','))
            {
                switch (int.Parse(item))
                {
                    case ReportParameters.PersonalInformation:
                        oSuperReportShareViewModel.PersonalViewModel = GetPersonalInformation(UserId);
                        break;
                    case ReportParameters.EmergencyInformation:
                        oSuperReportShareViewModel.EmergencyViewModel = GetEmergencyInformation(UserId);
                        break;
                    case ReportParameters.EmployerInformation:
                        oSuperReportShareViewModel.EmployerViewModel = GetEmployerInformation(UserId);
                        break;
                    case ReportParameters.InsuranceInformation:
                        oSuperReportShareViewModel.InsuranceViewModel = GetInsuranceInformation(UserId);
                        break;
                    case ReportParameters.LegalInformation:
                        oSuperReportShareViewModel.LegalViewModel = GetLegalInformation(UserId);
                        break;
                    case ReportParameters.Preferences:
                        oSuperReportShareViewModel.PreferencesViewModel = _repository.GetPreferences(UserId);
                        break;
                    case ReportParameters.Allergies:
                        oSuperReportShareViewModel.AllergyViewModel = GetAllergiesExportableList(UserId);
                        break;
                    case ReportParameters.Problems:
                        oSuperReportShareViewModel.HealthConditionViewModel = GetHealthConditionExportableList(UserId);
                        break;
                    case ReportParameters.Medications:
                        oSuperReportShareViewModel.MedicationViewModel = GetMedicationExportableList(UserId);
                        break;
                    case ReportParameters.Immunizations:
                        oSuperReportShareViewModel.ImmunizationViewModel = GetlstImmunizationExportableList(UserId);
                        break;
                    case ReportParameters.Tests:
                        oSuperReportShareViewModel.LabTestViewModel = GetLabTestExportableList(UserId);
                        break;
                    case ReportParameters.Procedures:
                        oSuperReportShareViewModel.ProceduresViewModel = GetProceduresExportableList(UserId);
                        break;
                    case ReportParameters.Activities:
                        oSuperReportShareViewModel.ActivitiesViewModel = GetActivitiesExportableList(UserId);
                        break;
                    case ReportParameters.BP:
                        oSuperReportShareViewModel.BloodPressureAndPulseViewModel = GetBloodPressureAndPulseExportableList(UserId);
                        break;
                    case ReportParameters.Weight:
                        oSuperReportShareViewModel.WeightViewModel = GetWeightExportableList(UserId);
                        break;
                    case ReportParameters.Glucose:
                        oSuperReportShareViewModel.BloodGlucoseViewModel = GetBloodGlucoseRecordsist(UserId);
                        break;

                    default:
                        break;
                }
            }
            return oSuperReportShareViewModel;
        }
        public SuperReportShareViewModel GetShareReport(string Passwrod)
        {
            UserShareRecordViewModels oUserShareRecordViewModels = _repository.GetUserId(Passwrod);
            SuperReportShareViewModel oSuperReportShareViewModel = new SuperReportShareViewModel();
            if (oUserShareRecordViewModels != null)
            {
                oSuperReportShareViewModel.UserId = oUserShareRecordViewModels.UserId;
                oSuperReportShareViewModel.MedicalContactId = oUserShareRecordViewModels.MedicalContactId;
                oSuperReportShareViewModel.UserRecordId = oUserShareRecordViewModels.UserRecordId;
                oSuperReportShareViewModel.Query = oUserShareRecordViewModels.Query;
                if (oUserShareRecordViewModels.Password.Contains("Expired"))
                {
                    oSuperReportShareViewModel.Status = oUserShareRecordViewModels.Password;
                    return oSuperReportShareViewModel;
                }
                oSuperReportShareViewModel.ImagePath = oUserShareRecordViewModels.ImagePath;
                oSuperReportShareViewModel.Status = "Success";
                Guid UserId = oUserShareRecordViewModels.UserId;

                foreach (string item in oUserShareRecordViewModels.strChecks.Split(','))
                {
                    switch (int.Parse(item))
                    {
                        case ReportParameters.PersonalInformation:
                            oSuperReportShareViewModel.PersonalViewModel = GetPersonalInformation(UserId);
                            break;
                        case ReportParameters.EmergencyInformation:
                            oSuperReportShareViewModel.EmergencyViewModel = GetEmergencyInformation(UserId);
                            break;
                        case ReportParameters.EmployerInformation:
                            oSuperReportShareViewModel.EmployerViewModel = GetEmployerInformation(UserId);
                            break;
                        case ReportParameters.InsuranceInformation:
                            oSuperReportShareViewModel.InsuranceViewModel = GetInsuranceInformation(UserId);
                            break;
                        case ReportParameters.LegalInformation:
                            oSuperReportShareViewModel.LegalViewModel = GetLegalInformation(UserId);
                            break;
                        case ReportParameters.Preferences:
                            oSuperReportShareViewModel.PreferencesViewModel = _repository.GetPreferences(UserId);
                            break;
                        case ReportParameters.Allergies:
                            oSuperReportShareViewModel.AllergyViewModel = GetAllergiesExportableList(UserId);
                            break;
                        case ReportParameters.Problems:
                            oSuperReportShareViewModel.HealthConditionViewModel = GetHealthConditionExportableList(UserId);
                            break;
                        case ReportParameters.Medications:
                            oSuperReportShareViewModel.MedicationViewModel = GetMedicationExportableList(UserId);
                            break;
                        case ReportParameters.Immunizations:
                            oSuperReportShareViewModel.ImmunizationViewModel = GetlstImmunizationExportableList(UserId);
                            break;
                        case ReportParameters.Tests:
                            oSuperReportShareViewModel.LabTestViewModel = GetLabTestExportableList(UserId);
                            break;
                        case ReportParameters.Procedures:
                            oSuperReportShareViewModel.ProceduresViewModel = GetProceduresExportableList(UserId);
                            break;
                        case ReportParameters.Activities:
                            oSuperReportShareViewModel.ActivitiesViewModel = GetActivitiesExportableList(UserId);
                            break;
                        case ReportParameters.BP:
                            oSuperReportShareViewModel.BloodPressureAndPulseViewModel = GetBloodPressureAndPulseExportableList(UserId);
                            break;
                        case ReportParameters.Weight:
                            oSuperReportShareViewModel.WeightViewModel = GetWeightExportableList(UserId);
                            break;
                        case ReportParameters.Glucose:
                            oSuperReportShareViewModel.BloodGlucoseViewModel = GetBloodGlucoseRecordsist(UserId);
                            break;

                        default:
                            break;
                    }
                }
                return oSuperReportShareViewModel;
            }



            return null;
        }
        public SuperReportShareViewModel GetPrescriptionReport(Guid UserId, Guid EprescriptionId)
        {
            SuperReportShareViewModel oSuperReportShareViewModel = new SuperReportShareViewModel();
            oSuperReportShareViewModel.ShareReportFeedBack = GetShareReportFeedBack(UserId, EprescriptionId);

            if (oSuperReportShareViewModel.ShareReportFeedBack != null)
            {

                UserShareRecordViewModels oUserShareRecordViewModels = _repository.GetShareReportDetail(oSuperReportShareViewModel.ShareReportFeedBack.UserRecordId);
                oSuperReportShareViewModel.ClinincName = oUserShareRecordViewModels.ClinicName;
                oSuperReportShareViewModel.DoctorName = oUserShareRecordViewModels.DoctorName;
                oSuperReportShareViewModel.Query = oUserShareRecordViewModels.Query;
                oSuperReportShareViewModel.QueryDateTime = oUserShareRecordViewModels.CreatedDate;
                oSuperReportShareViewModel.ResponseDateTime = oSuperReportShareViewModel.ShareReportFeedBack.CreatedDate;

                foreach (string item in oUserShareRecordViewModels.strChecks.Split(','))
                {
                    switch (int.Parse(item))
                    {
                        case ReportParameters.PersonalInformation:
                            oSuperReportShareViewModel.PersonalViewModel = GetPersonalInformation(UserId);
                            break;

                        case ReportParameters.EmergencyInformation:
                            oSuperReportShareViewModel.EmergencyViewModel = GetEmergencyInformation(UserId);
                            break;
                        case ReportParameters.EmployerInformation:
                            oSuperReportShareViewModel.EmployerViewModel = GetEmployerInformation(UserId);
                            break;
                        case ReportParameters.InsuranceInformation:
                            oSuperReportShareViewModel.InsuranceViewModel = GetInsuranceInformation(UserId);
                            break;
                        case ReportParameters.LegalInformation:
                            oSuperReportShareViewModel.LegalViewModel = GetLegalInformation(UserId);
                            break;
                        case ReportParameters.Preferences:
                            oSuperReportShareViewModel.PreferencesViewModel = _repository.GetPreferences(UserId);
                            break;
                        case ReportParameters.Allergies:
                            oSuperReportShareViewModel.AllergyViewModel = GetAllergiesExportableList(UserId);
                            break;
                        case ReportParameters.Problems:
                            oSuperReportShareViewModel.HealthConditionViewModel = GetHealthConditionExportableList(UserId);
                            break;
                        case ReportParameters.Medications:
                            oSuperReportShareViewModel.MedicationViewModel = GetMedicationExportableList(UserId);
                            break;
                        case ReportParameters.Immunizations:
                            oSuperReportShareViewModel.ImmunizationViewModel = GetlstImmunizationExportableList(UserId);
                            break;
                        case ReportParameters.Tests:
                            oSuperReportShareViewModel.LabTestViewModel = GetLabTestExportableList(UserId);
                            break;
                        case ReportParameters.Procedures:
                            oSuperReportShareViewModel.ProceduresViewModel = GetProceduresExportableList(UserId);
                            break;
                        case ReportParameters.Activities:
                            oSuperReportShareViewModel.ActivitiesViewModel = GetActivitiesExportableList(UserId);
                            break;
                        case ReportParameters.BP:
                            oSuperReportShareViewModel.BloodPressureAndPulseViewModel = GetBloodPressureAndPulseExportableList(UserId);
                            break;
                        case ReportParameters.Weight:
                            oSuperReportShareViewModel.WeightViewModel = GetWeightExportableList(UserId);
                            break;
                        case ReportParameters.Glucose:
                            oSuperReportShareViewModel.BloodGlucoseViewModel = GetBloodGlucoseRecordsist(UserId);
                            break;

                        default:
                            break;
                    }
                }
            }

            return oSuperReportShareViewModel;

        }
        public bool SaveFeedBack(ShareFeedBack oShareFeedBack, out string DoctorName)
        {
            return _repository.SaveFeedBack(oShareFeedBack, out DoctorName);
        }

        public UserShareRecordViewModels GetUserId(string Password)
        {

            return _repository.GetUserId(Password);
        }


        public Dictionary<string, Dictionary<string, string>> GetCompleteDictionary(string lst, Guid userId)
        {
            Dictionary<string, Dictionary<string, string>> dictProfile = null;
            try
            {
                dictProfile = new Dictionary<string, Dictionary<string, string>>();
                List<int> lstRptOptions = lst.Split(',').Select(Int32.Parse).ToList();

                foreach (int item in lstRptOptions)
                {
                    switch (item)
                    {
                        case ReportParameters.PersonalInformation:
                            dictProfile.Add("Personal Information", GetInfoDictionary(GetPersonalInformation(userId)));
                            break;
                        case ReportParameters.EmergencyInformation:
                            dictProfile.Add("Emergency Information", GetInfoDictionary(GetEmergencyInformation(userId)));
                            break;
                        case ReportParameters.EmployerInformation:
                            dictProfile.Add("Employer Information", GetInfoDictionary(GetEmployerInformation(userId)));
                            break;
                        case ReportParameters.InsuranceInformation:
                            dictProfile.Add("Insurance Information", GetInfoDictionary(GetInsuranceInformation(userId)));
                            break;
                        case ReportParameters.LegalInformation:
                            dictProfile.Add("Legal Information", GetInfoDictionary(GetLegalInformation(userId)));
                            break;
                        case ReportParameters.Preferences:
                            dictProfile.Add("Preferences & Special Needs", GetInfoDictionary(_repository.GetPreferences(userId)));
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return dictProfile;
        }

        public Dictionary<string, List<ReportColumn>> GetDisplayColumnsDictionary(string lst)
        {
            Dictionary<string, List<ReportColumn>> dictDisplayColumns = null;
            List<ReportColumn> columns = null;
            try
            {
                dictDisplayColumns = new Dictionary<string, List<ReportColumn>>();
                List<int> lstRptOptions = lst.Split(',').Select(Int32.Parse).Where(m => m > ReportParameters.Preferences).ToList();

                foreach (int item in lstRptOptions)
                {
                    switch (item)
                    {
                        case ReportParameters.Allergies:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Allergy Name", Width = 200 });
                            columns.Add(new ReportColumn { ColumnName = "From", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Still Have?", Width = 50 });
                            dictDisplayColumns.Add("Allergies", columns);
                            columns = null;
                            break;
                        case ReportParameters.Problems:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Condition Name", Width = 200 });
                            columns.Add(new ReportColumn { ColumnName = "Diag. Date", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Service Date", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Still Have Condition?", Width = 50 });
                            dictDisplayColumns.Add("Problems", columns);
                            columns = null;
                            break;
                        case ReportParameters.Medications:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Medication Name", Width = 200 });
                            columns.Add(new ReportColumn { ColumnName = "Prescribed Date", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Strength", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Still Taking Medication?", Width = 50 });
                            dictDisplayColumns.Add("Medications", columns);
                            columns = null;
                            break;
                        case ReportParameters.Immunizations:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Immunization Name", Width = 200 });
                            columns.Add(new ReportColumn { ColumnName = "Taken On", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Comments", Width = 100 });
                            dictDisplayColumns.Add("Immunizations", columns);
                            columns = null;
                            break;
                        case ReportParameters.Tests:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Test Name", Width = 200 });
                            columns.Add(new ReportColumn { ColumnName = "Perf. Date", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Result", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Unit", Width = 50 });
                            dictDisplayColumns.Add("Labs/Tests", columns);
                            columns = null;
                            break;
                        case ReportParameters.Procedures:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Procedure Name", Width = 200 });
                            columns.Add(new ReportColumn { ColumnName = "Date of Procedure", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Diagnosed by Doctor/Hospital", Width = 100 });
                            dictDisplayColumns.Add("Procedures", columns);
                            columns = null;
                            break;
                        case ReportParameters.BP:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Systolic", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Diastolic", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Pulse", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Comments", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Col. Date", Width = 50 });
                            dictDisplayColumns.Add("Blood Pressure", columns);
                            columns = null;
                            break;
                        case ReportParameters.Weight:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Weight", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Height", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Comments", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Col. Date", Width = 50 });
                            dictDisplayColumns.Add("Weight", columns);
                            columns = null;
                            break;
                        case ReportParameters.Glucose:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Result", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Value Type", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Comments", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Col. Date", Width = 50 });
                            dictDisplayColumns.Add("Blood Glucose", columns);
                            columns = null;
                            break;
                        case ReportParameters.Activities:
                            columns = new List<ReportColumn>();
                            columns.Add(new ReportColumn { ColumnName = "Sno", Width = 20 });
                            columns.Add(new ReportColumn { ColumnName = "Activity Name", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Path Name", Width = 100 });
                            columns.Add(new ReportColumn { ColumnName = "Distance", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Total Time", Width = 50 });
                            columns.Add(new ReportColumn { ColumnName = "Col. Date", Width = 50 });
                            dictDisplayColumns.Add("Activities", columns);
                            columns = null;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return dictDisplayColumns;
        }

        public Dictionary<string, List<DataRow>> GetDataRowDictionary(string lst, Guid userId)
        {
            Dictionary<string, List<DataRow>> dictDataRow = null;
            List<DataRow> lstDataRow = null;
            try
            {
                dictDataRow = new Dictionary<string, List<DataRow>>();
                List<int> lstRptOptions = lst.Split(',').Select(Int32.Parse).Where(m => m > ReportParameters.Preferences).ToList();

                foreach (int item in lstRptOptions)
                {
                    switch (item)
                    {
                        case ReportParameters.Allergies:
                            lstDataRow = GetAllergiesTable(userId);
                            dictDataRow.Add("Allergies", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Problems:
                            lstDataRow = GetHealthConditionsTable(userId);
                            dictDataRow.Add("Problems", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Medications:
                            lstDataRow = GetMedicationsTable(userId);
                            dictDataRow.Add("Medications", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Immunizations:
                            lstDataRow = GetImmunizationsTable(userId);
                            dictDataRow.Add("Immunizations", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Tests:
                            lstDataRow = GetLabTestTable(userId);
                            dictDataRow.Add("Labs/Tests", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Procedures:
                            lstDataRow = GetProceduresTable(userId);
                            dictDataRow.Add("Procedures", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Activities:
                            lstDataRow = GetActivitiesTable(userId);
                            dictDataRow.Add("Activities", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.BP:
                            lstDataRow = GetBloodPressureTable(userId);
                            dictDataRow.Add("Blood Pressure", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Weight:
                            lstDataRow = GetWeightTable(userId);
                            dictDataRow.Add("Weight", lstDataRow);
                            lstDataRow = null;
                            break;
                        case ReportParameters.Glucose:
                            lstDataRow = GetBloodGlucoseTable(userId);
                            dictDataRow.Add("Blood Glucose", lstDataRow);
                            lstDataRow = null;
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return dictDataRow;
        }





    }
}
