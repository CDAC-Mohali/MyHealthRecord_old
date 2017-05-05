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

        public List<EprescriptionViewModel> GetSharePrescriptionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Eprescription
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId == 5
                           select new EprescriptionViewModel
                           {
                               Id = p.Id,
                               DocName = p.DocName,
                               ClinicName = p.ClinicName,
                               DocAddress = p.DocAddress,
                               DocPhone = p.DocPhone,
                               Prescription = p.Prescription,
                               PresDate = p.PresDate,
                               strPresDate = GetDateCustomTimeString(p.PresDate),
                               CreatedDate = p.CreatedDate,
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.DocName.Contains(searchString));
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

        public List<EprescriptionViewModel> GetPrescriptionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Eprescription
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId!=5
                           select new EprescriptionViewModel
                           {
                               Id = p.Id,
                               DocName = p.DocName,
                               ClinicName = p.ClinicName,
                               DocAddress = p.DocAddress,
                               DocPhone = p.DocPhone,
                               Prescription = p.Prescription,
                               PresDate = p.PresDate,
                               strPresDate = GetDateCustomTimeString(p.PresDate),
                               CreatedDate = p.CreatedDate,
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.DocName.Contains(searchString));
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

        public int DeletePrescription(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Eprescription.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public bool AddPrescription(EprescriptionViewModel oPrescription, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oPrescription != null)
                {
                    oPrescription.Id = Guid.NewGuid();
                    Mapper.CreateMap<EprescriptionViewModel, Eprescription>();
                    Eprescription objPrescription = Mapper.Map<Eprescription>(oPrescription);
                    objPrescription.CreatedDate = DateTime.Now;
                    objPrescription.ModifiedDate = DateTime.Now;
                    _db.Eprescription.Add(objPrescription);
                    int res = _db.SaveChanges();
                    objPrescription = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            actId = oPrescription.Id;
            oPrescription = null;
            return _result;
        }

        public List<EprescriptionViewModel> GetEprescriptionCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Eprescription.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag).ToList();
                Mapper.CreateMap<Eprescription, EprescriptionViewModel>();
                List<EprescriptionViewModel> lstEprescriptionViewModel = Mapper.Map<List<Eprescription>, List<EprescriptionViewModel>>(list);
                list = null;
                return lstEprescriptionViewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdatePrescription(EprescriptionViewModel oPrescription)
        {
            bool result = false;
            try
            {
                var record = _db.Eprescription.FirstOrDefault(m => m.Id.Equals(oPrescription.Id));
                if (record != null)
                {
                    record.DocName = oPrescription.DocName;
                    record.DocAddress = oPrescription.DocAddress;
                    record.PresDate = oPrescription.PresDate;
                    record.Prescription = oPrescription.Prescription;
                    record.DocPhone = oPrescription.DocPhone;
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

        public EprescriptionViewModel GetPrescriptionById(Guid Id)
        {
            long result = 0;
            bool isViewed = false;
            EprescriptionViewModel oPrescriptionViewModel = null;
            try
            {
                Eprescription oPrescription = _db.Eprescription.FirstOrDefault(m => m.Id.Equals(Id));
                if(oPrescription.SourceId == 5)
                {
                    var recs = (from p in _db.Eprescription
                                join k in _db.ShareReportFeedBack on p.Id equals k.EPrescriptionId
                                join r in _db.ShareReportNotification on k.UserId equals r.UserId
                                where oPrescription.Id.Equals(k.EPrescriptionId) && !p.DeleteFlag && p.UserId.Equals(k.UserId)
                                && k.UserRecordId.Equals(r.UserRecordId) 
                                orderby p.CreatedDate descending
                                select new { p,k,r }).Take(1).ToList();
                    if (recs.Count!=0 )
                    {
                        foreach (var item in recs)
                        {
                            result = item.r.UserRecordId;
                            isViewed = item.r.isNotificationViewed;
                        }
                        if(!isViewed)

                        {
                            var notRecord = _db.ShareReportNotification.FirstOrDefault(s => s.UserRecordId.Equals(result));
                            notRecord.isNotificationViewed = true;
                            _db.SaveChanges();
                        }
                       
                    }
                }

                if (oPrescription != null)
                {
                    Mapper.CreateMap<Eprescription, EprescriptionViewModel>();
                    oPrescriptionViewModel = Mapper.Map<EprescriptionViewModel>(oPrescription);
                    oPrescription = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oPrescriptionViewModel;
        }


        public PersonalInformation GetPersonById(PersonalInformation obj)
        {
            var test = _db.PersonalInformation.FirstOrDefault(c => c.UserId.Equals(obj.UserId));
            return test;
        }



    }
}
