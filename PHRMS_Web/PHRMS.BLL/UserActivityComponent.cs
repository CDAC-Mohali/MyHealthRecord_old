using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using PHRMS.Data;
using System.Linq;
using Microsoft.AspNet.Mvc.Rendering;


namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        static List<SelectListItem> ContactUSStatesList;

        public IEnumerable<SelectListItem> GetContactUSStatesList()
        {
            if (ContactUSStatesList == null)
            {
                ContactUSStatesList = _repository.GetStatesList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                ContactUSStatesList.Insert(0, new SelectListItem { Text = "---Select State---", Value = "0" });
            }

            return ContactUSStatesList;
        }


        public bool AddUserActivity(UserActivityViewModels oUserActivityViewModels)
        {
            bool result = false;
            try
            {
                if (oUserActivityViewModels != null)
                {
                    if (oUserActivityViewModels.strTimeStamp != "")
                        oUserActivityViewModels.TimeStamp = DateTime.ParseExact(oUserActivityViewModels.strTimeStamp, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    result = _repository.AddUserActivity(oUserActivityViewModels);
                }

            }
            catch (Exception e)
            {

            }

            return result;
        }


        public bool AddContactUs(ContactusViewModel ContactusViewModel)
        {
            bool _result = false;
            try
            {
                if (ContactusViewModel != null)
                {
                    _repository.AddContactUs(ContactusViewModel);
                }
            }
            catch (Exception)
            {
            }

            return _result;
        }


        public bool ChangesPassowrd(Guid UserId, string NewPassword, string OldPassword)
        {
            bool _result = false;
            string OldPasswordHash, NewPasswordHash;
            try
            {
                CommonComponent _comconnect = new CommonComponent();
                NewPasswordHash = "";
                OldPasswordHash = "";

                if (!string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(OldPassword))
                {
                    _result=_repository.ChangePassword(UserId, NewPasswordHash, OldPasswordHash);

                }
            }
            catch (Exception)
            {
                _result = false;
            }

            return _result;
        }


        public string AddUserShareRecordEntry(Guid UserId,string strChecks,int ValidUpto,string DocEmail,string DocPhone, string query)
        {
            string result = "";
            try
            {
                UserShareRecordViewModels oUserShareRecordViewModels = new UserShareRecordViewModels();
                oUserShareRecordViewModels.UserId = UserId;
                Random randomclass = new Random();
                oUserShareRecordViewModels.Password = randomclass.Next(100000, 999999).ToString();
                oUserShareRecordViewModels.strChecks = strChecks;
                oUserShareRecordViewModels.ValidUpto = ValidUpto;
                oUserShareRecordViewModels.CreatedDate = DateTime.Now;
                oUserShareRecordViewModels.DocEmail = DocEmail;
                oUserShareRecordViewModels.DocPhone = DocPhone;
                oUserShareRecordViewModels.Query = query;
                if (_repository.AddUserShareRecordEntry(oUserShareRecordViewModels))
                    result = oUserShareRecordViewModels.Password;


            }
            catch (Exception e)
            {

            }

            return result;
        }




        public List<UserActivityViewModels> GetUserActivityExportableList(Guid Id, int count)
        {
            List<UserActivityViewModels> lstMedicationViewModel = null;
            Users userDetails = null;
            try
            {
                lstMedicationViewModel = _repository.GeUserActivityPartialList(Id, count);
                userDetails = _repository.GetUserDetailsForOpenEMR(Id);
                string uname = userDetails.FirstName + " " + userDetails.LastName;
                if (lstMedicationViewModel != null)
                {
                    foreach (var item in lstMedicationViewModel)
                    {
                        item.strTimeStamp = (item.TimeStamp != null && item.TimeStamp != DateTime.MinValue) ? item.TimeStamp.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.ActivityName = _repository.GetActivityNameById(item.ActivityId, item.Module);
                        item.UserName = uname;
                        item.strModuleName = GetModuleName(item.Module);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstMedicationViewModel;
        }

        public string GetModuleName(int module)
        {
            string result = "unknown";
            switch (module)
            {
                case 1:
                    result = "Allergies";
                    break;
                case 2:
                    result = "Immunization";
                    break;
                case 3:
                    result = "Lab";
                    break;
                case 4:
                    result = "Medication";
                    break;
                case 5:
                    result = "Procedures";
                    break;
                case 6:
                    result = "HealthCondition";
                    break;
                case 7:
                    result = "Activity";
                    break;
                case 8:
                    result = "Blood Pressure";
                    break;
                case 9:
                    result = "Blood Glucose";
                    break;
                case 10:
                    result = "Sleep";
                    break;
                case 11:
                    result = "Weight";
                    break;
                case 12:
                    result = "Profile Picture";
                    break;
                case 13:
                    result = "Prescription";
                    break;
                case 14:
                    result = "Temperature";

                    break;
                default:
                    result = "Unknown";
                    break;

            }
            return result;
        }

    }
}
