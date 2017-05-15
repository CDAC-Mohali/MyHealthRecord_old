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
        public List<AllergyViewModel> GetAllergiesGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Allergies
                           join k in _db.AllergyMaster on p.AllergyType equals k.Id
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new AllergyViewModel
                           {
                               Id = p.Id,
                               //StartDate = p.StartDate,
                               //EndDate = p.EndDate,
                               Still_Have = p.Still_Have,
                               CreatedDate = p.CreatedDate,
                               AllergyType = p.AllergyType,
                               UserId = p.UserId,
                               AllergyName = k.AllergyName,
                               strDuration = GetAllergyDurationById(p.DurationId),
                               //strStartDate = GetDateCustomTimeString(p.StartDate),
                               //strEndDate = GetDateCustomTimeString(p.EndDate),
                               strStill_Have = GetYesorNo(p.Still_Have),
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.AllergyName.Contains(searchString));
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

        public int DeleteAllergy(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Allergies.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetAllergyNameById(int Id)
        {
            try
            {
                var res = _db.AllergyMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.AllergyName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public string GetSeverityById(int Id)
        {
            try
            {
                var res = _db.AllergySeverity.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Severity;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetSeverityNameById(int Id)
        {
            try
            {
                var res = _db.AllergySeverity.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Severity;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetAllergyDurationById(int Id)
        {
            try
            {
                var res = _db.AllergyDuration.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Duration;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetYesorNo(bool val)
        {
            return val ? "Yes" : "No";
        }

        public string GetDateCustomTimeString(DateTime oDate)
        {
            try
            {
                return (oDate != null && oDate != DateTime.MinValue) ? oDate.ToString("dd/MM/yyyy") : "-";
            }
            catch (Exception)
            {

                return "";
            }
        }

        public bool AddAllergies(AllergyViewModel oAllergies, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oAllergies != null && oAllergies.AllergyType != 0)
                {
                    oAllergies.Id = Guid.NewGuid();
                    Mapper.CreateMap<AllergyViewModel, Allergies>();
                    Allergies objAllergies = Mapper.Map<Allergies>(oAllergies);
                    objAllergies.CreatedDate = DateTime.Now;
                    objAllergies.ModifiedDate = DateTime.Now;
                    _db.Allergies.Add(objAllergies);
                    int res = _db.SaveChanges();
                    objAllergies = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            actId = oAllergies.Id;
            oAllergies = null;
            return _result;
        }

        public bool UpdateAllergy(AllergyViewModel oAllergies)
        {
            bool result = false;
            try
            {
                var record = _db.Allergies.FirstOrDefault(m => m.Id.Equals(oAllergies.Id));
                if (record != null)
                {
                    record.Severity = oAllergies.Severity;
                    //record.StartDate = oAllergies.StartDate;
                    //record.EndDate = oAllergies.EndDate;
                    record.Still_Have = oAllergies.Still_Have;
                    //record.reactDes = oAllergies.reactDes;
                    record.ModifiedDate = DateTime.Now;
                    record.Comments = oAllergies.Comments;
                    //record.AllergyType = oAllergies.AllergyType;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public void EditAllergies(Allergies oAllergies)
        {
            var obj = _db.Allergies.Where(m => m.Id.Equals(oAllergies.Id)).FirstOrDefault();
            obj.AllergyType = oAllergies.AllergyType;
            obj.Comments = oAllergies.Comments;
            //obj.StartDate = oAllergies.StartDate;
            obj.Still_Have = oAllergies.Still_Have;
            //obj.EndDate = oAllergies.EndDate;
            _db.SaveChanges();
        }

        public AllergyViewModel GetAllergyById(Guid Id)
        {
            AllergyViewModel oAllergyViewModel = null;
            try
            {
                Allergies oAllergies = _db.Allergies.FirstOrDefault(m => m.Id.Equals(Id));

                if (oAllergies != null)
                {
                    Mapper.CreateMap<Allergies, AllergyViewModel>();
                    oAllergyViewModel = Mapper.Map<AllergyViewModel>(oAllergies);
                    oAllergies = null;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return oAllergyViewModel;
        }

        public List<AllergyMasterViewModel> GetAllergyMaster(string init)
        {
            try
            {
                //var list = _db.AllergyMaster.Where(m => m.AllergyName.ToLower().StartsWith(init.ToLower())).OrderBy(m => m.AllergyName).ToList();
                var list = (from k in _db.AllergyMaster
                            where k.AllergyName.ToLower().StartsWith(init.ToLower())
                            select new AllergyMaster
                            {
                                Id = k.Id,
                                AllergyName = k.AllergyName.ToLower()
                            }).OrderBy(m => m.AllergyName).ToList();
                Mapper.CreateMap<AllergyMaster, AllergyMasterViewModel>();
                List<AllergyMasterViewModel> lstAllergyMasterViewModel = Mapper.Map<List<AllergyMaster>, List<AllergyMasterViewModel>>(list);
                return lstAllergyMasterViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AllergyDurationViewModel> GetAllergyDurationList()
        {
            var list = _db.AllergyDuration.ToList();
            Mapper.CreateMap<AllergyDuration, AllergyDurationViewModel>();
            List<AllergyDurationViewModel> lstAllergyDuration = Mapper.Map<List<AllergyDuration>, List<AllergyDurationViewModel>>(list);
            return lstAllergyDuration;
        }

        public List<AllergySeverityViewModel> GetAllergySeverities()
        {
            try
            {
                var list = _db.AllergySeverity.ToList();
                Mapper.CreateMap<AllergySeverity, AllergySeverityViewModel>();
                List<AllergySeverityViewModel> lstAllergySeverityViewModel = Mapper.Map<List<AllergySeverity>, List<AllergySeverityViewModel>>(list);
                list = null;
                return lstAllergySeverityViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<AllergyViewModel> GetAllergiesCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Allergies.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId!=2).ToList();
                Mapper.CreateMap<Allergies, AllergyViewModel>();
                List<AllergyViewModel> lstAllergyViewModel = Mapper.Map<List<Allergies>, List<AllergyViewModel>>(list);
                list = null;
                return lstAllergyViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }



    }
}
