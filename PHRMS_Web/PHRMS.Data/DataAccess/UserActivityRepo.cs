using PHRMS.ViewModels;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {
        public bool AddUserActivity(UserActivityViewModels oUserActivityViewModels)
        {
            bool _result = false;
            try
            {
                if (oUserActivityViewModels != null && oUserActivityViewModels.Module != 0 && oUserActivityViewModels.Operation != 0)
                {
                    Mapper.CreateMap<UserActivityViewModels, UserActivities>();
                    UserActivities objUserActivities = Mapper.Map<UserActivities>(oUserActivityViewModels);
                    objUserActivities.TimeStamp = DateTime.Now;
                    _db.UserActivity.Add(objUserActivities);
                    int res = _db.SaveChanges();
                    oUserActivityViewModels = null;
                    objUserActivities = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            return _result;
        }
        public List<UserActivityViewModels> GeUserActivityPartialList(Guid Id, int count)
        {
            try
            {
                var list = _db.UserActivity.Where(m => m.UserId.Equals(Id)).OrderByDescending(l => l.TimeStamp).ToList();
                if (count > 0)
                    list = list.Take(count).ToList();
                Mapper.CreateMap<UserActivities, UserActivityViewModels>();
                List<UserActivityViewModels> lstUserActivityViewModel = Mapper.Map<List<UserActivities>, List<UserActivityViewModels>>(list);
                list = null;
                return lstUserActivityViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetActivityNameById(Guid Id, int Module)
        {
            try
            {

                switch (Module)
                {
                    case 1:
                        var data = _db.Allergies.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var Master = _db.AllergyMaster.Where(k => k.Id.Equals(data.AllergyType)).FirstOrDefault();
                        return Master.AllergyName;
                    case 2:
                        var dataImmunization = _db.Immunizations.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var MasterImmunization = _db.Immunizationsmasters.Where(k => k.ImmunizationsTypeId.Equals(dataImmunization.ImmunizationsTypeId)).FirstOrDefault();
                        return MasterImmunization.ImmunizationName;
                    case 3:
                        var dataLabTest = _db.LabTest.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var MasterLabTest = _db.LabTestMaster.Where(k => k.Id.Equals(dataLabTest.TestId)).FirstOrDefault();
                        return MasterLabTest.TestName;
                    case 4:
                        var dataMedication = _db.Medication.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var MasterMedication = _db.MedicationMaster.Where(k => k.Id.Equals(dataMedication.MedicineType)).FirstOrDefault();
                        return MasterMedication.MedicineName;
                    case 5:
                        var dataProcedure = _db.Procedures.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var MasterProcedure = _db.ProcedureMaster.Where(k => k.Id.Equals(dataProcedure.ProcedureType)).FirstOrDefault();
                        return MasterProcedure.ProcedureName;
                    case 6:
                        var dataHC = _db.HealthCondition.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var MasterHC = _db.HealthConditionMaster.Where(k => k.Id.Equals(dataHC.ConditionType)).FirstOrDefault();
                        return MasterHC.HealthCondition;
                    case 7:
                        var dataAct = _db.Activities.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        var MasterAct = _db.ActivityMaster.Where(k => k.ActivityId.Equals(dataAct.ActivityId)).FirstOrDefault();
                        return MasterAct.ActivityName;
                    case 8:
                        var dataBP = _db.BloodPressureAndPulse.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        return "Blood pressure";
                    case 9:
                        return "Blood Glucose";
                    case 10:
                        return "Sleep";
                    case 11:
                        return "Weight";
                    case 12:
                        return "Profile Picture";
                    case 13:
                        var dataPres = _db.Eprescription.Where(m => m.Id.Equals(Id)).FirstOrDefault();
                        return dataPres.ClinicName;
                    case 14:
                        return "Temperature";
                    default: return "";
                }

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}