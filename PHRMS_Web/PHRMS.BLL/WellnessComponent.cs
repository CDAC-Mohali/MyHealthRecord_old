using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using System.Linq;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {

        //Activities Region
        public List<ActivityMasterViewModel> GetActivitiesMaster()
        {
            try
            {
                return _repository.GetActivitiesMaster();
            }
            catch (Exception)
            {
            }
            return null;
        }
        public List<ActivitiesViewModel> GetActivitiesRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetActivitiesRecordsGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }
        public bool AddActivity(ActivitiesViewModel oActivities)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                if (oActivities != null)
                {
                    if (oActivities.strCollectionDate != "")
                        oActivities.CollectionDate = DateTime.ParseExact(oActivities.strCollectionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oActivities.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddActivity(oActivities, out ActId);
                        if (result)
                            flag = 1;
                    }
                    else
                    {
                        ActId = oActivities.Id;
                        result = _repository.UpdateActivity(oActivities);
                        if (result)
                            flag = 2;
                    }
                }

            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 7;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 7;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }
        public int DeleteActivities(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteActivities(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 7;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }
        public bool UpdateActivity(ActivitiesViewModel oWellness)
        {

            return _repository.UpdateActivity(oWellness);
        }
        public ActivitiesViewModel GetActivitiesById(Guid Id)
        {
            ActivitiesViewModel oBP = null;

            try
            {
                oBP = _repository.GetActivitiesById(Id);

                if (oBP != null)
                {
                    oBP.strCollectionDate = (oBP.CollectionDate != null && oBP.CollectionDate != DateTime.MinValue) ? oBP.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                }
            }
            catch (Exception)
            { }
            return oBP;
        }

        public List<ActivitiesViewModel> GetActivitiesExportableList(Guid Id)
        {
            List<ActivitiesViewModel> lstActivitiesViewModel = null;
            try
            {
                lstActivitiesViewModel = _repository.GetActivitiesCompleteList(Id);
                if (lstActivitiesViewModel != null)
                {
                    foreach (var item in lstActivitiesViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstActivitiesViewModel;
        }

        public List<System.Data.DataRow> GetActivitiesTable(Guid Id)
        {
            List<ActivitiesViewModel> lstActivitiesViewModel = null;
            var activities = new System.Data.DataTable();
            try
            {
                lstActivitiesViewModel = _repository.GetActivitiesCompleteList(Id);
                if (lstActivitiesViewModel != null)
                {
                    activities.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    activities.Columns.Add(new System.Data.DataColumn("Activity Name", Type.GetType("System.String")));
                    activities.Columns.Add(new System.Data.DataColumn("Path Name", Type.GetType("System.String")));
                    activities.Columns.Add(new System.Data.DataColumn("Distance", Type.GetType("System.String")));
                    activities.Columns.Add(new System.Data.DataColumn("Total Time", Type.GetType("System.String")));
                    activities.Columns.Add(new System.Data.DataColumn("Col. Date", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstActivitiesViewModel.Count; i++)
                    {
                        data = new Object[6];
                        data[0] = i + 1;
                        data[1] = lstActivitiesViewModel[i].ActivityName;
                        data[2] = lstActivitiesViewModel[i].PathName;
                        data[3] = lstActivitiesViewModel[i].Distance;
                        data[4] = lstActivitiesViewModel[i].FinishTime;
                        data[5] = (lstActivitiesViewModel[i].CollectionDate != null && lstActivitiesViewModel[i].CollectionDate != DateTime.MinValue) ? lstActivitiesViewModel[i].CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        activities.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return activities.Select().ToList();
        }


        //Blood Pressure and Pulse Region 
        public List<BloodPressureAndPulseViewModel> GetBPAndPulseRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetBPAndPulseRecordsGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }
        public bool AddBloodPressure(BloodPressureAndPulseViewModel oActivities)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                if (oActivities != null)
                {
                    if (oActivities.strCollectionDate != "")
                        oActivities.CollectionDate = DateTime.ParseExact(oActivities.strCollectionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oActivities.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddBloodPressureAndPulse(oActivities, out ActId);
                        if (result)
                            flag = 1;
                    }
                    else
                    {
                        result = _repository.UpdateBloodPressureAndPulse(oActivities);
                        flag = 2;
                    }
                }

            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 8;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 8;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }
        public int DeleteBloodPressureAndPulse(Guid oGuid, Guid userId)
        {

            var res = _repository.DeleteBloodPressureAndPulse(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 8;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }
        public bool UpdateBloodPressureAndPulse(BloodPressureAndPulseViewModel oWellness)
        {

            return _repository.UpdateBloodPressureAndPulse(oWellness);
        }
        public BloodPressureAndPulseViewModel GetBloodPressureById(Guid Id)
        {
            BloodPressureAndPulseViewModel oBP = null;
            try
            {
                oBP = _repository.GetBloodPressureById(Id);

                if (oBP != null)
                {
                    oBP.strCollectionDate = (oBP.CollectionDate != null && oBP.CollectionDate != DateTime.MinValue) ? oBP.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                }
            }
            catch (Exception)
            { }
            return oBP;
        }

        public List<System.Data.DataRow> GetBloodPressureTable(Guid Id)
        {
            List<BloodPressureAndPulseViewModel> lstBPViewModel = null;
            var bppulse = new System.Data.DataTable();
            try
            {
                lstBPViewModel = _repository.GetBloodPressureAndPulseCompleteList(Id);
                if (lstBPViewModel != null)
                {
                    bppulse.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    bppulse.Columns.Add(new System.Data.DataColumn("Systolic", Type.GetType("System.String")));
                    bppulse.Columns.Add(new System.Data.DataColumn("Diastolic", Type.GetType("System.String")));
                    bppulse.Columns.Add(new System.Data.DataColumn("Pulse", Type.GetType("System.String")));
                    bppulse.Columns.Add(new System.Data.DataColumn("Comments", Type.GetType("System.String")));
                    bppulse.Columns.Add(new System.Data.DataColumn("Col. Date", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstBPViewModel.Count; i++)
                    {
                        data = new Object[6];
                        data[0] = i + 1;
                        data[1] = lstBPViewModel[i].ResSystolic;
                        data[2] = lstBPViewModel[i].ResDiastolic;
                        data[3] = lstBPViewModel[i].ResPulse;
                        data[4] = lstBPViewModel[i].Comments;
                        data[5] = (lstBPViewModel[i].CollectionDate != null && lstBPViewModel[i].CollectionDate != DateTime.MinValue) ? lstBPViewModel[i].CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        bppulse.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return bppulse.Select().ToList(); ;
        }

        public List<BloodPressureAndPulseViewModel> GetBloodPressureAndPulseExportableList(Guid Id)
        {
            List<BloodPressureAndPulseViewModel> lstBloodPressureAndPulseViewModel = null;
            try
            {
                lstBloodPressureAndPulseViewModel = _repository.GetBloodPressureAndPulseCompleteList(Id);
                if (lstBloodPressureAndPulseViewModel != null)
                {
                    foreach (var item in lstBloodPressureAndPulseViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstBloodPressureAndPulseViewModel;
        }

        //Blood Glucose Region
        public List<BloodGlucoseViewModel> GetBloodGlucoseRecordsGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetBloodGlucoseRecordsGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public List<BloodGlucoseViewModel> GetBloodGlucoseRecordsist(Guid Id)
        {
            return _repository.GetBloodGlucoseCompleteList(Id);
        }
        public ShareReportFeedBackViewModel GetShareReportFeedBack(Guid Id, Guid EprescriptionId)
        {
            return _repository.GetShareReportFeedBack(Id, EprescriptionId);
        }
        
        public bool AddBloodGlucose(BloodGlucoseViewModel oActivities)
        {
            bool result = false;
            var flag = 0;
            try
            {
                if (oActivities != null)
                {
                    if (oActivities.strCollectionDate != "")
                        oActivities.CollectionDate = DateTime.ParseExact(oActivities.strCollectionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oActivities.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddBloodGlucose(oActivities);
                        if (result)
                            flag = 1;
                    }
                    else
                    {
                        result = _repository.UpdateBloodGlucose(oActivities);
                        if (result)
                            flag = 2;
                    }
                }

            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 9;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 9;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }

        public List<System.Data.DataRow> GetBloodGlucoseTable(Guid Id)
        {
            List<BloodGlucoseViewModel> lstBloodGlucoseViewModel = null;
            var bloodGlucose = new System.Data.DataTable();
            try
            {
                lstBloodGlucoseViewModel = _repository.GetBloodGlucoseCompleteList(Id);
                if (lstBloodGlucoseViewModel != null)
                {
                    bloodGlucose.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    bloodGlucose.Columns.Add(new System.Data.DataColumn("Result", Type.GetType("System.String")));
                    bloodGlucose.Columns.Add(new System.Data.DataColumn("Value Type", Type.GetType("System.String")));
                    bloodGlucose.Columns.Add(new System.Data.DataColumn("Comments", Type.GetType("System.String")));
                    bloodGlucose.Columns.Add(new System.Data.DataColumn("Col. Date", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstBloodGlucoseViewModel.Count; i++)
                    {
                        data = new Object[5];
                        data[0] = i + 1;
                        data[1] = lstBloodGlucoseViewModel[i].Result;
                        data[2] = lstBloodGlucoseViewModel[i].ValueType;
                        data[3] = lstBloodGlucoseViewModel[i].Comments;
                        data[4] = (lstBloodGlucoseViewModel[i].CollectionDate != null && lstBloodGlucoseViewModel[i].CollectionDate != DateTime.MinValue) ? lstBloodGlucoseViewModel[i].CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        bloodGlucose.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return bloodGlucose.Select().ToList(); ;
        }

        public int DeleteBloodGlucose(Guid oGuid)
        {
            return _repository.DeleteBloodGlucose(oGuid);
        }
        public bool UpdateBloodGlucose(BloodGlucoseViewModel oWellness)
        {

            return _repository.UpdateBloodGlucose(oWellness);
        }
        public BloodGlucoseViewModel GetBloodGlucoseById(Guid Id)
        {
            BloodGlucoseViewModel oBP = null;
            try
            {
                oBP = _repository.GetBloodGlucoseById(Id);

                if (oBP != null)
                {
                    oBP.strCollectionDate = (oBP.CollectionDate != null && oBP.CollectionDate != DateTime.MinValue) ? oBP.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                }
            }
            catch (Exception)
            { }
            return oBP;
        }

        public List<BloodGlucoseViewModel> GetBloodGlucoseExportableList(Guid Id)
        {
            List<BloodGlucoseViewModel> lstBloodGlucoseViewModel = null;
            try
            {
                lstBloodGlucoseViewModel = _repository.GetBloodGlucoseCompleteList(Id);
                if (lstBloodGlucoseViewModel != null)
                {
                    foreach (var item in lstBloodGlucoseViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstBloodGlucoseViewModel;
        }

        #region Sleep Region
        public List<SleepViewModel> GetSleepGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetSleepGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }
        public bool AddSleep(SleepViewModel oActivities)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");

            try
            {
                if (oActivities != null)
                {
                    if (oActivities.strCollectionDate != "")
                        oActivities.CollectionDate = DateTime.ParseExact(oActivities.strCollectionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oActivities.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddSleep(oActivities, out ActId);
                        if (result)
                        {
                            flag = 1;
                        }
                    }
                    else
                    {
                        ActId = oActivities.Id;
                        result = _repository.UpdateSleep(oActivities);
                        if (result)
                        {
                            flag = 1;
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 10;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 10;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }
        public int DeleteSleep(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteSleep(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 10;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }
        public bool UpdateSleep(SleepViewModel oWellness)
        {
            return _repository.UpdateSleep(oWellness);
        }
        public SleepViewModel GetSleepById(Guid Id)
        {
            SleepViewModel oBP = null;
            try
            {
                oBP = _repository.GetSleepById(Id);

                if (oBP != null)
                {
                    oBP.strCollectionDate = (oBP.CollectionDate != null && oBP.CollectionDate != DateTime.MinValue) ? oBP.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                }
            }
            catch (Exception)
            { }
            return oBP;
        }
        #endregion



        //Weight Region 
        public List<WeightViewModel> GetWeightGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetWeightGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }
        public bool AddWeight(WeightViewModel oActivities)
        {
            bool result = false;
            var flag = 0;
            try
            {
                if (oActivities != null)
                {
                    if (oActivities.strCollectionDate != "")
                        oActivities.CollectionDate = DateTime.ParseExact(oActivities.strCollectionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oActivities.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddWeight(oActivities);
                        if (result)
                        {
                            flag = 1;
                        }
                    }
                    else
                    {
                        result = _repository.UpdateWeight(oActivities);
                        if (result)
                        {
                            flag = 2;
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 11;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 11;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }
        public int DeleteWeight(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteWeight(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 11;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }
        public bool UpdateWeight(WeightViewModel oWellness)
        {
            return _repository.UpdateWeight(oWellness);
        }
        public WeightViewModel GetWeightById(Guid Id)
        {
            WeightViewModel oBP = null;
            try
            {
                oBP = _repository.GetWeightById(Id);

                if (oBP != null)
                {
                    oBP.strCollectionDate = (oBP.CollectionDate != null && oBP.CollectionDate != DateTime.MinValue) ? oBP.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oBP.BMI = _repository.GetBMI(oBP.Goal, oBP.Result);
                }
            }
            catch (Exception)
            { }
            return oBP;
        }


        public List<System.Data.DataRow> GetWeightTable(Guid Id)
        {
            List<WeightViewModel> lstWeightViewModel = null;
            var weight = new System.Data.DataTable();
            try
            {
                lstWeightViewModel = _repository.GetWeightsCompleteList(Id);
                if (lstWeightViewModel != null)
                {
                    weight.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    weight.Columns.Add(new System.Data.DataColumn("Weight", Type.GetType("System.String")));
                    weight.Columns.Add(new System.Data.DataColumn("Height", Type.GetType("System.String")));
                    weight.Columns.Add(new System.Data.DataColumn("Comments", Type.GetType("System.String")));
                    weight.Columns.Add(new System.Data.DataColumn("Col. Date", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstWeightViewModel.Count; i++)
                    {
                        data = new Object[5];
                        data[0] = i + 1;
                        data[1] = lstWeightViewModel[i].Result;
                        data[2] = lstWeightViewModel[i].Goal;
                        data[3] = lstWeightViewModel[i].Comments;
                        data[4] = (lstWeightViewModel[i].CollectionDate != null && lstWeightViewModel[i].CollectionDate != DateTime.MinValue) ? lstWeightViewModel[i].CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        weight.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return weight.Select().ToList();
        }

        public List<WeightViewModel> GetWeightExportableList(Guid Id)
        {
            List<WeightViewModel> lstWeightViewModel = null;
            try
            {
                lstWeightViewModel = _repository.GetWeightsCompleteList(Id);
                if (lstWeightViewModel != null)
                {
                    foreach (var item in lstWeightViewModel)
                    {
                        item.strCollectionDate = (item.CollectionDate != null && item.CollectionDate != DateTime.MinValue) ? item.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.BMI = _repository.GetBMI(item.Goal, item.Result);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstWeightViewModel;
        }

        public List<string> GetBMIForGraph(Guid Id)
        {
            List<string> lstBMI = null;
            try
            {
                List<WeightViewModel> lstWeightViewModel = _repository.GetWeightsCompleteList(Id);
                lstBMI = new List<string>();
                if (lstWeightViewModel != null && lstWeightViewModel.Count > 0)
                {
                    
                    foreach (var item in lstWeightViewModel)
                    {
                        lstBMI.Add(string.Concat(item.CollectionDate.ToString("dd/MM/yy"),",", _repository.GetBMI(item.Goal, item.Result)));
                    }
                    lstWeightViewModel = null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return lstBMI;
        }

        #region Temperature Region
        public List<TemperatureViewModel> GetTemperatureGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetTemperatureGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public bool AddTemperature(TemperatureViewModel oActivities)
        {
            bool result = false;
            var flag = 0;
            try
            {
                if (oActivities != null)
                {
                    if (oActivities.strCollectionDate != "")
                        oActivities.CollectionDate = DateTime.ParseExact(oActivities.strCollectionDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oActivities.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddTemperature(oActivities);
                        if (result)
                        {
                            flag = 1;
                        }
                    }
                    else
                    {
                        result = _repository.UpdateTemperature(oActivities);
                        if (result)
                        {
                            flag = 2;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 14;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 14;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oActivities.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }
        public int DeleteTemperature(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteActivities(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.Module = 7;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }

        public bool UpdateTemperature(TemperatureViewModel oWellness)
        {
            return _repository.UpdateTemperature(oWellness);
        }

        public TemperatureViewModel GetTemperatureById(Guid Id)
        {
            TemperatureViewModel oBP = null;
            try
            {
                oBP = _repository.GetTemperatureById(Id);

                if (oBP != null)
                {
                    oBP.strCollectionDate = (oBP.CollectionDate != null && oBP.CollectionDate != DateTime.MinValue) ? oBP.CollectionDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                }
            }
            catch (Exception)
            { }
            return oBP;
        }
        #endregion
    }
}
