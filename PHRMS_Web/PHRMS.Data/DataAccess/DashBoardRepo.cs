using AutoMapper;
using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {

        public DashboardAnalyticsViewModel UpdateAnalytics(Guid userId)
        {
            DashboardAnalyticsViewModel oAnalytics = new DashboardAnalyticsViewModel();
            try
            {
                oAnalytics.AllergiesCount = _db.Allergies.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId!=2).Count();
                oAnalytics.ImmunizationsCount = _db.Immunizations.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId != 2).Count();
                oAnalytics.LabsCount = _db.LabTest.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId != 2).Count();
                oAnalytics.MedicationsCount = _db.Medication.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId != 2).Count();
                oAnalytics.ProceduresCount = _db.Procedures.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId!=2).Count();
                oAnalytics.BloodGlucoseCount = _db.BloodGlucose.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId!=2).Count();
                var bpRec = _db.BloodPressureAndPulse.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId != 2).OrderByDescending(l => l.CreatedDate).FirstOrDefault();
                if (bpRec != null)
                {
                    oAnalytics.BPLatestSys = float.Parse(bpRec.ResSystolic);
                    oAnalytics.BPLatestDia = float.Parse(bpRec.ResDiastolic);
                    oAnalytics.LastBPCollectionDate = bpRec.CollectionDate.ToString("dd-MMM-yy");

                }
                var glucoseRec = _db.BloodGlucose.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId != 2).OrderByDescending(l => l.CreatedDate).FirstOrDefault();
                if (glucoseRec != null)
                {
                    oAnalytics.GlucoseLatest = float.Parse(glucoseRec.Result);
                    oAnalytics.LastGlucoseCollectionDate = glucoseRec.CollectionDate.ToString("dd-MMM-yy");
                }
                var ActivityRec = _db.Activities.Where(m => m.DeleteFlag == false && m.UserId == userId && m.SourceId != 2).OrderByDescending(l => l.CreatedDate).FirstOrDefault();
                if (ActivityRec != null)
                {
                    oAnalytics.LastActivityCollectionDate = ActivityRec.CollectionDate.ToString("dd-MMM-yy");
                    int activityId = ActivityRec.ActivityId;
                    string activityName = _db.ActivityMaster.Where(m => m.ActivityId.Equals(activityId)).FirstOrDefault().ActivityName;
                    oAnalytics.LastActivityType = activityName;
                    oAnalytics.LastActivityDistance = Math.Floor(ActivityRec.Distance);
                }
            }
            catch (Exception ex)
            {


            }
            return oAnalytics;
        }

        public List<string[]> GetLatestAllergies(Guid userId)
        {
            List<string[]> lstAllergies = null;
            try
            {
                var recs = (from p in _db.Allergies
                            join k in _db.AllergyMaster on p.AllergyType equals k.Id
                            join h in _db.AllergySeverity on p.Severity equals h.Id
                            where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                            orderby p.CreatedDate descending
                            select new { k, h, p.CreatedDate }).Take(1).ToList();

                lstAllergies = new List<string[]>();
                foreach (var item in recs)
                {
                    string severity = item.h.Severity.Replace("symptom", "");
                    string[] strArry = { item.k.AllergyName, severity, item.CreatedDate.ToString("dd-MMM-yy") };
                    lstAllergies.Add(strArry);
                    strArry = null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lstAllergies;
        }


        public List<string[]> GetLatestImmunizations(Guid userId)
        {
            List<string[]> lstImmunizations = null;
            try
            {
                var recs = (from p in _db.Immunizations
                            join k in _db.Immunizationsmasters on p.ImmunizationsTypeId equals k.ImmunizationsTypeId
                            where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                            orderby p.CreatedDate descending
                            select new { p, k }).Take(1).ToList();

                lstImmunizations = new List<string[]>();
                foreach (var item in recs)
                {
                    string[] strArry = { item.k.ImmunizationName, item.p.ImmunizationDate.ToString() };
                    lstImmunizations.Add(strArry);
                    strArry = null;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return lstImmunizations;
        }

        public List<string[]> GetLatestLabs(Guid userId)
        {
            List<string[]> lstLabsTest = null;
            try
            {
                var recs = (from p in _db.LabTest
                            join k in _db.LabTestMaster on p.TestId equals k.Id
                            where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                            orderby p.CreatedDate descending
                            select new { p, k }).Take(1).ToList();

                lstLabsTest = new List<string[]>();
                foreach (var item in recs)
                {
                    string[] strArry = { item.k.TestName, item.p.PerformedDate.ToString() };
                    lstLabsTest.Add(strArry);
                    strArry = null;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return lstLabsTest;
        }


        public List<string[]> GetLatestMedications(Guid userId)
        {
            List<string[]> lstMedications = null;
            try
            {
                var recs = (from p in _db.Medication
                            join k in _db.MedicationMaster on p.MedicineType equals k.Id
                            where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                            orderby p.CreatedDate descending
                            select new { p, k }).Take(1).ToList();

                lstMedications = new List<string[]>();
                foreach (var item in recs)
                {
                    string[] strArry = { item.k.MedicineName, item.p.PrescribedDate.ToString() };
                    lstMedications.Add(strArry);
                    strArry = null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lstMedications;
        }


        public List<string[]> GetLatestProcedures(Guid userId)
        {
            List<string[]> lstProcedures = null;
            try
            {
                var recs = (from p in _db.Procedures
                            join k in _db.ProcedureMaster on p.ProcedureType equals k.Id
                            where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                            orderby p.EndDate descending
                            select new { p, k }).Take(1).ToList();

                lstProcedures = new List<string[]>();
                foreach (var item in recs)
                {
                    string[] strArry = { item.k.ProcedureName, item.p.EndDate.ToString() };
                    lstProcedures.Add(strArry);
                    strArry = null;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return lstProcedures;
        }

        public string GetLatestActivities(Guid userId)
        {
            var recs = from p in _db.Activities where p.SourceId!=2 && p.UserId.Equals(userId) && !p.DeleteFlag orderby p.CreatedDate descending group p by p.CreatedDate into g select g.FirstOrDefault();
            decimal total_distance = 0;
            foreach (var item in recs)
            {
                total_distance += item.Distance;
            }
            return total_distance.ToString();
        }

        public List<BPViewModel> GetBPandPulseData(Guid userId)
        {
            var Res = (from p in _db.BloodPressureAndPulse
                       where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                       orderby p.CollectionDate
                       select new BPViewModel
                       {
                           Diastolic = float.Parse((p.ResDiastolic == null || p.ResDiastolic == "") ? "0" : p.ResDiastolic),
                           Systolic = float.Parse((p.ResSystolic == null || p.ResSystolic == "") ? "0" : p.ResSystolic),
                           Pulse = int.Parse((p.ResPulse == null || p.ResPulse == "") ? "0" : p.ResPulse),
                           Date = p.CollectionDate.ToString("dd-MM-yy")
                       }).ToList();

            return Res;

        }

        public List<string[]> GetWeightData(Guid userId)
        {
            List<string[]> Res = null;

            var Rec = (from p in _db.Weight
                       where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                       orderby p.CollectionDate
                       select p
                       ).ToList();
            Res = new List<string[]>();
            foreach (var item in Rec)
            {
                String date = item.CollectionDate.ToString("dd-MM-yy");
                string[] strArry = { item.Result.ToString(), date };
                Res.Add(strArry);
                strArry = null;
            }
            return Res;

        }

        public List<string[]> GetGlucoseData(Guid userId)
        {
            List<string[]> Res = null;

            var Rec = (from p in _db.BloodGlucose
                       where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                       orderby p.CollectionDate
                       select p
                       ).ToList();
            Res = new List<string[]>();
            foreach (var item in Rec)
            {
                String date = item.CollectionDate.ToString("dd-MM-yy");
                string[] strArry = { item.Result, date };
                Res.Add(strArry);
                strArry = null;
            }
            return Res;

        }

        public List<string[]> GetActivityData(Guid userId)
        {
            List<string[]> Result = null;

            var Rec = (from p in _db.Activities
                       where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=2
                       orderby p.CollectionDate
                       select p
                       ).ToList();
            Result = new List<string[]>();
            foreach (var item in Rec)
            {
                String date = item.CollectionDate.ToString("dd-MM-yy");

                string[] stringArry = { item.Distance.ToString(), date, item.FinishTime.ToString(), item.ActivityId.ToString() };
                Result.Add(stringArry);
                stringArry = null;
            }
            return Result;

        }

        public string GetHealthTip(int Id)
        {
            string str = "";
            try
            {
                var rec = _db.HealthTip.FirstOrDefault(m => m.Id == Id);
                if (rec != null)
                {
                    str = rec.Tip;
                }
            }
            catch (Exception)
            {

            }
            return str;
        }


    }
}
