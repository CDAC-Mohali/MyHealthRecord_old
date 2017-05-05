using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PHRMS.Data.DataAccess;
using PHRMS.ViewModels;
using System.Reflection;

namespace PHRMS.BLL
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public partial class CatalogService
    {
        IPHRMSRepo _repository = null;

        /// <summary>
        /// Create a Catalog Service based on the passed-in repository
        /// </summary>
        /// <param name="repository">An IPHRMSRepo</param>
        public CatalogService(IPHRMSRepo repository)
        {
            _repository = repository;
            if (_repository == null)
            {
                throw new InvalidOperationException("Catalog Repository cannot be null");
            }
        }

        public async Task<PersonalViewModel> SavePersonalInformation(PersonalViewModel oPersonalViewModel)
        {
            try
            {
                if (oPersonalViewModel.strDOB != null && oPersonalViewModel.strDOB != "")
                {
                    oPersonalViewModel.DOB = DateTime.ParseExact(oPersonalViewModel.strDOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                oPersonalViewModel = await _repository.PersonalInfoUpdate(oPersonalViewModel);
                if (oPersonalViewModel != null)
                {
                    oPersonalViewModel.strState = _repository.GetStateNameById(oPersonalViewModel.State);
                    oPersonalViewModel.strBloodType = _repository.GetBloodGroupById(oPersonalViewModel.BloodType);
                    oPersonalViewModel.strDiffAbled = oPersonalViewModel.DiffAbled ? "Yes - " + _repository.GetDisabilityTypeById(oPersonalViewModel.DiffAbledType) : "No";
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                    oPersonalViewModel.strDOB = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) : "";
                    oPersonalViewModel.strDOBwAge = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) + " (Age: " + CommonComponent.CalculateAge(oPersonalViewModel.DOB) + ")" : "";
                    oPersonalViewModel.strGender = GetGenderString(oPersonalViewModel.Gender);
                    oPersonalViewModel.DyCertPath = _repository.GetSavedFilePath(oPersonalViewModel.UserId, FileType.DisablityCert);
                }
                return oPersonalViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<EmergencyViewModel> SaveEmergencyInformation(EmergencyViewModel oEmergencyViewModel)
        {
            try
            {
                oEmergencyViewModel = await _repository.EmergencyInfoUpdate(oEmergencyViewModel);
                if (oEmergencyViewModel != null)
                {
                    oEmergencyViewModel.strPC_Relationship = _repository.GetRelationNameById(oEmergencyViewModel.PC_Relationship);
                    oEmergencyViewModel.strSC_State = _repository.GetStateNameById(oEmergencyViewModel.SC_State);
                    oEmergencyViewModel.strPC_State = _repository.GetStateNameById(oEmergencyViewModel.PC_State);
                }
                return oEmergencyViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<EmployerViewModel> SaveEmployerInformation(EmployerViewModel oEmployerViewModel)
        {
            try
            {
                if (oEmployerViewModel != null)
                {
                    oEmployerViewModel.strState = _repository.GetStateNameById(oEmployerViewModel.EmpState);
                }

                return await _repository.EmployerInfoUpdate(oEmployerViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<InsuranceViewModel> SaveInsuranceInformation(InsuranceViewModel oInsuranceViewModel)
        {
            try
            {
                if (oInsuranceViewModel.strValidTill != null && oInsuranceViewModel.strValidTill != "")
                {
                    oInsuranceViewModel.ValidTill = DateTime.ParseExact(oInsuranceViewModel.strValidTill, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                oInsuranceViewModel.strValidTill = (oInsuranceViewModel.ValidTill != null && oInsuranceViewModel.ValidTill != DateTime.MinValue) ? oInsuranceViewModel.ValidTill.ToString("dd/MM/yyyy", culture) : "";
                return await _repository.InsuranceInfoUpdate(oInsuranceViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LegalViewModel> SaveLegalInformation(LegalViewModel oLegalViewModel)
        {
            try
            {
                if (oLegalViewModel.strPA_DateInit != null && oLegalViewModel.strPA_DateInit != "")
                {
                    oLegalViewModel.PA_DateInit = DateTime.ParseExact(oLegalViewModel.strPA_DateInit, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (oLegalViewModel.strCD_DateInit != null && oLegalViewModel.strCD_DateInit != "")
                {
                    oLegalViewModel.CD_DateInit = DateTime.ParseExact(oLegalViewModel.strCD_DateInit, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                if (oLegalViewModel.strLW_DateInit != null && oLegalViewModel.strLW_DateInit != "")
                {
                    oLegalViewModel.LW_DateInit = DateTime.ParseExact(oLegalViewModel.strLW_DateInit, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                oLegalViewModel = await _repository.LegalInfoUpdate(oLegalViewModel);
                if (oLegalViewModel != null)
                {
                    oLegalViewModel.strPower_Attorney = GetYesorNo(oLegalViewModel.Power_Attorney);
                    oLegalViewModel.strLiving_Will = GetYesorNo(oLegalViewModel.Living_Will);
                    oLegalViewModel.strCare_Directive = GetYesorNo(oLegalViewModel.Care_Directive);
                }
                return await _repository.LegalInfoUpdate(oLegalViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PreferencesViewModel> SavePreferencesInformation(PreferencesViewModel oPreferencesViewModel)
        {
            try
            {
                return await _repository.PreferencesInfoUpdate(oPreferencesViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Get Aggregated profile Details.
        /// </summary>
        /// <param name="userId">Guid of the user currently logged in.</param>
        /// <returns>Object of type ProfileViewModel</returns>
        public ProfileViewModel GetProfileDetails(Guid userId)
        {
            ProfileViewModel oProfileViewModel = null;
            try
            {
                oProfileViewModel = new ProfileViewModel();
                var vEmergencyInformation = GetEmergencyInformation(userId);
                oProfileViewModel.oEmergencyInformation = vEmergencyInformation != null ? vEmergencyInformation : oProfileViewModel.oEmergencyInformation;
                var vPersonalInformation = GetPersonalInformation(userId);
                oProfileViewModel.oPersonalInformation = vPersonalInformation != null ? vPersonalInformation : oProfileViewModel.oPersonalInformation;
                var vEmployerInformation = GetEmployerInformation(userId);
                oProfileViewModel.oEmployerInformation = vEmployerInformation != null ? vEmployerInformation : oProfileViewModel.oEmployerInformation;
                var vInsuranceInformation = GetInsuranceInformation(userId);
                oProfileViewModel.oInsuranceInformation = vInsuranceInformation != null ? vInsuranceInformation : oProfileViewModel.oInsuranceInformation;
                var vLegalInformation = GetLegalInformation(userId);
                oProfileViewModel.oLegalInformation = vLegalInformation != null ? vLegalInformation : oProfileViewModel.oLegalInformation;
                var vPreferences = _repository.GetPreferences(userId);
                oProfileViewModel.oPreferences = vPreferences != null ? vPreferences : oProfileViewModel.oPreferences;
            }
            catch (Exception)
            {
            }
            return oProfileViewModel;
        }

        public PersonalViewModel GetPersonalInformation(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                oPersonalViewModel = _repository.GetPersonalInformation(userId);
                if (oPersonalViewModel != null)
                {
                    oPersonalViewModel.strState = _repository.GetStateNameById(oPersonalViewModel.State);
                    oPersonalViewModel.strBloodType = _repository.GetBloodGroupById(oPersonalViewModel.BloodType);
                    oPersonalViewModel.strDiffAbled = oPersonalViewModel.DiffAbled ? "Yes - " + _repository.GetDisabilityTypeById(oPersonalViewModel.DiffAbledType) : "No";
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                    oPersonalViewModel.strDOB = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) : "";
                    oPersonalViewModel.strDOBwAge = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) + " (Age: " + CommonComponent.CalculateAge(oPersonalViewModel.DOB) + ")" : "";
                    oPersonalViewModel.strGender = GetGenderString(oPersonalViewModel.Gender);
                    oPersonalViewModel.DyCertPath = _repository.GetSavedFilePath(oPersonalViewModel.UserId, FileType.DisablityCert);
                }
            }
            catch (Exception)
            {
            }
            return oPersonalViewModel;
        }

        public Dictionary<string, string> GetBasicPersonalInfoDictionary(Guid userId)
        {
            Dictionary<string, string> dictInfo = null;
            try
            {
                dictInfo = new Dictionary<string, string>();
                var data = GetPersonalInformation(userId);
                if (data != null)
                {
                    dictInfo.Add("Name", data.FirstName + " " + data.LastName);
                    dictInfo.Add("Email", data.Email);
                    dictInfo.Add("Phone", data.Cell_Phone);
                    dictInfo.Add("D.O.B", data.strDOB);
                    dictInfo.Add("Gender", data.strGender);
                    dictInfo.Add("Blood Type", data.strBloodType);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return dictInfo;
        }

        /// <summary>
        /// This method is used to generate dictionary object from object model.
        /// </summary>
        /// <param name="obj">object model</param>
        /// <returns>Dictionary object</returns>
        public Dictionary<string, string> GetInfoDictionary(object obj)
        {
            Dictionary<string, string> dictInfo = null;
            try
            {
                //var vdictInfo = result.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => prop.GetValue(result, null));
                dictInfo = new Dictionary<string, string>();
                foreach (var pi in obj.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(string))
                    {
                        string str = DisplayNameHelper.GetDisplayName(pi);
                        if (!string.IsNullOrEmpty(str))
                        {
                            dictInfo.Add(str, (pi.GetValue(obj, null) != null ? pi.GetValue(obj, null).ToString() : ""));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dictInfo;
        }


        //Newly Added method for Employer Information
        public EmployerViewModel GetEmployerInformation(Guid userId)
        {
            EmployerViewModel oEmployerViewModel = null;
            try
            {
                oEmployerViewModel = _repository.GetEmployerInformation(userId);
                if (oEmployerViewModel != null)
                {
                    oEmployerViewModel.strState = _repository.GetStateNameById(oEmployerViewModel.EmpState);

                }
            }
            catch (Exception)
            {
            }
            return oEmployerViewModel;
        }


        //Newly Added method for Employer Information
        public InsuranceViewModel GetInsuranceInformation(Guid userId)
        {
            InsuranceViewModel oInsuranceViewModel = null;
            try
            {
                oInsuranceViewModel = _repository.GetInsuranceInformation(userId);
                if (oInsuranceViewModel != null)
                {
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                    oInsuranceViewModel.strValidTill = (oInsuranceViewModel.ValidTill != null && oInsuranceViewModel.ValidTill != DateTime.MinValue) ? oInsuranceViewModel.ValidTill.ToString("dd/MM/yyyy", culture) : "";
                }
            }
            catch (Exception)
            {
            }
            return oInsuranceViewModel;
        }



        public EmergencyViewModel GetEmergencyInformation(Guid userId)
        {
            EmergencyViewModel oEmergencyViewModel = null;
            try
            {
                oEmergencyViewModel = _repository.GetEmergencyInformation(userId);
                if (oEmergencyViewModel != null)
                {
                    oEmergencyViewModel.strPC_Relationship = _repository.GetRelationNameById(oEmergencyViewModel.PC_Relationship);
                    oEmergencyViewModel.strPC_State = _repository.GetStateNameById(oEmergencyViewModel.PC_State);
                    oEmergencyViewModel.strSC_State = _repository.GetStateNameById(oEmergencyViewModel.SC_State);
                }
            }
            catch (Exception)
            {
            }
            return oEmergencyViewModel;
        }

        public LegalViewModel GetLegalInformation(Guid userId)
        {
            LegalViewModel oLegalViewModel = null;
            try
            {
                oLegalViewModel = _repository.GetLegalInformation(userId);
                if (oLegalViewModel != null)
                {
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                    oLegalViewModel.strPA_DateInit = (oLegalViewModel.PA_DateInit != null && oLegalViewModel.PA_DateInit != DateTime.MinValue) ? oLegalViewModel.PA_DateInit.ToString("dd/MM/yyyy", culture) : "";
                    oLegalViewModel.strCD_DateInit = (oLegalViewModel.CD_DateInit != null && oLegalViewModel.CD_DateInit != DateTime.MinValue) ? oLegalViewModel.CD_DateInit.ToString("dd/MM/yyyy", culture) : "";
                    oLegalViewModel.strLW_DateInit = (oLegalViewModel.LW_DateInit != null && oLegalViewModel.LW_DateInit != DateTime.MinValue) ? oLegalViewModel.LW_DateInit.ToString("dd/MM/yyyy", culture) : "";
                    oLegalViewModel.strPower_Attorney = GetYesorNo(oLegalViewModel.Power_Attorney);
                    oLegalViewModel.strLiving_Will = GetYesorNo(oLegalViewModel.Living_Will);
                    oLegalViewModel.strCare_Directive = GetYesorNo(oLegalViewModel.Care_Directive);
                }
            }
            catch (Exception)
            {
            }
            return oLegalViewModel;
        }

        private string GetGenderString(string chGender)
        {
            return chGender == "M" ? "Male" : (chGender == "F" ? "Female" : "Do Not Specify");
        }

        //27 MARCH 2017
        public ProfileViewModel GetPatientDetails(Guid userId)
        {
            ProfileViewModel oProfileViewModel = null;
            try
            {
                oProfileViewModel = new ProfileViewModel();
                var vPersonalInformation = GetPatientInformation(userId);
                oProfileViewModel.oPersonalInformation = vPersonalInformation != null ? vPersonalInformation : oProfileViewModel.oPersonalInformation;
            }
            catch (Exception)
            {
            }
            return oProfileViewModel;
        }

        public PersonalViewModel GetPatientInformation(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                oPersonalViewModel = _repository.GetPatientInformation(userId);
                if (oPersonalViewModel != null)
                {
                    //oPersonalViewModel.strState = _repository.GetStateNameById(oPersonalViewModel.State);
                    //oPersonalViewModel.strBloodType = _repository.GetBloodGroupById(oPersonalViewModel.BloodType);
                    //oPersonalViewModel.strDiffAbled = oPersonalViewModel.DiffAbled ? "Yes - " + _repository.GetDisabilityTypeById(oPersonalViewModel.DiffAbledType) : "No";
                    //System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                    //oPersonalViewModel.strDOB = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) : "";
                    //oPersonalViewModel.strDOBwAge = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) + " (Age: " + CommonComponent.CalculateAge(oPersonalViewModel.DOB) + ")" : "";
                    //oPersonalViewModel.strGender = GetGenderString(oPersonalViewModel.Gender);
                    //oPersonalViewModel.DyCertPath = _repository.GetSavedFilePath(oPersonalViewModel.UserId, FileType.DisablityCert);
                    
                }
            }
            catch (Exception)
            {
            }
            return oPersonalViewModel;
        }
    }
}
