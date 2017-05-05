using System;
using System.Linq;
using PHRMS.ViewModels;
using AutoMapper;
using System.Collections.Generic;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {

        public List<MedicalContactRecordsViewModel> GetContactGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.MedicalContactRecords
                           join k in _db.ContactTypes on p.MedContType equals k.Id
                           where p.UserId.Equals(userId) && p.DeleteFlag != true
                           select new MedicalContactRecordsViewModel
                           {
                               Id = p.Id,
                               ContactName = p.ContactName,
                               MedContType = k.Id,
                               strMedContType = k.MedContType,
                               PrimaryPhone = p.PrimaryPhone,
                               CreatedDate = p.CreatedDate,
                               EmailAddress = p.EmailAddress,
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(k => k.ContactName.Contains(searchString));
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
                item.sno = _serialNo++;
            }

            return rs;
        }

        public int DeleteContact(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.MedicalContactRecords.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetContactNameById(int Id)
        {
            try
            {
                var res = _db.ContactTypes.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.MedContType;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }
        public bool AddContact(MedicalContactRecordsViewModel oContact)
        {
            bool _result = false;
            try
            {
                if (oContact != null)
                {
                    oContact.Id = Guid.NewGuid();

                    Mapper.CreateMap<MedicalContactRecordsViewModel, MedicalContactRecords>();
                    MedicalContactRecords objContact = Mapper.Map<MedicalContactRecords>(oContact);
                    objContact.CreatedDate = DateTime.Now;
                    objContact.ModifiedDate = DateTime.Now;
                    _db.MedicalContactRecords.Add(objContact);
                    int res = _db.SaveChanges();
                    oContact = null;
                    objContact = null;
                    _result = res > 0;
                }
            }
            catch (Exception ex)
            {
            }
            return _result;
        }

        public bool UpdateContact(MedicalContactRecordsViewModel oContact)
        {
            bool result = false;
            try
            {
                var record = _db.MedicalContactRecords.FirstOrDefault(m => m.Id.Equals(oContact.Id));
                if (record != null)
                {
                    record.ContactName = oContact.ContactName;
                    record.MedContType = oContact.MedContType;
                    record.PrimaryPhone = oContact.PrimaryPhone;
                    record.ModifiedDate = DateTime.Now;
                    record.EmailAddress = oContact.EmailAddress;
                    record.Address1 = oContact.Address1;
                    record.Address2 = oContact.Address2;
                    record.CityVillage = oContact.CityVillage;
                    record.District = oContact.District;
                    record.State = oContact.State;
                    record.PIN = oContact.PIN;
                    record.ClinicName = oContact.ClinicName;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        public MedicalContactRecordsViewModel GetContactById(Guid Id)
        {
            MedicalContactRecordsViewModel oMedicalContactRecordsViewModel = null;
            try
            {
                MedicalContactRecords oContact = _db.MedicalContactRecords.FirstOrDefault(m => m.Id.Equals(Id));
                if (oContact != null)
                {

                    Mapper.CreateMap<MedicalContactRecords, MedicalContactRecordsViewModel>();
                    oMedicalContactRecordsViewModel = Mapper.Map<MedicalContactRecordsViewModel>(oContact);
                    if (oContact.State != 0)
                        oMedicalContactRecordsViewModel.strState = _db.States.FirstOrDefault(m => m.Id.Equals(oContact.State)).Name;
                    oContact = null;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return oMedicalContactRecordsViewModel;
        }


        public string GetContactTypeById(int Id)
        {
            try
            {
                var res = _db.ContactTypes.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.MedContType;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }


    }
}
