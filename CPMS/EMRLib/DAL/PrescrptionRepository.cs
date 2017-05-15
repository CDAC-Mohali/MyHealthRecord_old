using System.Collections.Generic;
using System.Linq;
using PagedList;
using EMRLib.DataModels;
using System;
using EMRViewModels;
using AutoMapper;
using System.Data.Entity.Core.Objects;

namespace EMRLib.DAL
{
    public partial class EMRRepository
    {
        public List<ImmunizationsMastersViewModel> GetImmunizationMaster(string init)
        {
            try
            {
                List<ImmunizationsMastersViewModel> list = (from k in context.Immunizationsmasters
                                                            where k.ImmunizationName.ToLower().StartsWith(init.ToLower())
                                                            select new ImmunizationsMastersViewModel
                                                            {
                                                                ImmunizationsTypeId = k.ImmunizationsTypeId,
                                                                ImmunizationName = k.ImmunizationName.ToLower()
                                                            }).OrderBy(m => m.ImmunizationName).ToList();
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<LabTestMasterViewModel> GetLabTestMaster(string str)
        {
            try
            {
                var list = (from k in context.LabTestMaster
                            where k.TestName.ToLower().StartsWith(str.ToLower())
                            select new LabTestMasterViewModel
                            {
                                Id = k.Id,
                                TestName = k.TestName.ToLower()
                            }).OrderBy(m => m.TestName).ToList();
                //var list = _db.LabTestMaster.Where(m => m.TestName.ToLower().StartsWith(str.ToLower())).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<ProblemMasterViewModel> GetProbleMaster(string str)
        {
            try
            {
                var list = (from k in context.HealthConditionMaster
                            where k.HealthCondition.StartsWith(str)
                            select new ProblemMasterViewModel
                            {
                                Id = k.Id,
                                ProblemName = k.HealthCondition
                            }).OrderBy(m => m.ProblemName).ToList();

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProcedureMasterViewModel> GetProcedureMaster(string str)
        {
            try
            {
                var list = (from k in context.ProcedureMaster
                            where k.ProcedureName.StartsWith(str)
                            select new ProcedureMasterViewModel
                            {
                                Id = k.Id,
                                ProcedureName = k.ProcedureName
                            }).OrderBy(m => m.ProcedureName).ToList();

                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AllergyMasterViewModel> GetAllergyMaster(string init)
        {
            try
            {
                //var list = _db.AllergyMaster.Where(m => m.AllergyName.ToLower().StartsWith(init.ToLower())).OrderBy(m => m.AllergyName).ToList();
                var list = (from k in context.AllergyMaster
                            where k.AllergyName.ToLower().StartsWith(init.ToLower())
                            select new AllergyMasterViewModel
                            {
                                Id = k.Id,
                                AllergyName = k.AllergyName.ToLower()
                            }).OrderBy(m => m.AllergyName).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<MedicationMasterViewModel> GetMedicationMaster(string init)
        {
            try
            {
                //var list = _db.AllergyMaster.Where(m => m.AllergyName.ToLower().StartsWith(init.ToLower())).OrderBy(m => m.AllergyName).ToList();
                var list = (from k in context.MedicationMaster
                            where k.MedicineName.ToLower().StartsWith(init.ToLower())
                            select new MedicationMasterViewModel
                            {
                                Id = k.Id,
                                MedicineName = k.MedicineName.ToLower()
                            }).Take(50).OrderBy(m => m.MedicineName).GroupBy(p => p.MedicineName).Select(g => g.FirstOrDefault()).ToList();
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public DoctorViewModel DoctorDetail(Guid DoctorId)
        {
            var doc = context.Doctor.Where(s => s.docid == DoctorId).FirstOrDefault();
            Mapper.CreateMap<DoctorViewModel, Doctor>();
            return Mapper.Map<DoctorViewModel>(doc);
        }
        public DocPatientDetailsViewModel PatientDetail(Guid PatientId)
        {
            var Patient = context.DocPatientDetails.Where(s => s.PHRMSUserId == PatientId).FirstOrDefault();
            Mapper.CreateMap<DocPatientDetailsViewModel, DocPatientDetails>();
            return Mapper.Map<DocPatientDetailsViewModel>(Patient);
        }
        public DocPatientDetailsViewModel EMRPatientDetail(long DocPatientId)
        {
            var Patient = context.DocPatientDetails.Where(s => s.DocPatientId == DocPatientId).FirstOrDefault();
            Mapper.CreateMap<DocPatientDetailsViewModel, DocPatientDetails>();
            return Mapper.Map<DocPatientDetailsViewModel>(Patient);

        }
        public PlaceViewModel PlaceViewModel(Guid DoctorId)
        {
            Mapper.CreateMap<PlaceViewModel, Places_of_Practice>();
            Mapper.CreateMap<Places_of_Practice, PlaceViewModel>();
            var Patient = Mapper.Map<Places_of_Practice, PlaceViewModel>(context.Places_of_Practice.Where(s => s.docid == DoctorId).FirstOrDefault());
            return Mapper.Map<PlaceViewModel>(Patient);
        }
        public bool SaveEMRComplete(EMRComplete oComplete)
        {
            bool _result = false;
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                Guid PresId = Guid.NewGuid();
                #region  Create Prescription
                var doc = context.Doctor.Where(s => s.docid == oComplete.DocId).FirstOrDefault();
                Eprescription oEprescription = new Eprescription();
                oEprescription.PhysicalExamination = oComplete.PhysicalExamination;
                oEprescription.ProblemDiagnosis = oComplete.ProblemDiagnosis;
                oEprescription.OtherAdvice = oComplete.OtherAdvice;
                oEprescription.Id = PresId;
                oEprescription.ClinicName = doc.name;
                oEprescription.CreatedDate = DateTime.Now;
                oEprescription.DeleteFlag = false;
                oEprescription.DocAddress = doc.Places_of_Practice[0].AddressLine1 + " " + doc.Places_of_Practice[0].AddressLine2;
                oEprescription.DocName = doc.name;
                oEprescription.DocPhone = doc.phone_number;
                oEprescription.FileName = " ";
                oEprescription.ModifiedDate = DateTime.Now;
                oEprescription.PresDate = DateTime.Now;
                oEprescription.Prescription = "";
                oEprescription.UserId = oComplete.UserId;
                oEprescription.SourceId = oComplete.SourceId;
                context.Eprescription.Add(oEprescription);
                context.SaveChanges();
                #endregion
                if (oComplete.EMRVitals != null)
                {
                    if ((!string.IsNullOrEmpty(oComplete.EMRVitals.Systolic) && !string.IsNullOrEmpty(oComplete.EMRVitals.Diastolic)) || !string.IsNullOrEmpty(oComplete.EMRVitals.Pulse))
                    {
                        BloodPressureAndPulse oBPPulse = new BloodPressureAndPulse();
                        oBPPulse.CreatedDate = DateTime.Now;
                        oBPPulse.ModifiedDate = DateTime.Now;
                        oBPPulse.DeleteFlag = false;
                        oBPPulse.ResSystolic = oComplete.EMRVitals.Systolic;
                        oBPPulse.ResDiastolic = oComplete.EMRVitals.Diastolic;
                        oBPPulse.ResPulse = oComplete.EMRVitals.Pulse;
                        oBPPulse.SourceId = oComplete.SourceId;
                        oBPPulse.UserId = oComplete.UserId;
                        oBPPulse.Id = Guid.NewGuid();
                        oBPPulse.PrescriptionId = PresId;
                        oBPPulse.CollectionDate = DateTime.Now;
                        context.BloodPressureAndPulse.Add(oBPPulse);
                    }

                    if (!string.IsNullOrEmpty(oComplete.EMRVitals.Glucose))
                    {
                        BloodGlucose oGlucose = new BloodGlucose();
                        oGlucose.CreatedDate = DateTime.Now;
                        oGlucose.ModifiedDate = DateTime.Now;
                        oGlucose.Id = Guid.NewGuid();
                        oGlucose.SourceId = oComplete.SourceId;
                        oGlucose.CollectionDate = DateTime.Now;
                        oGlucose.Result = oComplete.EMRVitals.Glucose;
                        oGlucose.UserId = oComplete.UserId;
                        oGlucose.DeleteFlag = false;
                        oGlucose.PrescriptionId = PresId;
                        context.BloodGlucose.Add(oGlucose);
                    }

                    if (!string.IsNullOrEmpty(oComplete.EMRVitals.Weight))
                    {
                        Weight oWeight = new Weight();
                        oWeight.CreatedDate = DateTime.Now;
                        oWeight.ModifiedDate = DateTime.Now;
                        oWeight.Id = Guid.NewGuid();
                        oWeight.SourceId = oComplete.SourceId;
                        oWeight.Result = oComplete.EMRVitals.Weight;
                        oWeight.UserId = oComplete.UserId;
                        oWeight.CollectionDate = DateTime.Now;
                        oWeight.Goal = "0";
                        oWeight.DeleteFlag = false;
                        oWeight.PrescriptionId = PresId;
                        context.Weight.Add(oWeight);
                    }

                    if (!string.IsNullOrEmpty(oComplete.EMRVitals.RespiratoryRate))
                    {
                        VitalSign oVitalSign = new VitalSign();
                        oVitalSign.CreatedDate = DateTime.Now;
                        oVitalSign.ModifiedDate = DateTime.Now;
                        oVitalSign.Type = VitalSignParameters.RespiratoryRate;
                        oVitalSign.Id = Guid.NewGuid();
                        oVitalSign.SourceId = oComplete.SourceId;
                        oVitalSign.Result = oComplete.EMRVitals.RespiratoryRate;
                        oVitalSign.UserId = oComplete.UserId;
                        oVitalSign.DeleteFlag = false;
                        oVitalSign.PrescriptionId = PresId;
                        context.VitalSign.Add(oVitalSign);
                    }

                    if (!string.IsNullOrEmpty(oComplete.EMRVitals.SpO2))
                    {
                        VitalSign oVitalSign = new VitalSign();
                        oVitalSign.CreatedDate = DateTime.Now;
                        oVitalSign.ModifiedDate = DateTime.Now;
                        oVitalSign.Type = VitalSignParameters.PulseOximetry;
                        oVitalSign.Id = Guid.NewGuid();
                        oVitalSign.SourceId = oComplete.SourceId;
                        oVitalSign.Result = oComplete.EMRVitals.SpO2;
                        oVitalSign.UserId = oComplete.UserId;
                        oVitalSign.DeleteFlag = false;
                        oVitalSign.PrescriptionId = PresId;
                        context.VitalSign.Add(oVitalSign);
                    }
                }

                if (oComplete.MedicalHistory != null && (!string.IsNullOrEmpty(oComplete.MedicalHistory.FamilyHistory) || !string.IsNullOrEmpty(oComplete.MedicalHistory.PersonalHistory)))
                {
                    MedicalHistory oHistory = new MedicalHistory();
                    oHistory.Id = Guid.NewGuid();
                    oHistory.PersonalHistory = oComplete.MedicalHistory.PersonalHistory;
                    oHistory.FamilyHistory = oComplete.MedicalHistory.FamilyHistory;
                    oHistory.UserId = oComplete.UserId;
                    oHistory.PrescriptionId = PresId;
                    context.MedicalHistory.Add(oHistory);
                }

                if (oComplete.Medications != null && oComplete.Medications.Count > 0)
                {
                    Mapper.CreateMap<MedicationViewModel, Medication>();
                    List<Medication> lstMedications = Mapper.Map<List<MedicationViewModel>, List<Medication>>(oComplete.Medications);
                    foreach (var item in lstMedications)
                    {
                        item.Id = Guid.NewGuid();
                        item.PrescriptionId = PresId;
                        item.UserId = oComplete.UserId;
                        context.Medication.Add(item);
                    }
                }

                if (oComplete.Appointment != null && oComplete.Appointment.Date.Date > DateTime.Now.Date)
                {
                    Mapper.CreateMap<AppointmentViewModel, Appointment_Fees>();
                    Appointment_Fees obj = Mapper.Map<Appointment_Fees>(oComplete.Appointment);
                    obj.PrescriptionId = PresId;
                    obj.UserId = oComplete.UserId;
                    obj.DocId = oComplete.DocId;
                    obj.Id = Guid.NewGuid();
                    context.Appointment_Fees.Add(obj);
                }

                if (oComplete.Allergies != null && oComplete.Allergies.Count > 0)
                {
                    foreach (var oAllergies in oComplete.Allergies)
                    {
                        if (oAllergies != null && oAllergies.AllergyType != 0)
                        {
                            oAllergies.Id = Guid.NewGuid();
                            Mapper.CreateMap<AllergyViewModel, Allergies>();
                            Allergies objAllergies = Mapper.Map<Allergies>(oAllergies);
                            objAllergies.CreatedDate = DateTime.Now;
                            objAllergies.ModifiedDate = DateTime.Now;
                            objAllergies.SourceId = 2;
                            objAllergies.PrescriptionId = PresId;
                            objAllergies.UserId = oComplete.UserId;
                            context.Allergies.Add(objAllergies);
                            context.SaveChanges();

                        }
                    }
                }
                if (oComplete.Problem != null && oComplete.Problem.Count > 0)
                {
                    foreach (var oProblem in oComplete.Problem)
                    {
                        if (oProblem != null && oProblem.ConditionType != 0)
                        {
                            oProblem.Id = Guid.NewGuid();
                            Mapper.CreateMap<ProblemViewModel, HealthCondition>();
                            HealthCondition objProblem = Mapper.Map<HealthCondition>(oProblem);
                            objProblem.CreatedDate = DateTime.Now;
                            objProblem.ModifiedDate = DateTime.Now;
                            objProblem.SourceId = 2;
                            objProblem.UserId = oComplete.UserId;
                            objProblem.PrescriptionId = PresId;
                            context.HealthCondition.Add(objProblem);
                            context.SaveChanges();

                        }
                    }
                }
                if (oComplete.Advice != null && oComplete.Advice.Count > 0)
                {
                    foreach (var oAdvice in oComplete.Advice)
                    {
                        if (oAdvice != null && oAdvice.ModuleId != 0)
                        {
                            DataModels.Advice Advice = new DataModels.Advice();
                            Advice.PrescriptionId = PresId;
                            Advice.ModuleId = oAdvice.ModuleId;
                            Advice.TypeId = oAdvice.TypeId;
                            context.Advice.Add(Advice);
                            context.SaveChanges();
                        }
                    }
                }
                DoctorUserMapping oDoctorUserMapping = new DoctorUserMapping();
                oDoctorUserMapping.PrescriptionId = PresId;
                oDoctorUserMapping.DocId = oComplete.DocId;
                oDoctorUserMapping.UserId = oComplete.UserId;
                //oDoctorUserMapping.EMRUserId = oComplete.EMRUserId;
                oDoctorUserMapping.CreatedDate = DateTime.Now;
                context.DoctorUserMapping.Add(oDoctorUserMapping);
                context.SaveChanges();
                _result = context.SaveChanges() > 0;
                #region check Medical Contact Against
                if (context.MedicalContactRecords.Where(s => s.PrimaryPhone == doc.phone_number && s.UserId == oComplete.UserId).FirstOrDefault() == null)
                {
                    MedicalContactRecords objContact = new MedicalContactRecords();
                    objContact.Id = Guid.NewGuid();
                    objContact.CreatedDate = DateTime.Now;
                    objContact.ModifiedDate = DateTime.Now;
                    objContact.UserId = oComplete.UserId;
                    objContact.ContactName = doc.name;
                    objContact.ClinicName = doc.Places_of_Practice[0].name;
                    objContact.MedContType = doc.Speciality;
                    objContact.Address1 = doc.Places_of_Practice[0].AddressLine1;
                    objContact.Address2 = doc.Places_of_Practice[0].AddressLine2;
                    objContact.CityVillage = doc.Places_of_Practice[0].city;
                    objContact.State = doc.Places_of_Practice[0].state;
                    objContact.PIN = doc.Places_of_Practice[0].pincode;
                    objContact.District = "";
                    objContact.EmailAddress = doc.email;
                    objContact.PrimaryPhone = doc.phone_number;
                    context.MedicalContactRecords.Add(objContact);
                    context.SaveChanges();
                }
                #endregion
                //if (_result)
                //{
                //    PrescriptionMap oMap = new PrescriptionMap();
                //    oMap.Id = PresId;
                //    oMap.MapId = mapId;
                //    _db.PrescriptionMap.Add(oMap);
                //    _db.SaveChanges();
                //}
                //    scope.Complete();
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

            return _result;
        }

        public List<EprescriptionViewModel> GetPrescriptionList(Guid DocId, Guid PatientId)
        {
            List<EprescriptionViewModel> result = null;
            try
            {
                var presRecord = context.Eprescription.Where(s => s.SourceId == 2 && s.UserId.Equals(PatientId)).ToList();
                if (presRecord != null)
                {
                    Mapper.CreateMap<Eprescription, EprescriptionViewModel>();
                    result = Mapper.Map<List<Eprescription>, List<EprescriptionViewModel>>(presRecord);
                }

            }
            catch (Exception)
            {
                result = null;
                //    throw;
            }
            return result;
        }

        public List<Events> AllGetEvents(Guid DocId)
        {
            List<Events> myList = new List<Events>();
            try
            {
                DateTime oDate = DateTime.Now;
                DateTime Week = oDate.Date.AddDays(7);
                DateTime Month = oDate.Date.AddMonths(1);

                List<Appointment_Fees> lstAppointment_Fees = context.Appointment_Fees.Where(s => s.DocId == DocId).Where(s => EntityFunctions.TruncateTime(s.Date) >= EntityFunctions.TruncateTime(oDate) && EntityFunctions.TruncateTime(s.Date) < EntityFunctions.TruncateTime(Month)).ToList();

                foreach (var Item in lstAppointment_Fees.GroupBy(s => s.Date).ToList())
                {
                    Events oEvents = new Events();
                    oEvents.start_date = Item.FirstOrDefault().Date.ToString("yyyy-MM-dd");
                    oEvents.end_date = Item.FirstOrDefault().Date.ToString("yyyy-MM-dd");
                    oEvents.text = lstAppointment_Fees.Where(s => s.Date.Date == Item.FirstOrDefault().Date.Date).Count().ToString();
                    myList.Add(oEvents);
                    // oDate.Date.AddDays(1);
                }
                //IEnumerable<Events> myEnumerable = myList;
                //oUsersViewModel = myEnumerable.ToList();


            }
            catch (Exception ex)
            {

                throw;
            }
            return myList;
        }

        public IPagedList<DoctorUserMappingViewModel> GetConsultationsData(int Page, Guid DoctorId, string Keyword)
        {
            try
            {
                Mapper.CreateMap<Doctor, DoctorViewModel>();
                Mapper.CreateMap<UsersViewModel, Users>();
                Mapper.CreateMap<EprescriptionViewModel, Eprescription>();

                Mapper.CreateMap<DoctorViewModel, DoctorUserMappingViewModel>();
                Mapper.CreateMap<Users, UsersViewModel>();
                Mapper.CreateMap<Eprescription, EprescriptionViewModel>();


                Mapper.CreateMap<DoctorUserMappingViewModel, DoctorUserMapping>().ForMember(s => s.Doctor, c => c.MapFrom(m => m.DoctorViewModel)).ForMember(s => s.Eprescription, c => c.MapFrom(m => m.EprescriptionViewModel)).ForMember(s => s.Users, c => c.MapFrom(m => m.UsersViewModel));
                Mapper.CreateMap<DoctorUserMapping, DoctorUserMappingViewModel>().ForMember(s => s.DoctorViewModel, c => c.MapFrom(m => m.Doctor)).ForMember(s => s.EprescriptionViewModel, c => c.MapFrom(m => m.Eprescription)).ForMember(s => s.UsersViewModel, c => c.MapFrom(m => m.Users));
                IPagedList<DoctorUserMappingViewModel> oDoctorUserMappingViewModel = Mapper.Map<List<DoctorUserMapping>, List<DoctorUserMappingViewModel>>(context.DoctorUserMapping.OrderByDescending(s => s.Id).Where(m => m.DocId.Equals(DoctorId) && (m.Users.Email.Contains(Keyword) || m.Users.MobileNo.Contains(Keyword) || m.Users.FirstName.Contains(Keyword) || m.Users.LastName.Contains(Keyword))).ToList()).OrderByDescending(s => s.Id).ToList().ToPagedList(Page, AppSetting.PageSize);

                //var obj = context.DoctorUserMapping.OrderByDescending(s => s.CreatedDate).Where(s => s.DocId == DoctorId && s.Users.Email.Contains(Keyword)).ToPagedList(Page, AppSetting.PageSize).ToList();
                //IPagedList<DoctorUserMappingViewModel> oDoctorUserMappingViewModel = Mapper.Map<List<DoctorUserMapping>, List<DoctorUserMappingViewModel>>(obj);
                return oDoctorUserMappingViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<DoctorUserMappingViewModel> GetPreviousVisitData(Guid UserId, Guid DoctorId)
        {
            try
            {
                Mapper.CreateMap<Doctor, DoctorViewModel>();
                Mapper.CreateMap<UsersViewModel, Users>();
                Mapper.CreateMap<EprescriptionViewModel, Eprescription>();

                Mapper.CreateMap<DoctorViewModel, DoctorUserMappingViewModel>();
                Mapper.CreateMap<Users, UsersViewModel>();
                Mapper.CreateMap<Eprescription, EprescriptionViewModel>();


                Mapper.CreateMap<DoctorUserMappingViewModel, DoctorUserMapping>().ForMember(s => s.Doctor, c => c.MapFrom(m => m.DoctorViewModel)).ForMember(s => s.Eprescription, c => c.MapFrom(m => m.EprescriptionViewModel)).ForMember(s => s.Users, c => c.MapFrom(m => m.UsersViewModel));
                Mapper.CreateMap<DoctorUserMapping, DoctorUserMappingViewModel>().ForMember(s => s.DoctorViewModel, c => c.MapFrom(m => m.Doctor)).ForMember(s => s.EprescriptionViewModel, c => c.MapFrom(m => m.Eprescription)).ForMember(s => s.UsersViewModel, c => c.MapFrom(m => m.Users));

                var obj = context.DoctorUserMapping.Where(s => s.UserId == UserId && s.DocId == DoctorId).ToList();
                List<DoctorUserMappingViewModel> oDoctorUserMappingViewModel = Mapper.Map<List<DoctorUserMapping>, List<DoctorUserMappingViewModel>>(obj);
                return oDoctorUserMappingViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



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
                Mapper.CreateMap<HealthCondition, ProblemViewModel>();
                Mapper.CreateMap<Doctor, DoctorViewModel>().ForMember(s => s.PlaceViewModel, c => c.MapFrom(m => m.Places_of_Practice));
                Mapper.CreateMap<DataModels.Advice, EMRViewModels.Advice>();
                Mapper.CreateMap<DoctorUserMappingViewModel, DoctorUserMapping>().ForMember(s => s.Doctor, c => c.MapFrom(m => m.DoctorViewModel)).ForMember(s => s.Eprescription, c => c.MapFrom(m => m.EprescriptionViewModel)).ForMember(s => s.Users, c => c.MapFrom(m => m.UsersViewModel));
                Mapper.CreateMap<DoctorUserMapping, DoctorUserMappingViewModel>().ForMember(s => s.DoctorViewModel, c => c.MapFrom(m => m.Doctor)).ForMember(s => s.EprescriptionViewModel, c => c.MapFrom(m => m.Eprescription)).ForMember(s => s.UsersViewModel, c => c.MapFrom(m => m.Users));

                var obj = context.DoctorUserMapping.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault();
                //   DoctorUserMappingViewModel oDoctorUserMappingViewModel = Mapper.Map<DoctorUserMapping, DoctorUserMappingViewModel>(obj);
                SuperViewModel oSuperViewModel = new SuperViewModel();
                oSuperViewModel.VitalSignViewModel = Mapper.Map<List<VitalSign>, List<VitalSignViewModel>>(context.VitalSign.Where(s => s.PrescriptionId == PrescriptionId).ToList());
                oSuperViewModel.DoctorViewModel = Mapper.Map<Doctor, DoctorViewModel>(obj.Doctor);
                oSuperViewModel.DocPatientDetailsViewModel = Mapper.Map<DocPatientDetails, DocPatientDetailsViewModel>(context.DocPatientDetails.Where(s => s.PHRMSUserId == obj.UserId).OrderByDescending(s => s.DocPatientId).FirstOrDefault());
                oSuperViewModel.PlaceViewModel = Mapper.Map<Places_of_Practice, PlaceViewModel>(context.Places_of_Practice.Where(s => s.docid == obj.DocId).FirstOrDefault());
                oSuperViewModel.UsersViewModel = Mapper.Map<Users, UsersViewModel>(obj.Users);
                oSuperViewModel.PersonalInformationViewModel = Mapper.Map<PersonalInformation, PersonalInformationViewModel>(context.PersonalInformation.Where(s => s.UserId == obj.UserId).FirstOrDefault());
                oSuperViewModel.EprescriptionViewModel = Mapper.Map<Eprescription, EprescriptionViewModel>(obj.Eprescription);
                oSuperViewModel.BloodPressureAndPulseViewModel = Mapper.Map<BloodPressureAndPulse, BloodPressureAndPulseViewModel>(context.BloodPressureAndPulse.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                oSuperViewModel.BloodGlucoseViewModel = Mapper.Map<BloodGlucose, BloodGlucoseViewModel>(context.BloodGlucose.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                oSuperViewModel.WeightViewModel = Mapper.Map<Weight, WeightViewModel>(context.Weight.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                oSuperViewModel.MedicalHistoryViewModel = Mapper.Map<MedicalHistory, MedicalHistoryViewModel>(context.MedicalHistory.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                //oSuperViewModel.ProblemViewModel = Mapper.Map<List<HealthCondition>, List<ProblemViewModel>>(context.HealthCondition.Where(s => s.PrescriptionId == PrescriptionId).ToList());
                oSuperViewModel.Appointment_FeesViewModel = Mapper.Map<Appointment_Fees, Appointment_FeesViewModel>(context.Appointment_Fees.Where(s => s.PrescriptionId == PrescriptionId).FirstOrDefault());
                oSuperViewModel.VitalSignViewModel = Mapper.Map<List<VitalSign>, List<VitalSignViewModel>>(context.VitalSign.Where(s => s.PrescriptionId == PrescriptionId).ToList());
                //     oSuperViewModel.Advice = Mapper.Map<List<DataModels.Advice>, List<EMRViewModels.Advice>>(context.Advice.Where(s => s.PrescriptionId == PrescriptionId).ToList());
                var Advice = context.Advice.Where(s => s.PrescriptionId == PrescriptionId).ToList();
                var Allergies = context.Allergies.Where(s => s.PrescriptionId == PrescriptionId).ToList();
                var Medication = context.Medication.Where(s => s.PrescriptionId == PrescriptionId).ToList();
                var HealthCondition = context.HealthCondition.Where(s => s.PrescriptionId == PrescriptionId).ToList();

                foreach (var Item in Medication)
                {
                    Item.MedicationName = GetMedicationName(Item.MedicineType);
                    Item.strFrequency = GetFrequencyName(Item.Frequency);
                    Item.strDosValue = GetDosageValue(Item.DosValue);
                    Item.strDosUnit = GetDosageUnit(Item.DosUnit);
                    Item.strRoute = GetMedicineRoute(Item.Route);
                }
                oSuperViewModel.MedicationViewModel = Mapper.Map<List<Medication>, List<MedicationViewModel>>(Medication);

                foreach (var Item in Allergies)
                {
                    Item.strAllergyType = GetAlleryType(Item.AllergyType);
                    Item.strDuration = GetAllergyDuration(Item.DurationId);
                    Item.strSeverity = GetAllergySeverity(Item.Severity);
                }
                oSuperViewModel.AllergiesViewModel = Mapper.Map<List<Allergies>, List<AllergiesViewModel>>(Allergies);

                oSuperViewModel.ProblemViewModel = Mapper.Map<List<HealthCondition>, List<ProblemViewModel>>(HealthCondition);

                foreach (var Item in oSuperViewModel.ProblemViewModel)
                {
                    Item.strProblemType = GetProblemName(Item.ConditionType);


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
                oSuperViewModel.Advice = Mapper.Map<List<DataModels.Advice>, List<EMRViewModels.Advice>>(Advice);

                return oSuperViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string GetProcedureParameters(int TypeId)
        {
            return context.ProcedureMaster.Where(s => s.Id == TypeId).FirstOrDefault().ProcedureName;
        }

        public string GetLabTest(int TypeId)
        {
            return context.LabTestMaster.Where(s => s.Id == TypeId).FirstOrDefault().TestName;
        }

        public string GetImmunization(int TypeId)
        {
            return context.Immunizationsmasters.Where(s => s.ImmunizationsTypeId == TypeId).FirstOrDefault().ImmunizationName;
        }


        public string GetAlleryType(int AllergyType)
        {
            return context.AllergyMaster.Where(s => s.Id == AllergyType).FirstOrDefault().AllergyName;

        }

        public string GetProblemName(int ConditionType)
        {
            return context.HealthConditionMaster.Where(s => s.Id == ConditionType).FirstOrDefault().HealthCondition;

        }


        public string GetAllergyDuration(int DurationId)
        {

            return context.AllergyDuration.Where(s => s.Id == DurationId).FirstOrDefault().Duration;

        }
        public string GetAllergySeverity(int DurationId)
        {

            return context.AllergySeverity.Where(s => s.Id == DurationId).FirstOrDefault().Severity;
        }


        public string GetMedicationName(int MedicineType)
        {
            return context.MedicationMaster.Where(s => s.Id == MedicineType).FirstOrDefault().MedicineName;
        }
        public string GetFrequencyName(int Frequency)
        {
            return context.FrequencyTaken.Where(s => s.Id == Frequency).FirstOrDefault().Frequency;
        }
        public string GetDosageValue(int DosValue)
        {
            return context.DosageValue.Where(s => s.Id == DosValue).FirstOrDefault().DosValue;
        }
        public string GetDosageUnit(int DosUnit)
        {
            return context.DosageUnit.Where(s => s.Id == DosUnit).FirstOrDefault().DosUnit;
        }
        public string GetMedicineRoute(int MedicineRoute)
        {
            return context.MedicineRoute.Where(s => s.Id == MedicineRoute).FirstOrDefault().Route;
        }



        public List<BPViewModel> GetBPandPulseData(Guid userId)
        {
            List<BPViewModel> onj = new List<BPViewModel>();
            try
            {
                var Res2 = (from p in context.BloodPressureAndPulse
                            where p.UserId.Equals(userId) && p.SourceId.Equals(2)
                            select p).OrderBy(s => s.CollectionDate).ToList();
                foreach (var a in Res2.Where(s => s.CollectionDate > DateTime.Now.AddYears(-20)))
                {
                    BPViewModel onjj = new BPViewModel();
                    onjj.Diastolic = (a.ResDiastolic == null || a.ResDiastolic == "") ? 0 : float.Parse(a.ResDiastolic);
                    onjj.Systolic = (a.ResSystolic == null || a.ResSystolic == "") ? 0 : float.Parse(a.ResSystolic);
                    onjj.Pulse = (a.ResPulse == null || a.ResPulse == "") ? 0 : float.Parse(a.ResPulse);
                    onjj.Date = a.CollectionDate == null ? "" : a.CollectionDate.ToString("dd-MM-yy");
                    onj.Add(onjj);
                }
                return onj;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<string[]> GetGlucoseData(Guid userId)
        {
            List<string[]> Res = new List<string[]>();
            try
            {
                var Rec = (from p in context.BloodGlucose
                           where p.UserId == userId && p.SourceId.Equals(2)
                           select new BGlocoseModel
                           {
                               Result = p.Result,
                               CollectionDate = p.CollectionDate
                           }).OrderBy(m => m.CollectionDate).ToList();


                foreach (var item in Rec)
                {

                    String date = item.CollectionDate == null ? "" : item.CollectionDate.ToString("dd-MM-yy");
                    string[] strArry = { item.Result, date };
                    Res.Add(strArry);
                    strArry = null;

                }
                return Res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string CheckPhoneRevist(string Phone, long RecodeId, Guid DocId)
        {
            string str = "0";
            try
            {

                if (RecodeId != 0)
                {

                    var rec = context.DocPatientDetails.Where(m => m.DocPatientId != RecodeId && m.DocId == DocId && m.PhoneNumber == Phone).FirstOrDefault();
                    if (rec != null)
                    {
                        str = "1";
                    }
                }
                else
                {
                    var rec2 = context.DocPatientDetails.Where(m => m.DocId == DocId && m.PhoneNumber == Phone).FirstOrDefault();
                    if (rec2 != null)
                    {
                        str = "1";
                    }
                }
            }
            catch (Exception)
            {

            }
            return str;
        }


    }
}
