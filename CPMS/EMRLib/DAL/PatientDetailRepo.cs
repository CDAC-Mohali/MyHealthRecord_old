using AutoMapper;
using EMRLib.DataModels;
using EMRViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMRLib.DAL
{
    public sealed class FileDirPaths
    {
        public static readonly string DyCertPath = @"Images/DisabilityCert/";
        public static readonly string LabReportPath = @"Images/DisabilityCert/";
    }
    public partial class EMRRepository : IEMRRepository
    {

        public string AddPHRMSOTPShare(Guid UserId, int Type)
        {

            try
            {
                PatientDetailOTP oPatientDetailOTP = new PatientDetailOTP();
                oPatientDetailOTP.PHRMSUserId = UserId;
                Random randomclass = new Random();
                oPatientDetailOTP.OTP = randomclass.Next(100000, 999999).ToString();
                //oPatientDetailOTP.ValidUpto = ValidUpto;
                oPatientDetailOTP.CreatedDate = DateTime.Now;
                oPatientDetailOTP.Type = Type;
                context.PatientDetailOTP.Add(oPatientDetailOTP);
                context.SaveChanges();
                return oPatientDetailOTP.OTP;
            }
            catch (Exception e)
            {

            }

            return "";
        }
        public bool CheckPHRMSOTPShare(Guid PHRMSUserId, string OTP, int Type)
        {
            try
            {
                PatientDetailOTP oPatientDetailOTP = context.PatientDetailOTP.Where(s => s.PHRMSUserId == PHRMSUserId && s.OTP == OTP && s.Type == Type ).FirstOrDefault();
                if (oPatientDetailOTP != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }

            return false; ;
        }
        public PersonalViewModel GetPersonalInformation(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                var record = context.PersonalInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<PersonalInformation, PersonalViewModel>();
                    oPersonalViewModel = Mapper.Map<PersonalViewModel>(record);
                }
                else
                    oPersonalViewModel = GetInitialPersonalDetails(userId);

                //if (oPersonalViewModel != null)
                //{
                //    oPersonalViewModel.strState = GetStateNameById(oPersonalViewModel.State);
                //    oPersonalViewModel.strBloodType = GetBloodGroupById(oPersonalViewModel.BloodType);
                //    //    oPersonalViewModel.strDiffAbled = oPersonalViewModel.DiffAbled ? "Yes - " + GetDisabilityTypeById(oPersonalViewModel.DiffAbledType) : "No";
                //    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("pt-BR");
                //    oPersonalViewModel.strDOB = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) : "";
                //    oPersonalViewModel.strDOBwAge = (oPersonalViewModel.DOB != null && oPersonalViewModel.DOB != DateTime.MinValue) ? oPersonalViewModel.DOB.ToString("dd/MM/yyyy", culture) + " (Age: " + CalculateAge(oPersonalViewModel.DOB) + ")" : "";
                //    oPersonalViewModel.strGender = GetGenderString(oPersonalViewModel.Gender);
                //    oPersonalViewModel.DyCertPath = GetSavedFilePath(oPersonalViewModel.UserId, EMRViewModels.FileType.DisablityCert);
                //}
            }
            catch (Exception ex)
            {

            }
            return oPersonalViewModel;
        }
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
        public string GetSavedFilePath(Guid Id, EMRViewModels.FileType type)
        {
            string strPath = "";
            try
            {
                var rec = context.FilePath.OrderByDescending(m => m.CreatedDate).Where(m => m.UserId.Equals(Id) && !m.DeleteFlag).FirstOrDefault();
                if (rec != null)
                {
                    strPath = FileDirPaths.DyCertPath + rec.FileName;
                    rec = null;
                }
            }
            catch (Exception)
            {
            }
            return strPath;
        }
        private string GetGenderString(string chGender)
        {
            return chGender == "M" ? "Male" : (chGender == "F" ? "Female" : "Do Not Specify");
        }

        public string GetBloodGroupById(int Id)
        {
            string strBloodgp = "";
            try
            {
                if (Id == 0)
                    strBloodgp = "Do Not Specify";
                else
                    strBloodgp = context.BloodGroups.FirstOrDefault(m => m.Id == Id).Name;
            }
            catch (Exception)
            {
            }
            return strBloodgp;
        }

        public string GetDisabilityTypeById(int Id)
        {
            string strDisabilityTypes = "";
            try
            {
                if (Id == 0)
                    strDisabilityTypes = "Do Not Specify";
                else
                    strDisabilityTypes = context.DisabilityTypes.FirstOrDefault(m => m.Id == Id).Name;
            }
            catch (Exception)
            {
            }
            return strDisabilityTypes;
        }

        public string GetStateNameById(int Id)
        {
            string strStates = "";
            try
            {
                if (Id == 0)
                    strStates = "Not Available";
                else
                    strStates = context.States.FirstOrDefault(m => m.Id == Id).Name;
            }
            catch (Exception)
            {
            }
            return strStates;
        }

        public string GetRelationNameById(int Id)
        {
            string strRelation = "";
            try
            {
                if (Id == 0)
                    strRelation = "Not Available";
                else
                    strRelation = context.Relationship.FirstOrDefault(m => m.Id == Id).Relation;
            }
            catch (Exception)
            {
            }
            return strRelation;
        }
        public PersonalViewModel GetInitialPersonalDetails(Guid userId)
        {
            PersonalViewModel oPersonalViewModel = null;
            try
            {
                var user = context.Users.FirstOrDefault(m => m.UserId.Equals(userId));
                if (user != null)
                {
                    oPersonalViewModel = new PersonalViewModel();
                    oPersonalViewModel.FirstName = user.FirstName;
                    oPersonalViewModel.LastName = user.LastName;
                    oPersonalViewModel.Uhid = user.AadhaarNo;
                    oPersonalViewModel.Cell_Phone = user.MobileNo;
                    oPersonalViewModel.Email = user.Email;
                    user = null;
                }
            }
            catch (Exception)
            {

            }
            return oPersonalViewModel;
        }

        public EmergencyViewModel GetEmergencyInformation(Guid userId)
        {
            EmergencyViewModel oEmergencyViewModel = null;
            try
            {
                var record = context.EmergencyInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<EmergencyInformation, EmergencyViewModel>();
                    oEmergencyViewModel = Mapper.Map<EmergencyViewModel>(record);
                }
            }
            catch (Exception)
            {

            }
            return oEmergencyViewModel;
        }

        public EmployerViewModel GetInitialEmployerDetails(Guid userId)
        {
            EmployerViewModel oEmployerViewModel = null;
            try
            {
                oEmployerViewModel = new EmployerViewModel();
                var user = context.Users.FirstOrDefault(m => m.UserId.Equals(userId));
                if (user != null)
                {
                    oEmployerViewModel.CUG = user.CUG;
                    user = null;
                }

            }
            catch (Exception)
            {

            }
            return oEmployerViewModel;
        }

        public EmployerViewModel GetEmployerInformation(Guid userId)
        {
            EmployerViewModel oEmployerViewModel = null;
            try
            {
                var record = context.EmployerInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<EmployerInformation, EmployerViewModel>();
                    oEmployerViewModel = Mapper.Map<EmployerViewModel>(record);
                }
                else
                    oEmployerViewModel = GetInitialEmployerDetails(userId);
            }
            catch (Exception)
            {

            }
            return oEmployerViewModel;
        }

        public InsuranceViewModel GetInsuranceInformation(Guid userId)
        {
            InsuranceViewModel oInsuranceViewModel = null;
            try
            {
                var record = context.InsuranceInformation.FirstOrDefault(m => m.UserId.Equals(userId));

                if (record != null)
                {
                    Mapper.CreateMap<InsuranceInformation, InsuranceViewModel>();
                    oInsuranceViewModel = Mapper.Map<InsuranceViewModel>(record);
                }

            }
            catch (Exception)
            {

            }
            return oInsuranceViewModel;
        }

        public LegalViewModel GetLegalInformation(Guid userId)
        {
            LegalViewModel oLegalViewModel = null;
            try
            {
                var record = context.LegalInformation.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<LegalInformation, LegalViewModel>();
                    oLegalViewModel = Mapper.Map<LegalViewModel>(record);
                }

            }
            catch (Exception)
            {

            }
            return oLegalViewModel;
        }

        public PreferencesViewModel GetPreferences(Guid userId)
        {
            PreferencesViewModel oPreferencesViewModel = null;
            try
            {
                var record = context.Preferences.FirstOrDefault(m => m.UserId.Equals(userId));
                if (record != null)
                {
                    Mapper.CreateMap<Preferences, PreferencesViewModel>();
                    oPreferencesViewModel = Mapper.Map<PreferencesViewModel>(record);
                }

            }
            catch (Exception)
            {

            }
            return oPreferencesViewModel;
        }
        public string GetSeverityById(int Id)
        {
            try
            {
                var res = context.AllergySeverity.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Severity;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetSeverityNameById(int Id)
        {
            try
            {
                var res = context.AllergySeverity.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Severity;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetAllergyDurationById(int Id)
        {
            try
            {
                var res = context.AllergyDuration.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Duration;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public string GetAllergyNameById(int Id)
        {
            try
            {
                var res = context.AllergyMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.AllergyName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public string GetYesorNo(bool val)
        {
            return val ? "Yes" : "No";
        }
        public List<AllergyViewModel> GetAllergiesCompleteList(Guid Id)
        {
            try
            {
                var list = context.Allergies.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Allergies, AllergyViewModel>();
                List<AllergyViewModel> lstAllergyViewModel = Mapper.Map<List<Allergies>, List<AllergyViewModel>>(list);
                foreach (var item in lstAllergyViewModel)
                {
                    item.strStill_Have = item.Still_Have ? "Yes" : "No";
                    item.strDuration = GetAllergyDurationById(item.DurationId);
                    //item.strStartDate = (item.StartDate != null && item.StartDate != DateTime.MinValue) ? item.StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    //item.strEndDate = (item.EndDate != null && item.EndDate != DateTime.MinValue) ? item.EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    item.strSeverity = GetSeverityById(item.Severity);
                    item.AllergyName = GetAllergyNameById(item.AllergyType);
                    // item.strDuration = context.GetAllergyDurationById(item.DurationId);
                }
                list = null;
                return lstAllergyViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<HealthConditionViewModel> GetHealthConditionExportableList(Guid Id)
        {
            try
            {
                var list = context.HealthCondition.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<HealthCondition, HealthConditionViewModel>();
                List<HealthConditionViewModel> lstHealthConditionViewModel = Mapper.Map<List<HealthCondition>, List<HealthConditionViewModel>>(list);
                if (lstHealthConditionViewModel != null)
                {
                    foreach (var item in lstHealthConditionViewModel)
                    {
                        item.strStillHaveCondition = item.StillHaveCondition ? "Yes" : "No";
                        item.strDiagnosisDate = (item.DiagnosisDate != null && item.DiagnosisDate != DateTime.MinValue) ? item.DiagnosisDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strServiceDate = (item.ServiceDate != null && item.ServiceDate != DateTime.MinValue) ? item.ServiceDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.HealthCondition = GetHealthConditionNameById(item.ConditionType);
                    }
                }
                list = null;
                return lstHealthConditionViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetHealthConditionNameById(int Id)
        {
            try
            {
                var res = context.HealthConditionMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.HealthCondition;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public List<MedicationViewModel> GetMedicationCompleteList(Guid Id)
        {
            try
            {
                var list = context.Medication.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Medication, MedicationViewModel>();
                List<MedicationViewModel> lstMedicationViewModel = Mapper.Map<List<Medication>, List<MedicationViewModel>>(list);
                if (lstMedicationViewModel != null)
                {
                    foreach (var item in lstMedicationViewModel)
                    {
                        item.strTakingMedicine = item.TakingMedicine ? "Yes" : "No";
                        item.strPrescribedDate = (item.PrescribedDate != null && item.PrescribedDate != DateTime.MinValue) ? item.PrescribedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strDispensedDate = (item.DispensedDate != null && item.DispensedDate != DateTime.MinValue) ? item.DispensedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.MedicineName = GetMedicineNameById(item.MedicineType);
                        item.strRoute = GetRouteById(item.Route);
                        item.strDosValue = GetDosageValueById(item.DosValue);
                        item.strDosUnit = GetDosageUnitById(item.DosUnit);
                        item.strFrequency = GetFrequencyById(item.Frequency);
                        item.lstFileModels = GetAllAttachments(EMRViewModels.FileType.Medication, item.Id);

                    }
                }
                list = null;
                return lstMedicationViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string GetMedicineNameById(int Id)
        {
            try
            {
                var res = context.MedicationMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.MedicineName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetRouteById(int Id)
        {
            try
            {
                var res = context.MedicineRoute.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Route;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetDosageValueById(int Id)
        {
            try
            {
                var res = context.DosageValue.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.DosValue;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetDosageUnitById(int Id)
        {
            try
            {
                var res = context.DosageUnit.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.DosUnit;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetFrequencyById(int Id)
        {
            try
            {
                var res = context.FrequencyTaken.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Frequency;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public List<ImmunizationViewModel> GetImmunizationCompleteList(Guid Id)
        {
            try
            {
                var list = context.Immunizations.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Immunizations, ImmunizationViewModel>();
                List<ImmunizationViewModel> lstImmunizationViewModel = Mapper.Map<List<Immunizations>, List<ImmunizationViewModel>>(list);
                if (lstImmunizationViewModel != null)
                {
                    foreach (var item in lstImmunizationViewModel)
                    {
                        item.strImmunizationDate = (item.ImmunizationDate != null && item.ImmunizationDate != DateTime.MinValue) ? item.ImmunizationDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.ImmunizationName = GetImmunizationNameById(item.ImmunizationsTypeId);
                    }
                }
                list = null;
                return lstImmunizationViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetImmunizationNameById(int Id)
        {
            try
            {
                var res = context.Immunizationsmasters.Where(m => m.ImmunizationsTypeId == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.ImmunizationName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public string GetTestNameById(int Id)
        {
            try
            {
                var res = context.LabTestMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.TestName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public int GetTestIdByName(String name)
        {
            try
            {
                var res = context.LabTestMaster.Where(m => m.TestName == name).FirstOrDefault();
                if (res != null)
                {
                    return res.Id;
                }
            }
            catch (Exception)
            {
            }

            return 0;
        }

        public List<LabTestViewModel> GetLabTestCompleteList(Guid Id)
        {
            try
            {
                var list = context.LabTest.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<LabTest, LabTestViewModel>();
                List<LabTestViewModel> lstLabTestViewModel = Mapper.Map<List<LabTest>, List<LabTestViewModel>>(list);
                if (lstLabTestViewModel != null)
                {
                    foreach (var item in lstLabTestViewModel)
                    {
                        item.strPerformedDate = (item.PerformedDate != null && item.PerformedDate != DateTime.MinValue) ? item.PerformedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.TestName = GetTestNameById(item.TestId);
                        item.lstFileModels = GetAllAttachments(EMRViewModels.FileType.LabReport, item.Id);
                    }
                }
                list = null;
                return lstLabTestViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProceduresViewModel> GetProceduresCompleteList(Guid Id)
        {
            try
            {
                var list = context.Procedures.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Procedures, ProceduresViewModel>();
                List<ProceduresViewModel> lstProceduresViewModel = Mapper.Map<List<Procedures>, List<ProceduresViewModel>>(list);
                if (lstProceduresViewModel != null)
                {
                    foreach (var item in lstProceduresViewModel)
                    {
                        item.strStartDate = (item.StartDate != null && item.StartDate != DateTime.MinValue) ? item.StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strEndDate = (item.EndDate != null && item.EndDate != DateTime.MinValue) ? item.EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.ProcedureName = GetProcedureNameById(item.ProcedureType);
                        item.lstFileModels = GetAllAttachments(EMRViewModels.FileType.Procedure, item.Id);
                    }
                }
                list = null;
                return lstProceduresViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<FileViewModel> GetAllAttachments(EMRViewModels.FileType type, Guid oGuid)
        {
            List<FileViewModel> lstFileViewModel = null;
            try
            {
                var recs = context.FilePath.Where(m => m.FileType == (FileType)type && m.RecId.Equals(oGuid)).ToList();
                Mapper.CreateMap<FilePath, FileViewModel>();
                lstFileViewModel = Mapper.Map<List<FilePath>, List<FileViewModel>>(recs);
            }
            catch (Exception)
            { }
            return lstFileViewModel;
        }
        public string GetProcedureNameById(int Id)
        {
            try
            {
                var res = context.ProcedureMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.ProcedureName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public List<ActivitiesViewModel> GetActivitiesCompleteList(Guid Id)
        {
            try
            {
                var list = context.Activities.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Activities, ActivitiesViewModel>();
                List<ActivitiesViewModel> lstActivitiesViewModel = Mapper.Map<List<Activities>, List<ActivitiesViewModel>>(list);
                if (lstActivitiesViewModel != null)
                {
                    foreach (var item in lstActivitiesViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
                list = null;
                lstActivitiesViewModel.ForEach(m => m.ActivityName = GetActivityNameById(m.ActivityId));
                return lstActivitiesViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetActivityNameById(int ActivityId)
        {
            string strResult = "";
            try
            {
                var rec = context.ActivityMaster.FirstOrDefault(m => m.ActivityId == ActivityId);
                if (rec != null)
                {
                    strResult = rec.ActivityName;
                    rec = null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return strResult;
        }

        public List<BloodPressureAndPulseViewModel> GetBloodPressureAndPulseCompleteList(Guid Id)
        {
            try
            {
                var list = context.BloodPressureAndPulse.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<BloodPressureAndPulse, BloodPressureAndPulseViewModel>();
                List<BloodPressureAndPulseViewModel> lstBloodPressureAndPulseViewModel = Mapper.Map<List<BloodPressureAndPulse>, List<BloodPressureAndPulseViewModel>>(list);
                if (lstBloodPressureAndPulseViewModel != null)
                {
                    foreach (var item in lstBloodPressureAndPulseViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
                list = null;
                return lstBloodPressureAndPulseViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<WeightViewModel> GetWeightsCompleteList(Guid Id)
        {
            try
            {
                var list = context.Weight.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Weight, WeightViewModel>();
                List<WeightViewModel> lstWeightViewModel = Mapper.Map<List<Weight>, List<WeightViewModel>>(list);
                if (lstWeightViewModel != null)
                {
                    foreach (var item in lstWeightViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
                list = null;
                return lstWeightViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BloodGlucoseViewModel> GetBloodGlucoseCompleteList(Guid Id)
        {
            try
            {
                var list = context.BloodGlucose.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<BloodGlucose, BloodGlucoseViewModel>();
                List<BloodGlucoseViewModel> lstBloodGlucoseViewModel = Mapper.Map<List<BloodGlucose>, List<BloodGlucoseViewModel>>(list);
                list = null;
                return lstBloodGlucoseViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
