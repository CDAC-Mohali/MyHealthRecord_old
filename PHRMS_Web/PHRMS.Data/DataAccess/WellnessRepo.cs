using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;
using System.Runtime.Remoting.Contexts;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {
        #region Activities
        //Activities Region
        public List<ActivitiesViewModel> GetActivitiesRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Activities
                           join k in _db.ActivityMaster on p.ActivityId equals k.ActivityId
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new ActivitiesViewModel
                           {
                               Id = p.Id,
                               Result = p.Result,
                               PathName = p.PathName,
                               Distance = p.Distance,
                               StartTime = p.StartTime,
                               FinishTime = p.FinishTime,
                               CollectionDate = p.CollectionDate,
                               strCollectionDate = GetDateCustomTimeString(p.CollectionDate),
                               Comments = p.Comments,
                               CreatedDate = p.CreatedDate,
                               ActivityName = k.ActivityName,
                               SourceId=p.SourceId,
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Result.Contains(searchString));
            }

            total = records.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            else
            {
                records = SortHelper.OrderByDescending(records, "CreatedDate");
            }

            //var recs = records.AsEnumerable();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            var rs = records.ToList();

            foreach (var item in rs)
            {
                item.sno
                    = _serialNo++;
            }

            return rs;
        }
        public bool AddActivity(ActivitiesViewModel oActivities, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oActivities != null && oActivities.ActivityId != 0)
                {
                    oActivities.Id = Guid.NewGuid();

                    Mapper.CreateMap<ActivitiesViewModel, Activities>();
                    Activities objWellness = Mapper.Map<Activities>(oActivities);
                    objWellness.CreatedDate = DateTime.Now;
                    objWellness.ModifiedDate = DateTime.Now;
                    _db.Activities.Add(objWellness);
                    int res = _db.SaveChanges();

                    objWellness = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            actId = oActivities.Id;
            oActivities = null;
            return _result;
        }
        public bool UpdateActivity(ActivitiesViewModel oWellness)
        {
            bool result = false;
            try
            {
                var record = _db.Activities.FirstOrDefault(m => m.Id.Equals(oWellness.Id));
                if (record != null)
                {
                    record.Result = oWellness.Result;
                    record.PathName = oWellness.PathName;

                    record.Distance = oWellness.Distance;
                    record.StartTime = oWellness.StartTime;
                    record.FinishTime = oWellness.FinishTime;
                    record.CollectionDate = oWellness.CollectionDate;
                    record.Comments = oWellness.Comments;
                    record.ModifiedDate = DateTime.Now;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public int DeleteActivities(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Activities.FirstOrDefault(m => m.Id.Equals(oGuid));

                if (rec != null)
                {
                    rec.DeleteFlag = true;
                    res = _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public ActivitiesViewModel GetActivitiesById(Guid Id)
        {
            ActivitiesViewModel oMedicationViewModel = null;
            try
            {
                Activities oMedicines = _db.Activities.FirstOrDefault(m => m.Id.Equals(Id));
                if (oMedicines != null)
                {
                    Mapper.CreateMap<Activities, ActivitiesViewModel>();
                    oMedicationViewModel = Mapper.Map<ActivitiesViewModel>(oMedicines);
                    oMedicines = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oMedicationViewModel;
        }
        public List<ActivityMasterViewModel> GetActivitiesMaster()
        {
            try
            {
                var list = _db.ActivityMaster.ToList();
                Mapper.CreateMap<ActivityMaster, ActivityMasterViewModel>();
                List<ActivityMasterViewModel> lstMedicationMasterViewModel = Mapper.Map<List<ActivityMaster>, List<ActivityMasterViewModel>>(list);
                return lstMedicationMasterViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Blood Glucose
        //Blood Glucose Region
        public List<BloodGlucoseViewModel> GetBloodGlucoseRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.BloodGlucose
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new BloodGlucoseViewModel
                           {
                               Id = p.Id,
                               Result = p.Result,
                               Goal = p.Goal,
                               ValueType = p.ValueType,
                               CollectionDate = p.CollectionDate,
                               strCollectionDate = GetDateCustomTimeString(p.CollectionDate),
                               Comments = p.Comments,
                               CreatedDate = p.CreatedDate
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Result.Contains(searchString));
            }

            total = records.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            else
            {
                records = SortHelper.OrderByDescending(records, "CreatedDate");
            }

            //var recs = records.AsEnumerable();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            var rs = records.ToList();

            foreach (var item in rs)
            {
                item.sno
                    = _serialNo++;
            }

            return rs;
        }
        public bool AddBloodGlucose(BloodGlucoseViewModel oWellness)
        {
            bool _result = false;
            try
            {
                if (oWellness != null)
                {
                    oWellness.Id = Guid.NewGuid();

                    Mapper.CreateMap<BloodGlucoseViewModel, BloodGlucose>();
                    BloodGlucose objWellness = Mapper.Map<BloodGlucose>(oWellness);
                    objWellness.CreatedDate = DateTime.Now;
                    objWellness.ModifiedDate = DateTime.Now;
                    _db.BloodGlucose.Add(objWellness);
                    int res = _db.SaveChanges();
                    oWellness = null;
                    objWellness = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            return _result;
        }
        public bool UpdateBloodGlucose(BloodGlucoseViewModel oWellness)
        {
            bool result = false;
            try
            {
                var record = _db.BloodGlucose.FirstOrDefault(m => m.Id.Equals(oWellness.Id));
                if (record != null)
                {
                    record.Result = oWellness.Result;
                    record.Goal = oWellness.Goal;
                    record.ValueType = oWellness.ValueType;
                    record.CollectionDate = oWellness.CollectionDate;
                    record.Comments = oWellness.Comments;
                    record.ModifiedDate = DateTime.Now;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public int DeleteBloodGlucose(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.BloodGlucose.FirstOrDefault(m => m.Id.Equals(oGuid));

                if (rec != null)
                {
                    rec.DeleteFlag = true;
                    res = _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public BloodGlucoseViewModel GetBloodGlucoseById(Guid Id)
        {
            BloodGlucoseViewModel oMedicationViewModel = null;
            try
            {
                BloodGlucose oMedicines = _db.BloodGlucose.FirstOrDefault(m => m.Id.Equals(Id));
                if (oMedicines != null)
                {
                    Mapper.CreateMap<BloodGlucose, BloodGlucoseViewModel>();
                    oMedicationViewModel = Mapper.Map<BloodGlucoseViewModel>(oMedicines);
                    oMedicines = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oMedicationViewModel;
        }
        #endregion

        #region Blood Pressure
        //Blood Pressure and Pulse Region
        public List<BloodPressureAndPulseViewModel> GetBPAndPulseRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.BloodPressureAndPulse
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new BloodPressureAndPulseViewModel
                           {
                               Id = p.Id,
                               ResSystolic = p.ResSystolic,
                               ResDiastolic = p.ResDiastolic,
                               GoalSystolic = p.GoalSystolic,
                               GoalDiastolic = p.GoalDiastolic,
                               CollectionDate = p.CollectionDate,
                               strCollectionDate = GetDateCustomTimeString(p.CollectionDate),
                               GoalPulse = p.GoalPulse,
                               ResPulse = p.ResPulse,
                               Comments = p.Comments,
                               CreatedDate = p.CreatedDate
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Comments.Contains(searchString));
            }

            total = records.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            else
            {
                records = SortHelper.OrderByDescending(records, "CreatedDate");
            }

            //var recs = records.AsEnumerable();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            var rs = records.ToList();

            foreach (var item in rs)
            {
                item.sno
                    = _serialNo++;
            }

            return rs;
        }
        public bool AddBloodPressureAndPulse(BloodPressureAndPulseViewModel oWellness, out Guid ActId)
        {
            bool _result = false;
            try
            {
                if (oWellness != null)
                {
                    oWellness.Id = Guid.NewGuid();
                    Mapper.CreateMap<BloodPressureAndPulseViewModel, BloodPressureAndPulse>();
                    BloodPressureAndPulse objWellness = Mapper.Map<BloodPressureAndPulse>(oWellness);
                    objWellness.CreatedDate = DateTime.Now;
                    objWellness.ModifiedDate = DateTime.Now;
                    _db.BloodPressureAndPulse.Add(objWellness);
                    int res = _db.SaveChanges();

                    objWellness = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            ActId = oWellness.Id;
            oWellness = null;
            return _result;
        }
        public bool UpdateBloodPressureAndPulse(BloodPressureAndPulseViewModel oWellness)
        {
            bool result = false;
            try
            {
                var record = _db.BloodPressureAndPulse.FirstOrDefault(m => m.Id.Equals(oWellness.Id));
                if (record != null)
                {
                    record.ResSystolic = oWellness.ResSystolic;
                    record.ResDiastolic = oWellness.ResDiastolic;
                    record.GoalSystolic = oWellness.GoalSystolic;
                    record.GoalDiastolic = oWellness.GoalDiastolic;
                    record.GoalPulse = oWellness.GoalPulse;
                    record.ResPulse = oWellness.ResPulse;
                    record.CollectionDate = oWellness.CollectionDate;
                    record.Comments = oWellness.Comments;
                    record.ModifiedDate = DateTime.Now;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public int DeleteBloodPressureAndPulse(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.BloodPressureAndPulse.FirstOrDefault(m => m.Id.Equals(oGuid));

                if (rec != null)
                {
                    rec.DeleteFlag = true;
                    res = _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public BloodPressureAndPulseViewModel GetBloodPressureById(Guid Id)
        {
            BloodPressureAndPulseViewModel oMedicationViewModel = null;
            try
            {
                BloodPressureAndPulse oMedicines = _db.BloodPressureAndPulse.FirstOrDefault(m => m.Id.Equals(Id));
                if (oMedicines != null)
                {
                    Mapper.CreateMap<BloodPressureAndPulse, BloodPressureAndPulseViewModel>();
                    oMedicationViewModel = Mapper.Map<BloodPressureAndPulseViewModel>(oMedicines);
                    oMedicines = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oMedicationViewModel;
        }
        #endregion

        #region Sleep
        //Sleep Region
        public List<SleepViewModel> GetSleepGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Sleep
                           where p.UserId.Equals(userId) && !p.DeleteFlag
                           select new SleepViewModel
                           {
                               Id = p.Id,
                               Result = p.Result,
                               Goal = p.Goal,
                               CollectionDate = p.CollectionDate,
                               strCollectionDate = GetDateCustomTimeString(p.CollectionDate),
                               Comments = p.Comments,
                               CreatedDate = p.CreatedDate
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Comments.Contains(searchString));
            }

            total = records.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            else
            {
                records = SortHelper.OrderBy(records, "CreatedDate");
            }

            //var recs = records.AsEnumerable();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            var rs = records.ToList();

            foreach (var item in rs)
            {
                item.sno
                    = _serialNo++;
            }

            return rs;
        }
        public bool AddSleep(SleepViewModel oWellness, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oWellness != null)
                {
                    oWellness.Id = Guid.NewGuid();
                    Mapper.CreateMap<SleepViewModel, Sleep>();
                    Sleep objWellness = Mapper.Map<Sleep>(oWellness);
                    objWellness.CreatedDate = DateTime.Now;
                    objWellness.ModifiedDate = DateTime.Now;
                    _db.Sleep.Add(objWellness);
                    int res = _db.SaveChanges();
                    objWellness = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            actId = oWellness.Id;
            oWellness = null;
            return _result;
        }
        public bool UpdateSleep(SleepViewModel oWellness)
        {
            bool result = false;
            try
            {
                var record = _db.Sleep.FirstOrDefault(m => m.Id.Equals(oWellness.Id));
                if (record != null)
                {
                    record.Result = oWellness.Result;
                    record.Goal = oWellness.Goal;
                    record.CollectionDate = oWellness.CollectionDate;
                    record.Comments = oWellness.Comments;
                    record.ModifiedDate = DateTime.Now;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public int DeleteSleep(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Sleep.FirstOrDefault(m => m.Id.Equals(oGuid));

                if (rec != null)
                {
                    rec.DeleteFlag = true;
                    res = _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public SleepViewModel GetSleepById(Guid Id)
        {
            SleepViewModel oSleepViewModel = null;
            try
            {
                Sleep oWellness = _db.Sleep.FirstOrDefault(m => m.Id.Equals(Id));
                if (oWellness != null)
                {
                    Mapper.CreateMap<Sleep, SleepViewModel>();
                    oSleepViewModel = Mapper.Map<SleepViewModel>(oWellness);
                    oWellness = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oSleepViewModel;
        }
        #endregion

        #region Weight
        //Weight Region
        public List<WeightViewModel> GetWeightGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            List<WeightViewModel> rs = null;
            try
            {
                int _serialNo = 1;
                var records = (from p in _db.Weight
                               where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2 
                               select new WeightViewModel
                               {
                                   Id = p.Id,
                                   Result = p.Result,
                                   Goal = p.Goal,
                                   CollectionDate = p.CollectionDate,
                                   strCollectionDate = GetDateCustomTimeString(p.CollectionDate),
                                   Comments = p.Comments,
                                   CreatedDate = p.CreatedDate,
                                   BMI = GetBMI(p.Goal, p.Result)
                               }).AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records.Where(p => p.Comments.Contains(searchString));
                }

                total = records.Count();

                if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
                {
                    if (direction.Trim().ToLower() == "asc")
                    {
                        records = SortHelper.OrderBy(records, sortBy);
                    }
                    else
                    {
                        records = SortHelper.OrderByDescending(records, sortBy);
                    }
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, "CreatedDate");
                }

                //var recs = records.AsEnumerable();

                if (page.HasValue && limit.HasValue)
                {
                    int start = (page.Value - 1) * limit.Value;
                    records = records.Skip(start).Take(limit.Value);
                }

                rs = records.ToList();

                foreach (var item in rs)
                {
                    item.sno = _serialNo++;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return rs;
        }
        public bool AddWeight(WeightViewModel oWellness)
        {
            bool _result = false;
            try
            {
                if (oWellness != null)
                {
                    oWellness.Id = Guid.NewGuid();

                    Mapper.CreateMap<WeightViewModel, Weight>();
                    Weight objWellness = Mapper.Map<Weight>(oWellness);
                    objWellness.CreatedDate = DateTime.Now;
                    objWellness.ModifiedDate = DateTime.Now;
                    _db.Weight.Add(objWellness);
                    int res = _db.SaveChanges();
                    oWellness = null;
                    objWellness = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            return _result;
        }
        public bool UpdateWeight(WeightViewModel oWellness)
        {
            bool result = false;
            try
            {
                var record = _db.Weight.FirstOrDefault(m => m.Id.Equals(oWellness.Id));
                if (record != null)
                {
                    record.Result = oWellness.Result;
                    record.Goal = oWellness.Goal;
                    record.CollectionDate = oWellness.CollectionDate;
                    record.Comments = oWellness.Comments;
                    record.ModifiedDate = DateTime.Now;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public int DeleteWeight(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Weight.FirstOrDefault(m => m.Id.Equals(oGuid));

                if (rec != null)
                {
                    rec.DeleteFlag = true;
                    res = _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public WeightViewModel GetWeightById(Guid Id)
        {
            WeightViewModel oWeightViewModel = null;
            try
            {
                Weight oWellness = _db.Weight.FirstOrDefault(m => m.Id.Equals(Id));
                if (oWellness != null)
                {
                    Mapper.CreateMap<Weight, WeightViewModel>();
                    oWeightViewModel = Mapper.Map<WeightViewModel>(oWellness);
                    oWellness = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oWeightViewModel;
        }
        #endregion

        #region Temperature
        //Temperature Region
        public List<TemperatureViewModel> GetTemperatureGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Temperature
                           where p.UserId.Equals(userId) && !p.DeleteFlag
                           select new TemperatureViewModel
                           {
                               Id = p.Id,
                               Result = p.Result,
                               Goal = p.Goal,
                               CollectionDate = p.CollectionDate,
                               strCollectionDate = GetDateCustomTimeString(p.CollectionDate),
                               Comments = p.Comments,
                               CreatedDate = p.CreatedDate
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.Comments.Contains(searchString));
            }

            total = records.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            else
            {
                records = SortHelper.OrderBy(records, "CreatedDate");
            }

            //var recs = records.AsEnumerable();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            var rs = records.ToList();

            foreach (var item in rs)
            {
                item.sno
                    = _serialNo++;
            }

            return rs;
        }
        public bool AddTemperature(TemperatureViewModel oWellness)
        {
            bool _result = false;
            try
            {
                if (oWellness != null)
                {
                    oWellness.Id = Guid.NewGuid();

                    Mapper.CreateMap<TemperatureViewModel, Temperature>();
                    Temperature objWellness = Mapper.Map<Temperature>(oWellness);
                    objWellness.CreatedDate = DateTime.Now;
                    objWellness.ModifiedDate = DateTime.Now;
                    _db.Temperature.Add(objWellness);
                    int res = _db.SaveChanges();
                    oWellness = null;
                    objWellness = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            return _result;
        }
        public bool UpdateTemperature(TemperatureViewModel oWellness)
        {
            bool result = false;
            try
            {
                var record = _db.Temperature.FirstOrDefault(m => m.Id.Equals(oWellness.Id));
                if (record != null)
                {
                    record.Result = oWellness.Result;
                    record.Goal = oWellness.Goal;
                    record.CollectionDate = oWellness.CollectionDate;
                    record.Comments = oWellness.Comments;
                    record.ModifiedDate = DateTime.Now;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public int DeleteTemperature(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Temperature.FirstOrDefault(m => m.Id.Equals(oGuid));

                if (rec != null)
                {
                    rec.DeleteFlag = true;
                    res = _db.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return res;
        }
        public TemperatureViewModel GetTemperatureById(Guid Id)
        {
            TemperatureViewModel oSleepViewModel = null;
            try
            {
                Temperature oWellness = _db.Temperature.FirstOrDefault(m => m.Id.Equals(Id));
                if (oWellness != null)
                {
                    Mapper.CreateMap<Temperature, TemperatureViewModel>();
                    oSleepViewModel = Mapper.Map<TemperatureViewModel>(oWellness);
                    oWellness = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oSleepViewModel;
        }
        #endregion
        public List<WeightViewModel> GetWeightsCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Weight.Where(m => m.SourceId != 2 && m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).OrderBy("CollectionDate").ToList();
                Mapper.CreateMap<Weight, WeightViewModel>();
                List<WeightViewModel> lstWeightViewModel = Mapper.Map<List<Weight>, List<WeightViewModel>>(list);
                list = null;
                return lstWeightViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BloodPressureAndPulseViewModel> GetBloodPressureAndPulseCompleteList(Guid Id)
        {
            try
            {
                var list = _db.BloodPressureAndPulse.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<BloodPressureAndPulse, BloodPressureAndPulseViewModel>();
                List<BloodPressureAndPulseViewModel> lstBloodPressureAndPulseViewModel = Mapper.Map<List<BloodPressureAndPulse>, List<BloodPressureAndPulseViewModel>>(list);
                list = null;
                return lstBloodPressureAndPulseViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ActivitiesViewModel> GetActivitiesCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Activities.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<Activities, ActivitiesViewModel>();
                List<ActivitiesViewModel> lstActivitiesViewModel = Mapper.Map<List<Activities>, List<ActivitiesViewModel>>(list);
                list = null;
                lstActivitiesViewModel.ForEach(m => m.ActivityName = GetActivityNameById(m.ActivityId));
                return lstActivitiesViewModel;
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
                var list = _db.BloodGlucose.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
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

        public ShareReportFeedBackViewModel GetShareReportFeedBack(Guid Id, Guid EprescriptionId)
        {
            try
            {
                var list = _db.ShareReportFeedBack.Where(m => m.UserId.Equals(Id) && m.EPrescriptionId.Equals(EprescriptionId)).FirstOrDefault();
                Mapper.CreateMap<ShareReportFeedBack, ShareReportFeedBackViewModel>();
                ShareReportFeedBackViewModel objs = Mapper.Map<ShareReportFeedBackViewModel>(list);
                return objs;
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
                var rec = _db.ActivityMaster.FirstOrDefault(m => m.ActivityId == ActivityId);
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
    }
}


