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
        //public List<All>

        public List<ImmunizationViewModel> GetImmunizationGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            List<ImmunizationViewModel> rs = null;
            try
            {
                int _serialNo = 1;
                var records = (from p in _db.Immunizations
                               join k in _db.Immunizationsmasters on p.ImmunizationsTypeId equals k.ImmunizationsTypeId
                               where p.UserId.Equals(userId) && p.DeleteFlag != true && p.SourceId != 2
                               select new ImmunizationViewModel
                               {
                                   Id = p.Id,
                                   ImmunizationName = k.ImmunizationName,
                                   ImmunizationDate = p.ImmunizationDate,
                                   strImmunizationDate = GetDateCustomTimeString(p.ImmunizationDate),
                                   ImmunizationsTypeId = p.ImmunizationsTypeId,
                                   Comments = p.Comments,
                                   CreatedDate = p.CreatedDate,
                                   SourceId = p.SourceId
                               }).AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchString))
                {
                    records = records.Where(k => k.ImmunizationName.Contains(searchString));
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

        public int DeleteImmunization(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Immunizations.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetImmunizationNameById(int Id)
        {
            try
            {
                var res = _db.Immunizationsmasters.Where(m => m.ImmunizationsTypeId == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.ImmunizationName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }



        public bool AddImmunization(ImmunizationViewModel oImmunization, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oImmunization != null)
                {
                    oImmunization.Id = Guid.NewGuid();
                    Mapper.CreateMap<ImmunizationViewModel, Immunizations>();
                    Immunizations objImmunization = Mapper.Map<Immunizations>(oImmunization);
                    objImmunization.CreatedDate = DateTime.Now;
                    objImmunization.ModifiedDate = DateTime.Now;
                    _db.Immunizations.Add(objImmunization);
                    int res = _db.SaveChanges();
                    objImmunization = null;
                    _result = res > 0;
                }
            }
            catch (Exception ex)
            {
            }
            actId = oImmunization.Id;
            oImmunization = null;
            return _result;
        }

        public bool UpdateImmunization(ImmunizationViewModel oImmunization)
        {
            bool result = false;
            try
            {
                var record = _db.Immunizations.FirstOrDefault(m => m.Id.Equals(oImmunization.Id));
                if (record != null)
                {
                    record.Comments = oImmunization.Comments;
                    record.ImmunizationDate = oImmunization.ImmunizationDate;
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

        public ImmunizationViewModel GetImmunizationById(Guid Id)
        {
            ImmunizationViewModel oImmunizationViewModel = null;
            try
            {
                Immunizations oImmunization = _db.Immunizations.FirstOrDefault(m => m.Id.Equals(Id));
                if (oImmunization != null)
                {
                    Mapper.CreateMap<Immunizations, ImmunizationViewModel>();
                    oImmunizationViewModel = Mapper.Map<ImmunizationViewModel>(oImmunization);
                    oImmunization = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oImmunizationViewModel;
        }

        public List<ImmunizationsMastersViewModel> GetImmunizationMaster(string init)
        {
            try
            {
                var list = (from k in _db.Immunizationsmasters
                            where k.ImmunizationName.ToLower().StartsWith(init.ToLower())
                            select new ImmunizationsMasters
                            {
                                ImmunizationsTypeId = k.ImmunizationsTypeId,
                                ImmunizationName = k.ImmunizationName.ToLower()
                            }).OrderBy(m => m.ImmunizationName).ToList();
                //var list = _db.Immunizationsmasters.Where(m => m.ImmunizationName.ToLower().StartsWith(init.ToLower())).ToList();
                Mapper.CreateMap<ImmunizationsMasters, ImmunizationsMastersViewModel>();
                List<ImmunizationsMastersViewModel> lstImmunizationsMastersViewModel = Mapper.Map<List<ImmunizationsMasters>, List<ImmunizationsMastersViewModel>>(list);
                return lstImmunizationsMastersViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ImmunizationViewModel> GetImmunizationCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Immunizations.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<Immunizations, ImmunizationViewModel>();
                List<ImmunizationViewModel> lstImmunizationViewModel = Mapper.Map<List<Immunizations>, List<ImmunizationViewModel>>(list);
                list = null;
                return lstImmunizationViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
