using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using AutoMapper;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {
        //public List<All>

        public List<HealthConditionViewModel> GetHealthConditionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.HealthCondition
                           join k in _db.HealthConditionMaster on p.ConditionType equals k.Id
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new HealthConditionViewModel
                           {
                               Id = p.Id,
                               ConditionType = p.ConditionType,
                               HealthCondition = k.HealthCondition,
                               StillHaveCondition = p.StillHaveCondition,
                               strStillHaveCondition = GetYesorNo(p.StillHaveCondition),
                               DiagnosisDate = p.DiagnosisDate,
                               strDiagnosisDate = GetDateCustomTimeString(p.DiagnosisDate),
                               ServiceDate = p.ServiceDate,
                               strServiceDate = GetDateCustomTimeString(p.ServiceDate),
                               Provider = p.Provider,                           
                               Notes = p.Notes,
                               CreatedDate = p.CreatedDate,
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.HealthCondition.Contains(searchString));
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
                item.sno = _serialNo++;
            }

            return rs;
        }

        public int DeleteHealthCondition(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.HealthCondition.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetHealthConditionNameById(int Id)
        {
            try
            {
                var res = _db.HealthConditionMaster.Where(m => m.Id == Id).FirstOrDefault();
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
  

        public bool AddHealthCondition(HealthConditionViewModel oConditions,  out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oConditions != null && oConditions.ConditionType != 0)
                {
                    oConditions.Id = Guid.NewGuid();
                    Mapper.CreateMap<HealthConditionViewModel, HealthCondition>();
                    HealthCondition objConditions = Mapper.Map<HealthCondition>(oConditions);
                    objConditions.CreatedDate = DateTime.Now;
                    objConditions.ModifiedDate = DateTime.Now;
                    _db.HealthCondition.Add(objConditions);
                    int res = _db.SaveChanges();
                    objConditions = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            actId = oConditions.Id;
            oConditions = null;
            return _result;
        }

        public bool UpdateHealthCondition(HealthConditionViewModel oConditions)
        {
            bool result = false;
            try
            {
                var record = _db.HealthCondition.FirstOrDefault(m => m.Id.Equals(oConditions.Id));
                if (record != null)
                {
                    record.StillHaveCondition = oConditions.StillHaveCondition;
                    record.DiagnosisDate = oConditions.DiagnosisDate;
                    record.ServiceDate = oConditions.ServiceDate;
                    record.Provider = oConditions.Provider;
                    record.Notes = oConditions.Notes;
                    record.ModifiedDate = DateTime.Now;                   
                    record.Notes = oConditions.Notes;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public HealthConditionViewModel GetHealthConditionById(Guid Id)
        {
            HealthConditionViewModel oHealthConditionViewModel = null;
            try
            {
                HealthCondition oConditions = _db.HealthCondition.FirstOrDefault(m => m.Id.Equals(Id));
                if (oConditions != null)
                {
                    Mapper.CreateMap<HealthCondition, HealthConditionViewModel>();
                    oHealthConditionViewModel = Mapper.Map<HealthConditionViewModel>(oConditions);
                    oConditions = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oHealthConditionViewModel;
        }

        public List<HealthConditionMasterViewModel> GetHealthConditionMaster(string init)
        {
            try
            {
                var list = (from k in _db.HealthConditionMaster
                            where k.HealthCondition.ToLower().StartsWith(init.ToLower())
                            select new HealthConditionMaster
                            {
                                Id = k.Id,
                                HealthCondition = k.HealthCondition.ToLower()
                            }).OrderBy(m => m.HealthCondition).ToList();
                //var list = _db.HealthConditionMaster.Where(m => m.HealthCondition.ToLower().StartsWith(init.ToLower())).ToList();
                Mapper.CreateMap<HealthConditionMaster, HealthConditionMasterViewModel>();
                List<HealthConditionMasterViewModel> lstHealthConditionMasterViewModel = Mapper.Map<List<HealthConditionMaster>, List<HealthConditionMasterViewModel>>(list);
                return lstHealthConditionMasterViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<HealthConditionViewModel> GetHealthConditionCompleteList(Guid Id)
        {
            try
            {
                var list = _db.HealthCondition.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<HealthCondition, HealthConditionViewModel>();
                List<HealthConditionViewModel> lstHealthConditionViewModel = Mapper.Map<List<HealthCondition>, List<HealthConditionViewModel>>(list);
                list = null;
                return lstHealthConditionViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
