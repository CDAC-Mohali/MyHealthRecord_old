using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PHRMS.Data.DataAccess
{
    public interface IPHRMSRepo
    {

        IEnumerable<Users> GetUsers();
        Users GetUser(int userId);
        Users AddUser(Users user);
        bool DeleteUser(int userId);
        Users GetUserDetailsForOpenEMR(Guid Id);
        #region Immunization
        IEnumerable<ImmunizationsMasters> GetListOfImmunizations();
        IEnumerable<Immunizations> GetImmunizations();
        List<ImmunizationViewModel> GetImmunizationGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        string GetImmunizationNameById(int Id);
        string GetUsersById(Guid Id);
        bool AddImmunization(ImmunizationViewModel oImmunization, out Guid actId);
        bool UpdateImmunization(ImmunizationViewModel oImmunization);
        ImmunizationViewModel GetImmunizationById(Guid Id);
        List<ImmunizationsMastersViewModel> GetImmunizationMaster(string init);
        int DeleteImmunization(Guid oGuid);
        List<ImmunizationViewModel> GetImmunizationCompleteList(Guid Id);
        #endregion

        Task<RegistrationViewModel> Register(RegistrationViewModel oRegistrationViewModel);
        Task<bool> ResetPassword(Guid userId, string Password);
        UsersViewModel LoginByOTP(Guid UserId);
        UsersViewModel GetUsersDetails(string strUsername);
        
             bool DoesMobileExistForOTP(string strMobileNo);
        bool DoesMobileExist(string strMobileNo);
        bool DoesEmailExist(string strEmail);
        bool DoesEmailOrMobileExist(string strUserName);
        bool DoesMobileExistMedical(Guid userId, string Mobileno,string EmailAddress);

        bool InsertRecPullSMS(PullSMS oPullSMS);

        RegistrationViewModel MigrateTempRegRecord(RegistrationViewModel oModel);
        string FetchOTPFromTemp(string strMobile);
        bool SetProfilePic(string path, Guid userID);

        #region Allergies
        List<AllergyViewModel> GetAllergiesGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        string GetAllergyNameById(int Id);
        string GetSeverityById(int Id);

        string GetSeverityNameById(int Id);
        string GetAllergyDurationById(int Id);
        bool AddAllergies(AllergyViewModel oAllergies, out Guid actId);
        void EditAllergies(Allergies oAllergies);
        List<AllergyMasterViewModel> GetAllergyMaster(string init);
        List<AllergySeverityViewModel> GetAllergySeverities();
        int DeleteAllergy(Guid oGuid);
        AllergyViewModel GetAllergyById(Guid Id);
        bool UpdateAllergy(AllergyViewModel oAllergies);
        List<AllergyViewModel> GetAllergiesCompleteList(Guid Id);
        List<AllergyDurationViewModel> GetAllergyDurationList();
        #endregion

        #region Procedures
        List<ProceduresViewModel> GetProceduresGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        string GetProcedureNameById(int Id);
        bool AddProcedure(ProceduresViewModel oProcedure, out Guid actId);
        bool UpdateProcedure(ProceduresViewModel oProcedure);
        List<ProcedureMasterViewModel> GetProcedureMaster(string str);
        ProceduresViewModel GetProcedureById(Guid Id);
        int DeleteProcedure(Guid oGuid);
        List<ProceduresViewModel> GetProceduresCompleteList(Guid Id);
        #endregion

        #region Medication
        List<MedicationViewModel> GetMedicationGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        int DeleteMedicine(Guid oGuid);
        string GetMedicineNameById(int Id);
        string GetRouteById(int Id);
        string GetDosageValueById(int Id);
        string GetDosageUnitById(int Id);
        string GetFrequencyById(int Id);
        bool AddMedicine(MedicationViewModel oMedicines, out Guid actId);
        bool UpdateMedicine(MedicationViewModel oMedicines);
        MedicationViewModel GetMedicineById(Guid Id);
        List<MedicationMasterViewModel> GetMedicationMaster(string str, int skip);
        List<MedicineRouteViewModel> GetRoutes();
        List<DosageValueViewModel> GetDosagevalues();
        List<DosageUnitViewModel> GetDosageunits();
        List<FrequencyTakenViewModel> GetFrequencies();
        List<MedicationViewModel> GetMedicationCompleteList(Guid Id);
        #endregion


        #region Lab
        List<LabTestViewModel> GetLabTestResultGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        int DeleteResult(Guid oGuid);
        string GetTestNameById(int Id);
        int GetTestIdByName(String name);
        bool AddTest(LabTestViewModel oTests, out Guid actId);
        bool UpdateResult(LabTestViewModel oResults);
        LabTestViewModel GetResultById(Guid Id);
        List<LabTestMasterViewModel> GetLabTestMaster(string str);
        List<LabTestViewModel> GetLabTestCompleteList(Guid Id);
        #endregion

        #region Profile
        Task<PersonalViewModel> PersonalInfoUpdate(PersonalViewModel oProfileInsert);
        Task<EmergencyViewModel> EmergencyInfoUpdate(EmergencyViewModel oEmergencyInsert);
        Task<EmployerViewModel> EmployerInfoUpdate(EmployerViewModel oEmployerViewModel);
        Task<InsuranceViewModel> InsuranceInfoUpdate(InsuranceViewModel oInsuranceViewModel);
        Task<LegalViewModel> LegalInfoUpdate(LegalViewModel oLegalViewModel);
        Task<PreferencesViewModel> PreferencesInfoUpdate(PreferencesViewModel oPreferencesViewModel);
        PersonalViewModel GetPersonalInformation(Guid userId);
        PersonalViewModel GetPatientInformation(Guid userId);
        EmergencyViewModel GetEmergencyInformation(Guid userId);
        EmployerViewModel GetEmployerInformation(Guid userId);
        InsuranceViewModel GetInsuranceInformation(Guid userId);
        LegalViewModel GetLegalInformation(Guid userId);
        PreferencesViewModel GetPreferences(Guid userId);
        void UpdateNameInReg(Guid userId, string strFirstName, string strLastName);
        #endregion

        #region Shared
        List<StatesModel> GetStatesList();
        List<ContactTypeModel> GetContactTypesList();
        List<DisabilityTypesModel> GetDisabilityTypesList();
        List<BloodGroupsModel> GetBloodGroupsList();
        List<RelationshipModel> GetRelationshipList();
        string GetStateNameById(int Id);
        string GetRelationNameById(int Id);
        string GetDisabilityTypeById(int Id);
        string GetBloodGroupById(int Id);
        List<string> GetPostalCodesFromMaster(string strPostalCode);
        List<MedicalContactRecordsViewModel> GetDoctorsList(Guid UserId);
        bool AddContactUs(ContactusViewModel oModel);
        bool ChangePassword(Guid UserId, string NewPassword, string OldPassword);


        bool SoftDeleteFileIfExists(FileViewModel oModel);
        bool SaveFileDetails(FileViewModel oModel);
        string GetSavedFilePath(Guid Id, PHRMS.ViewModels.FileType type);
        bool SaveBulkFiles(List<FileViewModel> lstFiles);
        List<FileViewModel> GetAllAttachments(PHRMS.ViewModels.FileType type, Guid oGuid);
        String GetProfilePercentage(Guid userid);
        string GetEmailByUserId(Guid UserId);
      

        float GetBMI(string height, string weight);
        #endregion

        #region Wellness
        List<ActivitiesViewModel> GetActivitiesRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        List<BloodGlucoseViewModel> GetBloodGlucoseRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        List<BloodPressureAndPulseViewModel> GetBPAndPulseRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        List<SleepViewModel> GetSleepGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        List<WeightViewModel> GetWeightGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        List<TemperatureViewModel> GetTemperatureGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        bool AddBloodGlucose(BloodGlucoseViewModel oWellness);
        bool UpdateBloodGlucose(BloodGlucoseViewModel oWellness);
        int DeleteBloodGlucose(Guid oGuid);
        bool AddSleep(SleepViewModel oWellness, out Guid actId);
        bool UpdateSleep(SleepViewModel oWellness);
        int DeleteSleep(Guid oGuid);
        bool AddWeight(WeightViewModel oWellness);
        bool UpdateWeight(WeightViewModel oWellness);
        int DeleteWeight(Guid oGuid);
        bool AddActivity(ActivitiesViewModel oActivities, out Guid ActId);
        bool UpdateActivity(ActivitiesViewModel oWellness);
        int DeleteActivities(Guid oGuid);
        bool AddBloodPressureAndPulse(BloodPressureAndPulseViewModel oWellness, out Guid ActId);
        bool UpdateBloodPressureAndPulse(BloodPressureAndPulseViewModel oWellness);
        int DeleteBloodPressureAndPulse(Guid oGuid);
        bool AddTemperature(TemperatureViewModel oWellness);
        bool UpdateTemperature(TemperatureViewModel oWellness);
        int DeleteTemperature(Guid oGuid);
        BloodPressureAndPulseViewModel GetBloodPressureById(Guid Id);
        BloodGlucoseViewModel GetBloodGlucoseById(Guid Id);
        SleepViewModel GetSleepById(Guid Id);
        TemperatureViewModel GetTemperatureById(Guid Id);
        WeightViewModel GetWeightById(Guid Id);
        List<ActivityMasterViewModel> GetActivitiesMaster();
        ActivitiesViewModel GetActivitiesById(Guid Id);
        List<ActivitiesViewModel> GetActivitiesCompleteList(Guid Id);
        List<BloodPressureAndPulseViewModel> GetBloodPressureAndPulseCompleteList(Guid Id);
        List<WeightViewModel> GetWeightsCompleteList(Guid Id);
        List<BloodGlucoseViewModel> GetBloodGlucoseCompleteList(Guid Id);
        ShareReportFeedBackViewModel GetShareReportFeedBack(Guid Id,Guid EprescriptionId);
        #endregion

        #region HealthCondition
        List<HealthConditionViewModel> GetHealthConditionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        int DeleteHealthCondition(Guid oGuid);
        string GetHealthConditionNameById(int Id);
        bool AddHealthCondition(HealthConditionViewModel oCondition, out Guid actId);
        bool UpdateHealthCondition(HealthConditionViewModel oCondition);
        HealthConditionViewModel GetHealthConditionById(Guid Id);
        List<HealthConditionMasterViewModel> GetHealthConditionMaster(string init);
        List<HealthConditionViewModel> GetHealthConditionCompleteList(Guid Id);
        #endregion

        #region Eprescription
        List<EprescriptionViewModel> GetPrescriptionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        List<EprescriptionViewModel> GetSharePrescriptionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);

        SuperViewModel ViewDetal(Guid PrescriptionId);
        EprescriptionViewModel GetPrescriptionById(Guid Id);
        bool AddPrescription(EprescriptionViewModel oPrescription, out Guid actId);
        bool UpdatePrescription(EprescriptionViewModel oPrescription);
        int DeletePrescription(Guid oGuid);
        PersonalInformation GetPersonById(PersonalInformation obj);
        List<EprescriptionViewModel> GetEprescriptionCompleteList(Guid Id);
        #endregion

        #region DashboardAnalytics        
        DashboardAnalyticsViewModel UpdateAnalytics(Guid userId);
        // int DeleteAnalytics(int Id);
        List<BPViewModel> GetBPandPulseData(Guid userId);
        List<string[]> GetLatestProcedures(Guid userId);
        List<string[]> GetLatestMedications(Guid userId);
        List<string[]> GetLatestLabs(Guid userId);
        List<string[]> GetLatestImmunizations(Guid userId);
        List<string[]> GetLatestAllergies(Guid userId);
        List<string[]> GetWeightData(Guid userId);
        List<string[]> GetActivityData(Guid userId);
        List<string[]> GetGlucoseData(Guid userId);
        string GetLatestActivities(Guid userId);
        bool AddUserActivity(UserActivityViewModels oUserActivityViewModels);
        bool AddUserShareRecordEntry(UserShareRecordViewModels oUserShareRecordViewModels);
        UserShareRecordViewModels GetUserId(string Password);
        UserShareRecordViewModels GetShareReportDetail(long UserRecordId);
        
        bool SaveFeedBack(ShareFeedBack oShareFeedBack, out string DoctorName);

        List<UserActivityViewModels> GeUserActivityPartialList(Guid Id, int count);
        string GetActivityNameById(Guid Id, int Module);
        string GetHealthTip(int Id);
        #endregion

        #region MedicalContact
        List<MedicalContactRecordsViewModel> GetContactGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        string GetContactNameById(int Id);
        MedicalContactRecordsViewModel GetContactById(Guid Id);
        bool AddContact(MedicalContactRecordsViewModel oContact);
        bool UpdateContact(MedicalContactRecordsViewModel oContacts);
        int DeleteContact(Guid oGuid);
        string GetContactTypeById(int Id);
        #endregion

        #region OpenEMR
        List<StatesOpenEMRViewModel> GetStatesOpenEMRList();
        List<DistrictsViewModel> GetDistrictNameByStateId(int StateId);
        List<SubDistrictsViewModel> GetSubDistrictNameByDistrictId(int DistrictId);
        List<Hospital_OpenEMRViewModel> GetOpenEMRGridList(Guid userId, string city, int? page, int? limit, string sortBy, string direction, string searchString, out int total);
        //void AddRowToTrigger();
        #endregion

        int GetUserCount();
        #region Notifications
        List<NotificationViewModel> GetNotifications(Guid userId);
        #endregion

        #region updateNotificationAfterViewedByUser
        void updateNotificationAfterViewedByUser(Guid userId);
        #endregion

        
    }
}




