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

        public List<ProceduresViewModel> GetProceduresGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Procedures
                           join k in _db.ProcedureMaster on p.ProcedureType equals k.Id
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new ProceduresViewModel
                           {
                               Id = p.Id,
                               StartDate = p.StartDate,
                               EndDate = p.EndDate,
                               CreatedDate = p.CreatedDate,
                               ProcedureType = p.ProcedureType,
                               UserId = p.UserId,
                               ProcedureName = k.ProcedureName,
                               SurgeonName = p.SurgeonName,
                               strStartDate = GetDateCustomTimeString(p.StartDate),
                               strEndDate = GetDateCustomTimeString(p.EndDate),
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.ProcedureName.Contains(searchString));
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

        public int DeleteProcedure(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Procedures.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetProcedureNameById(int Id)
        {
            try
            {
                var res = _db.ProcedureMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.ProcedureName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public bool AddProcedure(ProceduresViewModel oProcedure, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oProcedure != null && oProcedure.ProcedureType != 0)
                {
                    oProcedure.Id = Guid.NewGuid();
                    Mapper.CreateMap<ProceduresViewModel, Procedures>();
                    Procedures objProcedures = Mapper.Map<Procedures>(oProcedure);
                    objProcedures.CreatedDate = DateTime.Now;
                    objProcedures.ModifiedDate = DateTime.Now;
                    _db.Procedures.Add(objProcedures);
                    int res = _db.SaveChanges();
                    objProcedures = null;
                  
                    _result = res > 0;
                }
            }
            catch (Exception ex)
            {
            }
            actId = oProcedure.Id;
            oProcedure = null;
            return _result;
        }

        public bool UpdateProcedure(ProceduresViewModel oProcedure)
        {
            bool result = false;
            try
            {
                var record = _db.Procedures.FirstOrDefault(m => m.Id.Equals(oProcedure.Id));
                if (record != null)
                {
                    record.StartDate = oProcedure.StartDate;
                    record.EndDate = oProcedure.EndDate;
                    record.ModifiedDate = DateTime.Now;
                    record.Comments = oProcedure.Comments;
                    record.SurgeonName = oProcedure.SurgeonName; 
                    result = _db.SaveChanges() > 0;
                    oProcedure = null;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public ProceduresViewModel GetProcedureById(Guid Id)
        {
            ProceduresViewModel oProceduresViewModel = null;
            try
            {
                Procedures oProcedures = _db.Procedures.FirstOrDefault(m => m.Id.Equals(Id));

                if (oProcedures != null)
                {
                    Mapper.CreateMap<Procedures, ProceduresViewModel>();
                    oProceduresViewModel = Mapper.Map<ProceduresViewModel>(oProcedures);
                    oProcedures = null;
                }

            }
            catch (Exception)
            {

                throw;
            }
            return oProceduresViewModel;
        }

        public List<ProcedureMasterViewModel> GetProcedureMaster(string str)
        {
            try
            {
                var list = (from k in _db.ProcedureMaster
                            where k.ProcedureName.StartsWith(str)
                            select new ProcedureMaster
                            {
                                Id = k.Id,
                                ProcedureName = k.ProcedureName
                            }).OrderBy(m => m.ProcedureName).ToList();
                //var list = _db.ProcedureMaster.Where(m => m.ProcedureName.ToLower().StartsWith(str.ToLower())).ToList();
                Mapper.CreateMap<ProcedureMaster, ProcedureMasterViewModel>();
                List<ProcedureMasterViewModel> lstProcedureMasterViewModel = Mapper.Map<List<ProcedureMaster>, List<ProcedureMasterViewModel>>(list);
                list = null;
                return lstProcedureMasterViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<ProceduresViewModel> GetProceduresCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Procedures.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<Procedures, ProceduresViewModel>();
                List<ProceduresViewModel> lstProceduresViewModel = Mapper.Map<List<Procedures>, List<ProceduresViewModel>>(list);
                list = null;
                return lstProceduresViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
