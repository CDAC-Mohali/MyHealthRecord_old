using System;
using System.Collections.Generic;
using System.Linq;
using PHRMS.ViewModels;
using AutoMapper;
using System.Threading.Tasks;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo : IPHRMSRepo
    {
        private readonly PHRMSDbContext _db;

        public PHRMSRepo(PHRMSDbContext db)
        {
            _db = db;
        }

        #region Users

        public Users AddUser(Users user)
        {
            _db.Users.Add(user);

            if (_db.SaveChanges() > 0)
            {
                return user;
            }
            return null;

        }
        public int GetUserCount()
        {
            return _db.Users.Count();
        }

        public bool DeleteUser(int userId)
        {
            var user = _db.Users.FirstOrDefault(c => c.UserId.Equals(userId));
            if (user != null)
            {
                _db.Users.Remove(user);
                return _db.SaveChanges() > 0;
            }
            return false;
        }

        public Users GetUser(int userId)
        {
            return _db.Users.FirstOrDefault(c => c.UserId.Equals(userId));
        }

        public IEnumerable<Users> GetUsers()
        {
            return _db.Users.AsEnumerable();
        }

        #endregion

        #region Immunization

        // Implementationm of the Immunizations masters table at the back end to get the list of the immunizations

        public IEnumerable<ImmunizationsMasters> GetListOfImmunizations()
        {
            var objImmunization = _db.Immunizationsmasters.AsEnumerable();
            return objImmunization;
        }

        //Fetching out the current status of Immunizations
        public IEnumerable<Immunizations> GetImmunizations()
        {
            return _db.Immunizations.AsEnumerable();
        }

        //public string GetImmunizationNameById(int Id)
        //{
        //    var obj = _db.Immunizationsmasters.Where(m => m.ImmunizationsTypeId == Id).OrderByDescending(m => m.ImmunizationsTypeId).FirstOrDefault();
        //    return obj.ImmunizationName;
        //}
        public string GetUsersById(Guid Id)
        {
            var obj = _db.Users.Where(m => m.UserId == Id).OrderByDescending(m => m.UserId).FirstOrDefault();
            return obj.FirstName;
        }

        public Users GetUserDetailsForOpenEMR(Guid Id)
        {
            Users uobj = null;
            uobj = _db.Users.Where(m => m.UserId == Id).OrderByDescending(m => m.UserId).FirstOrDefault();
            return uobj;


        }
        //Implementation of the Add Immunization Method to insert the immunization data

        //public string AddImmunization(Immunizations imu)
        //{
        //    //Fetching the currently available id from tables Add Immunizations 
        //    //long recordId = Convert.ToInt64(_db.Immunizations.Select(temp => temp.ImmunizationsRecordsId));

        //    //Fetching the Immunizations id selected by the user
        //    //long immuneId = Convert.ToInt64(_db.Immunizationsmasters.Where(temp => temp.ImmunizationName == imu.ImmunizationName));


        //    //Fetching the id of the currently logged in user
        //    long enteredBy = Convert.ToInt64(_db.LoginLog.Select(temp => temp.UserId));

        //    //Fetching the user id of the account holder
        //    long userAccount = Convert.ToInt64(_db.Users.Select(temp => temp.UserId));

        //    //Instantiating the insertion object of type AddImmunizations Table
        //    Immunizations insertImmunizations = new Immunizations();

        //Assingning the values to different fields of the table given by users
        //insertImmunizations.ImmunizationsRecordsId = recordId++;
        //insertImmunizations.UserId = userAccount;
        //insertImmunizations.ImmunizationsId = 1;//immuneId;
        //insertImmunizations.ImmunizationName = imu.ImmunizationName;
        //insertImmunizations.ImmunizationDate = imu.ImmunizationDate;
        //insertImmunizations.Comments = "test";//imu.Comments;
        //insertImmunizations.EnteredById = imu.EnteredById; //enteredBy;


        //Finally inserting the values inside the table and saving the changes made
        //try
        //{
        //    _db.Immunizations.Add(insertImmunizations);
        //    _db.SaveChanges();
        //    return "Success";

        //}
        //catch(Exception ex)
        //{
        //    //Exception thrown if invalid operation or etc
        //    return "Failure"+ex.Message;
        //    //throw new InvalidOperationException();

        //}



        //}
        #endregion

        public async Task<RegistrationViewModel> Register(RegistrationViewModel oRegistrationViewModel)
        {
            DeletePreviousRecords(oRegistrationViewModel.MobileNo, oRegistrationViewModel.Email);
            Mapper.CreateMap<RegistrationViewModel, TempReg>();
            TempReg oTempReg = Mapper.Map<TempReg>(oRegistrationViewModel);
            oTempReg.Id = Guid.NewGuid();
            oTempReg.CreatedDate = DateTime.Now;
            _db.TempReg.Add(oTempReg);

            if (await _db.SaveChangesAsync() > 0)
            {
                oRegistrationViewModel.Id = oTempReg.Id;
                return oRegistrationViewModel;
            }
            return null;
        }

        public void DeletePreviousRecords(string strMobile, string strEmail)
        {
            try
            {
                var recs = _db.TempReg.Where(m => m.MobileNo.Equals(strMobile) || m.Email.Equals(strEmail)).ToList();
                if (recs != null && recs.Count > 0)
                {
                    foreach (var item in recs)
                    {
                        _db.Remove(item);
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<bool> ResetPassword(Guid userId, string Password)
        {
            bool res = false;
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.UserId.Equals(userId));
                if (!user.Password.Equals(Password))
                {
                    user.Password = Password;
                    res = await _db.SaveChangesAsync() > 0;
                }
                else
                    res = true;
                user = null;
            }
            catch (Exception)
            {
            }
            return res;
        }

        public bool InsertRecPullSMS(PullSMS oPullSMS)
        {
            try
            {
                _db.PullSMS.Add(oPullSMS);
                int res = _db.SaveChanges();
                oPullSMS = null;
                return res > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public RegistrationViewModel MigrateTempRegRecord(RegistrationViewModel oModel)
        {
            TempReg objTempReg = null;
            Users objUsers = null;
            oModel.status = RegProcessCode.ProcessFailure;
            try
            {
                objTempReg = _db.TempReg.OrderByDescending(m => m.CreatedDate).FirstOrDefault(m => m.Id.Equals(oModel.Id));
                if (oModel.OTP.Equals(objTempReg.OTP))
                {
                    if (objTempReg != null)
                    {
                        objUsers = new Users();
                        objUsers.UserId = Guid.NewGuid();
                        objUsers.FirstName = objTempReg.FirstName;
                        objUsers.LastName = objTempReg.LastName;
                        objUsers.Password = objTempReg.Password;
                        objUsers.PwdChangeDate = DateTime.Now;
                        objUsers.CreatedDate = DateTime.Now;
                        objUsers.RegDate = objTempReg.CreatedDate;
                        objUsers.AadhaarNo = objTempReg.AadhaarNo;
                        objUsers.Email = objTempReg.Email;
                        objUsers.MobileNo = objTempReg.MobileNo;
                        objUsers.ImgPath = "/Images/ProfilePics/e1.png";
                        objUsers.RoleId = 2;
                        objUsers.Status = true;
                        objUsers.CUG = objTempReg.CUG;

                        _db.Users.Add(objUsers);
                        PersonalInformation oPersonalInformation = new PersonalInformation();
                        oPersonalInformation.State = objTempReg.State;
                        oPersonalInformation.Gender = objTempReg.Gender;
                        oPersonalInformation.UserId = objUsers.UserId;
                        _db.PersonalInformation.Add(oPersonalInformation);
                        _db.SaveChanges();

                        _db.TempReg.Remove(objTempReg);
                        int res = _db.SaveChanges();
                        if (res > 0)
                            oModel.status = RegProcessCode.Success;
                    }
                }
                else
                    oModel.status = RegProcessCode.OtpMisMatch;
            }
            catch (Exception)
            {
            }

            objTempReg = null;
            objUsers = null;
            return oModel;
        }

        public string FetchOTPFromTemp(string strMobile)
        {
            string strRes = "";
            try
            {
                var res = _db.TempReg.Where(m => m.MobileNo.Equals(strMobile)).OrderByDescending(keySelector => keySelector.CreatedDate).FirstOrDefault();
                if (res != null)
                {
                    strRes = res.OTP;
                }
            }
            catch (Exception)
            {
            }

            return strRes;
        }
        public UsersViewModel LoginByOTP(Guid UserId)
        {
            UsersViewModel oUsersViewModel = null;
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.UserId == UserId);
                Mapper.CreateMap<Users, UsersViewModel>();
                oUsersViewModel = Mapper.Map<UsersViewModel>(user);
                user = null;
            }
            catch (Exception)
            {

                throw;
            }
            return oUsersViewModel;
        }

        public UsersViewModel GetUsersDetails(string strUsername)
        {
            UsersViewModel oUsersViewModel = null;
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.Email.Equals(strUsername) || m.MobileNo.Equals(strUsername));
                Mapper.CreateMap<Users, UsersViewModel>();
                oUsersViewModel = Mapper.Map<UsersViewModel>(user);
                user = null;
            }
            catch (Exception)
            {

                throw;
            }
            return oUsersViewModel;
        }
        public bool DoesMobileExistForOTP(string strMobileNo)
        {
            bool result = false;
            try
            {
                var record = _db.Users.FirstOrDefault(m => m.MobileNo.Equals(strMobileNo));

                if (record != null)
                {
                    result = true;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }
        public bool DoesMobileExist(string strMobileNo)
        {
            bool result = true;
            try
            {
                var record = _db.Users.FirstOrDefault(m => m.MobileNo.Equals(strMobileNo));

                if (record != null)
                {
                    result = false;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public bool DoesEmailExist(string strEmail)
        {
            bool result = true;
            try
            {
                var record = _db.Users.FirstOrDefault(m => m.Email.Equals(strEmail));

                if (record != null)
                {
                    result = false;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public bool DoesEmailOrMobileExist(string strUserName)
        {
            bool result = false;
            try
            {
                var record = _db.Users.FirstOrDefault(m => m.Email.Equals(strUserName) || m.MobileNo.Equals(strUserName));

                if (record != null)
                {
                    result = true;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }



        public List<NotificationViewModel> GetNotifications(Guid userId)
        {
            List<NotificationViewModel> lstNotifications = null;
            try
            {
                var recs = (from p in _db.ShareReportNotification
                            join k in _db.UserShareRecord
                            on p.UserRecordId equals k.UserRecordId
                            where (p.UserId.Equals(userId) && p.isPrescribedByDoctor
                            && !p.isNotificationViewed)
                            orderby p.CreatedDate descending
                            select new { p, k }).ToList();
                lstNotifications = new List<NotificationViewModel>();
                if (recs != null)
                {
                    foreach (var item in recs)
                    {
                        NotificationViewModel oNotViewModel = new NotificationViewModel();
                        var medCont = _db.MedicalContactRecords.Where(s => s.Id == item.k.MedicalContactId).FirstOrDefault();
                        var shareReportFeedBack = _db.ShareReportFeedBack.Where(s => s.MedicalContactId == item.k.MedicalContactId && s.UserRecordId.Equals(item.p.UserRecordId)).FirstOrDefault();
                        if (oNotViewModel != null)
                        {
                            oNotViewModel.MedicalContactName = medCont.ContactName;
                            if (shareReportFeedBack != null)
                            {
                                oNotViewModel.Message = shareReportFeedBack.FeedBack;
                                oNotViewModel.userRecordId = shareReportFeedBack.UserRecordId;
                            }
                        }

                        lstNotifications.Add(oNotViewModel);
                        oNotViewModel = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return lstNotifications;
        }


        public void updateNotificationAfterViewedByUser(Guid userId)
        {
            int res = 0;
            try
            {
                var list = _db.ShareReportNotification.Where(s => s.UserId == userId && s.isPrescribedByDoctor).ToList();
                foreach (var item in list)
                {
                    item.isNotificationViewed = true;
                    res++;
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }


        #region Show EMR Prescription
        public SuperViewModel ViewDetal(Guid PrescriptionId)
        {
            try
            {
                //Mapper.CreateMap<Doctor, DoctorViewModel>();
                //Mapper.CreateMap<UsersViewModel, Users>();
                //Mapper.CreateMap<EprescriptionViewModel, Eprescription>();

                Mapper.CreateMap<DoctorUserMapping, DoctorUserMappingViewModel>();
                Mapper.CreateMap<Users, UsersViewModel>();
                Mapper.CreateMap<Places_of_Practice, PlaceViewModel>();
                Mapper.CreateMap<Eprescription, EprescriptionViewModel>();
                Mapper.CreateMap<BloodPressureAndPulse, BloodPressureAndPulseViewModel>();
                Mapper.CreateMap<BloodGlucose, BloodGlucoseViewModel>();
                Mapper.CreateMap<Weight, WeightViewModel>();
                Mapper.CreateMap<PersonalInformation, PersonalInformationViewModel>();
                Mapper.CreateMap<PlaceViewModel, Places_of_Practice>();
                Mapper.CreateMap<VitalSignMaster, VitalSignMasterViewModel>();
                Mapper.CreateMap<VitalSign, VitalSignViewModel>();
                Mapper.CreateMap<MedicalHistory, MedicalHistoryViewModel>();
                Mapper.CreateMap<Medication, MedicationViewModel>();
                Mapper.CreateMap<Appointment_Fees, Appointment_FeesViewModel>();
                Mapper.CreateMap<Allergies, AllergiesViewModel>();
                Mapper.CreateMap<Doctor, DoctorViewModel>();//.ForMember(s => s.PlaceViewModel, c => c.MapFrom(m => m.Places_of_Practice));
                Mapper.CreateMap<Advice, AdviceViewModel>();
                Mapper.CreateMap<DoctorUserMappingViewModel, DoctorUserMapping>().ForMember(s => s.Doctor, c => c.MapFrom(m => m.DoctorViewModel)).ForMember(s => s.Eprescription, c => c.MapFrom(m => m.EprescriptionViewModel)).ForMember(s => s.Users, c => c.MapFrom(m => m.UsersViewModel));
                Mapper.CreateMap<DoctorUserMapping, DoctorUserMappingViewModel>().ForMember(s => s.DoctorViewModel, c => c.MapFrom(m => m.Doctor)).ForMember(s => s.EprescriptionViewModel, c => c.MapFrom(m => m.Eprescription)).ForMember(s => s.UsersViewModel, c => c.MapFrom(m => m.Users));
                Mapper.CreateMap<DocPatientDetails, DocPatientDetailsViewModel>();
                Mapper.CreateMap<HealthCondition, HealthConditionViewModel>();

                SuperViewModel oSuperViewModel = new SuperViewModel();
                var obj = _db.DoctorUserMapping.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault();
                if (obj != null)
                {
                    //   DoctorUserMappingViewModel oDoctorUserMappingViewModel = Mapper.Map<DoctorUserMapping, DoctorUserMappingViewModel>(obj);

                    oSuperViewModel.VitalSignViewModel = Mapper.Map<List<VitalSign>, List<VitalSignViewModel>>(_db.VitalSign.Where(s => s.PrescriptionId == PrescriptionId).ToList());
                    var doc = _db.Doctor.Where(s => s.docid == obj.DocId).FirstOrDefault();

                    oSuperViewModel.DoctorViewModel = Mapper.Map<Doctor, DoctorViewModel>(doc);
                    oSuperViewModel.DocPatientDetailsViewModel = Mapper.Map<DocPatientDetails, DocPatientDetailsViewModel>(_db.DocPatientDetails.Where(s => s.PHRMSUserId == obj.UserId).OrderByDescending(s => s.DocPatientId).FirstOrDefault());
                    oSuperViewModel.PlaceViewModel = Mapper.Map<Places_of_Practice, PlaceViewModel>(_db.Places_of_Practice.Where(s => s.docid == obj.DocId).FirstOrDefault());
                    oSuperViewModel.UsersViewModel = Mapper.Map<Users, UsersViewModel>(_db.Users.Where(s => s.UserId == obj.UserId).FirstOrDefault());
                    oSuperViewModel.PersonalInformationViewModel = Mapper.Map<PersonalInformation, PersonalInformationViewModel>(_db.PersonalInformation.Where(s => s.UserId == obj.UserId).FirstOrDefault());
                    oSuperViewModel.EprescriptionViewModel = Mapper.Map<Eprescription, EprescriptionViewModel>(_db.Eprescription.Where(s => s.Id == obj.PrescriptionId).FirstOrDefault());
                    oSuperViewModel.BloodPressureAndPulseViewModel = Mapper.Map<BloodPressureAndPulse, BloodPressureAndPulseViewModel>(_db.BloodPressureAndPulse.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                    oSuperViewModel.BloodGlucoseViewModel = Mapper.Map<BloodGlucose, BloodGlucoseViewModel>(_db.BloodGlucose.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                    oSuperViewModel.WeightViewModel = Mapper.Map<Weight, WeightViewModel>(_db.Weight.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                    oSuperViewModel.MedicalHistoryViewModel = Mapper.Map<MedicalHistory, MedicalHistoryViewModel>(_db.MedicalHistory.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                    oSuperViewModel.Appointment_FeesViewModel = Mapper.Map<Appointment_Fees, Appointment_FeesViewModel>(_db.Appointment_Fees.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                    //  oSuperViewModel.MedicalHistoryViewModel = Mapper.Map<MedicalHistory, MedicalHistoryViewModel>(_db.MedicalHistory.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());

                    //     oSuperViewModel.Advice = Mapper.Map<List<DataModels.Advice>, List<EMRViewModels.Advice>>(_db.Advice.Where(s => s.PrescriptionId == PrescriptionId).ToList());
                    var Advice = _db.Advice.Where(s => s.PrescriptionId == PrescriptionId).ToList();
                    var Allergies = _db.Allergies.Where(s => s.PrescriptionId == PrescriptionId).ToList();
                    var Medication = _db.Medication.Where(s => s.PrescriptionId == PrescriptionId).ToList();

                    var HealthCondition = _db.HealthCondition.Where(s => s.PrescriptionId == PrescriptionId).ToList();

                    foreach (var Item in Medication)
                    {
                        Item.MedicationName = GetMedicationName(Item.MedicineType);
                        Item.strFrequency = GetFrequencyName(Item.Frequency);
                        Item.strDosValue = GetDosageValue(Item.DosValue);
                        Item.strDosUnit = GetDosageUnit(Item.DosUnit);
                        Item.strRoute = GetMedicineRoute(Item.Route);
                    }
                    oSuperViewModel.MedicationViewModel = Mapper.Map<List<Medication>, List<MedicationViewModel>>(Medication);
                    oSuperViewModel.AllergiesViewModel = Mapper.Map<List<Allergies>, List<AllergiesViewModel>>(Allergies);

                    foreach (var Item in oSuperViewModel.AllergiesViewModel)
                    {
                        Item.strAllergyType = GetAlleryType(Item.AllergyType);
                        Item.strDuration = GetAllergyDuration(Item.DurationId);
                        Item.strSeverity = GetAllergySeverity(Item.Severity);
                    }

                    oSuperViewModel.HealthConditionViewModel = Mapper.Map<List<HealthCondition>, List<HealthConditionViewModel>>(HealthCondition);


                    foreach (var Item in oSuperViewModel.HealthConditionViewModel)
                    {
                        Item.ProblemName = GetProblemName(Item.ConditionType);

                    }


                    foreach (var Item in Advice)
                    {

                        if (ReportParameters.Immunizations == Item.ModuleId)
                            Item.Name = GetImmunization(Item.TypeId);
                        else if (ReportParameters.Tests == Item.ModuleId)
                            Item.Name = GetLabTest(Item.TypeId);
                        else if (ReportParameters.Procedures == Item.ModuleId)
                            Item.Name = GetProcedureParameters(Item.TypeId);
                    }
                    oSuperViewModel.Advice = Mapper.Map<List<Advice>, List<AdviceViewModel>>(Advice);
                }
                return oSuperViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetProcedureParameters(int TypeId)
        {
            return _db.ProcedureMaster.Where(s => s.Id == TypeId).FirstOrDefault().ProcedureName;
        }

        public string GetLabTest(int TypeId)
        {
            return _db.LabTestMaster.Where(s => s.Id == TypeId).FirstOrDefault().TestName;
        }

        public string GetImmunization(int TypeId)
        {
            return _db.Immunizationsmasters.Where(s => s.ImmunizationsTypeId == TypeId).FirstOrDefault().ImmunizationName;
        }


        public string GetAlleryType(int AllergyType)
        {
            return _db.AllergyMaster.Where(s => s.Id == AllergyType).FirstOrDefault().AllergyName;

        }
        public string GetAllergyDuration(int DurationId)
        {

            return _db.AllergyDuration.Where(s => s.Id == DurationId).FirstOrDefault().Duration;

        }
        public string GetAllergySeverity(int DurationId)
        {

            return _db.AllergySeverity.Where(s => s.Id == DurationId).FirstOrDefault().Severity;
        }

        public string GetProblemName(int ProblemType)
        {
            return _db.HealthConditionMaster.Where(s => s.Id == ProblemType).FirstOrDefault().HealthCondition;
        }


        public string GetMedicationName(int MedicineType)
        {
            return _db.MedicationMaster.Where(s => s.Id == MedicineType).FirstOrDefault().MedicineName;
        }
        public string GetFrequencyName(int Frequency)
        {
            return _db.FrequencyTaken.Where(s => s.Id == Frequency).FirstOrDefault().Frequency;
        }
        public string GetDosageValue(int DosValue)
        {
            return _db.DosageValue.Where(s => s.Id == DosValue).FirstOrDefault().DosValue;
        }
        public string GetDosageUnit(int DosUnit)
        {
            return _db.DosageUnit.Where(s => s.Id == DosUnit).FirstOrDefault().DosUnit;
        }
        public string GetMedicineRoute(int MedicineRoute)
        {
            return _db.MedicineRoute.Where(s => s.Id == MedicineRoute).FirstOrDefault().Route;
        }




        public bool DoesMobileExistMedical(Guid userId, string Mobileno, string EmailAddress)
        {
            bool result = false;
            try
            {
                var record = _db.MedicalContactRecords.Where(m => m.UserId.Equals(userId)).FirstOrDefault(m => (m.EmailAddress.Equals(EmailAddress) || m.PrimaryPhone.Equals(Mobileno)));

                if (record != null)
                {
                    result = true;

                }
                else
                {
                    result = false;
                }
            }
            catch (Exception)
            { }
            return result;
        }
        #endregion
    }
}

