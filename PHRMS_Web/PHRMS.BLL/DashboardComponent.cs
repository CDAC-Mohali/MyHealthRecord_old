using System;
using PHRMS.ViewModels;
using System.Collections.Generic;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public DashboardAnalyticsViewModel UpdateAnalytics(Guid userId)
        {
            try
            {
                return _repository.UpdateAnalytics(userId);
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
        public List<BPViewModel> GetBPandPulseData(Guid userId)
        {
            try
            {
                return _repository.GetBPandPulseData(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetWeightData(Guid userId)
        {
            try
            {
                return _repository.GetWeightData(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetGlucoseData(Guid userId)
        {
            try
            {
                return _repository.GetGlucoseData(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetActivityData(Guid userId)
        {
            try
            {
                return _repository.GetActivityData(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetLatestAllergies(Guid userId)
        {
            List<string[]> result = null;
            try
            {
                result = _repository.GetLatestAllergies(userId);
            }
            catch (Exception)
            {
            }
            return result;
        }
        public List<string[]> GetLatestImmunizations(Guid userId)
        {
            try
            {
                return _repository.GetLatestImmunizations(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetLatestLabs(Guid userId)
        {
            try
            {
                return _repository.GetLatestLabs(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetLatestMedications(Guid userId)
        {
            try
            {
                return _repository.GetLatestMedications(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<string[]> GetLatestProcedures(Guid userId)
        {
            try
            {
                return _repository.GetLatestProcedures(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public string GetLatestActivities(Guid userId)
        {
            try
            {
                return _repository.GetLatestActivities(userId);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public string GetHealthTip()
        {
            string str = "";
            try
            {
                Random random = new Random();
                int num = random.Next(1,99);
                str = _repository.GetHealthTip(num);
            }
            catch (Exception)
            {
            }
            return str;
        }
    }
}