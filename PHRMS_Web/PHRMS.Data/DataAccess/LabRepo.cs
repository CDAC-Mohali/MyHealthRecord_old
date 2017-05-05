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

        public List<LabTestViewModel> GetLabTestResultGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.LabTest
                           join k in _db.LabTestMaster on p.TestId equals k.Id
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new LabTestViewModel
                           {
                               Id = p.Id,
                               TestName = k.TestName,

                               PerformedDate = p.PerformedDate,
                               strPerformedDate = GetDateCustomTimeString(p.PerformedDate),
                               Comments = p.Comments,
                               Result = p.Result,
                               Unit = p.Unit,
                               CreatedDate = p.CreatedDate,
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(x => x.TestName.Contains(searchString));
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

        public int DeleteResult(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.LabTest.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetTestNameById(int Id)
        {
            try
            {
                var res = _db.LabTestMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.TestName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public int GetTestIdByName(String name)
        {
            try
            {
                var res = _db.LabTestMaster.Where(m => m.TestName == name).FirstOrDefault();
                if (res != null)
                {
                    return res.Id;
                }
            }
            catch (Exception)
            {
            }

            return 0;
        }

        public bool AddTest(LabTestViewModel oTests, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oTests != null && oTests.TestId != 0)
                {
                    oTests.Id = Guid.NewGuid();
                    Mapper.CreateMap<LabTestViewModel, LabTest>();
                    LabTest objTests = Mapper.Map<LabTest>(oTests);
                    objTests.CreatedDate = DateTime.Now;
                    objTests.ModifiedDate = DateTime.Now;
                    _db.LabTest.Add(objTests);
                    int res = _db.SaveChanges();
                    objTests = null;
                    _result = res > 0;
                }
            }
            catch (Exception ex)
            {
            }
            actId = oTests.Id;
            oTests = null;
            return _result;
        }

        public bool UpdateResult(LabTestViewModel oResults)
        {
            bool result = false;
            try
            {
                var record = _db.LabTest.FirstOrDefault(m => m.Id.Equals(oResults.Id));
                if (record != null)
                {

                    record.PerformedDate = oResults.PerformedDate;
                    record.Result = oResults.Result;
                    record.Unit = oResults.Unit;
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

        public LabTestViewModel GetResultById(Guid Id)
        {
            LabTestViewModel oResultViewModel = null;
            try
            {
                LabTest oResults = _db.LabTest.FirstOrDefault(m => m.Id.Equals(Id));
                if (oResults != null)
                {
                    Mapper.CreateMap<LabTest, LabTestViewModel>();
                    oResultViewModel = Mapper.Map<LabTestViewModel>(oResults);
                    oResults = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oResultViewModel;
        }


        public List<LabTestMasterViewModel> GetLabTestMaster(string str)
        {
            try
            {
                var list = (from k in _db.LabTestMaster
                            where k.TestName.ToLower().StartsWith(str.ToLower())
                            select new LabTestMaster
                            {
                                Id = k.Id,
                                TestName = k.TestName.ToLower()
                            }).OrderBy(m => m.TestName).ToList();
                //var list = _db.LabTestMaster.Where(m => m.TestName.ToLower().StartsWith(str.ToLower())).ToList();
                Mapper.CreateMap<LabTestMaster, LabTestMasterViewModel>();
                List<LabTestMasterViewModel> lstLabTestMasterViewModel = Mapper.Map<List<LabTestMaster>, List<LabTestMasterViewModel>>(list);
                return lstLabTestMasterViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<LabTestViewModel> GetLabTestCompleteList(Guid Id)
        {
            try
            {
                var list = _db.LabTest.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<LabTest, LabTestViewModel>();
                List<LabTestViewModel> lstLabTestViewModel = Mapper.Map<List<LabTest>, List<LabTestViewModel>>(list);
                list = null;
                return lstLabTestViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
