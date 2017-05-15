using EMRLib.DataModels;
using EMRViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace EMRLib.DAL
{
    public interface IEMRRepository
    {
        Guid AddDoctor(Doctor oModel);
        bool UpdateDoctorStatus(Doctor oModel);
        bool ChangePassword(Guid DocId, string NewPassword, string OldPassword);
        bool CompleteRegistration(Places_of_Practice oModel, string fileName);
        bool DoesMobileExist(string strMobileNo);
        bool DoesEmailExist(string strEmail);
        ForgotPasswordModel VerifyUsernameforPasswordChange(ForgotPasswordModel model);
        DoctorViewModel GetUsersDetails(string strUsername);
        bool ResetPassword(Guid userId, string Password);
        bool AddContactUs(ContactusViewModel oModel);
        bool DoesEmailOrMobileExist(string UserName);

        DoctorViewModel Login(string UserName, string Password);
        
        DoctorViewModel LoginByOTP(Guid userId);

        string GetFilePath(Guid docid);
        #region Patients
        IPagedList<DocPatientDetailsViewModel> GetPatientsList(int page, Guid DocId);
        IPagedList<DocPatientDetailsViewModel> GetPatientsListSearch(int page, Guid DocId,string keyword);
        DocPatientDetailsViewModel CheckPatientByDoc(DocPatientDetailsViewModel oDocPatientDetailsViewModel);
        IPagedList<Appointment_FeesViewModel> GetAppointmentsByDate(Guid docId, int Type, int page = 1);
        List<Appointment_FeesViewModel> LoadDataForAppointments(Guid docId, DateTime date);

        DocPatientDetailsViewModel GetPatientById(Guid DocId, long PatId);
        Guid SavePatientDetails(DocPatientDetailsViewModel model, out UsersViewModel Status);
        long GetTotalAppointmentByDoctor(Guid docId);
        PersonalViewModel GetPersonalInformation(Guid userId);
        PersonalViewModel GetInitialPersonalDetails(Guid userId);
        EmergencyViewModel GetEmergencyInformation(Guid userId);
        EmployerViewModel GetInitialEmployerDetails(Guid userId);
        EmployerViewModel GetEmployerInformation(Guid userId);
        InsuranceViewModel GetInsuranceInformation(Guid userId);
        LegalViewModel GetLegalInformation(Guid userId);
        PreferencesViewModel GetPreferences(Guid userId);
        List<AllergyViewModel> GetAllergiesCompleteList(Guid Id);
        List<HealthConditionViewModel> GetHealthConditionExportableList(Guid Id);
        List<MedicationViewModel> GetMedicationCompleteList(Guid Id);
        List<ImmunizationViewModel> GetImmunizationCompleteList(Guid Id);
        List<LabTestViewModel> GetLabTestCompleteList(Guid Id);
        List<ProceduresViewModel> GetProceduresCompleteList(Guid Id);
        List<ActivitiesViewModel> GetActivitiesCompleteList(Guid Id);
        List<BloodPressureAndPulseViewModel> GetBloodPressureAndPulseCompleteList(Guid Id);
        List<WeightViewModel> GetWeightsCompleteList(Guid Id);
        List<BloodGlucoseViewModel> GetBloodGlucoseCompleteList(Guid Id);
        string AddPHRMSOTPShare(Guid PHRMSUserId, int Type);
        bool CheckPHRMSOTPShare(Guid PHRMSUserId, string OTP,int Type);
        List<DoctorUserMappingViewModel> GetPreviousVisitData(Guid UserId, Guid DoctorId);
        IPagedList<DoctorUserMappingViewModel> GetConsultationsData(int Page,Guid DoctorId,string Keyword);
         List<Events> AllGetEvents(Guid DocId);
        


        #endregion

        #region Prescription
        List<ImmunizationsMastersViewModel> GetImmunizationMaster(string init);
        List<LabTestMasterViewModel> GetLabTestMaster(string str);
        List<ProcedureMasterViewModel> GetProcedureMaster(string str);
        List<ProblemMasterViewModel> GetProbleMaster(string str);
        List<AllergyMasterViewModel> GetAllergyMaster(string init);
        List<MedicationMasterViewModel> GetMedicationMaster(string init);
        bool SaveEMRComplete(EMRComplete oComplete);
        List<EprescriptionViewModel> GetPrescriptionList(Guid DocId, Guid PatientId);
        SuperViewModel ViewDetal(Guid PrescriptionId);
        DocPatientDetailsViewModel PatientDetail(Guid PatientId);
        DocPatientDetailsViewModel EMRPatientDetail(long DocPatientId);

        
        DoctorViewModel DoctorDetail(Guid DoctorId);
   
        PlaceViewModel PlaceViewModel(Guid DoctorId);
        string GetProcedureParameters(int TypeId);
        string GetLabTest(int TypeId);
        string GetImmunization(int TypeId);
        string GetAlleryType(int AllergyType);
        string GetProblemName(int ConditionType);
        string GetAllergyDuration(int DurationId);
        string GetAllergySeverity(int DurationId);
        string GetMedicationName(int MedicineType);
        string GetFrequencyName(int Frequency);
        string GetDosageValue(int DosValue);
        string GetDosageUnit(int DosUnit);
        string GetMedicineRoute(int MedicineRoute);

        string CheckPhoneRevist(string Phone, long RecodeId, Guid DocId);
        #endregion

        #region Profile Pic Management
        bool SetProfilePic(string imgpath, Guid guid);
        bool SoftDeleteFileIfExists(EMRDocFilePath docFilePath);
        bool SaveFileDetails(EMRDocFilePath docFilePath);


        #endregion


        List<BPViewModel> GetBPandPulseData(Guid userId);
        List<string[]> GetGlucoseData(Guid userId);
        //string GetHealthTip(int Id);    
        List<PatientReginModel> PatientRegionGraph(Guid docId);
        PatientReginModel PatientGenderGraph(Guid docId);
        PatientReginModel EMRPatientDetail(Guid docId);
        WeekModel GetPatientLastWeekDetail(Guid docId);
    }
}
