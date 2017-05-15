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

        public List<MedicationViewModel> GetMedicationGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Medication
                           join k in _db.MedicationMaster on p.MedicineType equals k.Id
                           where p.UserId.Equals(userId) && !p.DeleteFlag && p.SourceId != 2
                           select new MedicationViewModel
                           {
                               Id = p.Id,
                               MedicineType = p.MedicineType,
                               MedicineName = k.MedicineName,
                               TakingMedicine = p.TakingMedicine,
                               strTakingMedicine = GetYesorNo(p.TakingMedicine),
                               PrescribedDate = p.PrescribedDate,
                               strPrescribedDate = GetDateCustomTimeString(p.PrescribedDate),
                               DispensedDate = p.DispensedDate,
                               strDispensedDate = GetDateCustomTimeString(p.DispensedDate),
                               Provider = p.Provider,
                               Route = p.Route,
                               Strength = p.Strength,
                               DosValue = p.DosValue,
                               DosUnit = p.DosUnit,
                               Frequency = p.Frequency,
                               LabelInstructions = p.LabelInstructions,
                               Notes = p.Notes,
                               CreatedDate = p.CreatedDate,
                               SourceId = p.SourceId
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(p => p.MedicineName.Contains(searchString));
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

        public int DeleteMedicine(Guid oGuid)
        {
            int res = 0;
            try
            {
                var rec = _db.Medication.FirstOrDefault(m => m.Id.Equals(oGuid));

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

        public string GetMedicineNameById(int Id)
        {
            try
            {
                var res = _db.MedicationMaster.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.MedicineName;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetRouteById(int Id)
        {
            try
            {
                var res = _db.MedicineRoute.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Route;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetDosageValueById(int Id)
        {
            try
            {
                var res = _db.DosageValue.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.DosValue;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetDosageUnitById(int Id)
        {
            try
            {
                var res = _db.DosageUnit.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.DosUnit;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public string GetFrequencyById(int Id)
        {
            try
            {
                var res = _db.FrequencyTaken.Where(m => m.Id == Id).FirstOrDefault();
                if (res != null)
                {
                    return res.Frequency;
                }
            }
            catch (Exception)
            {
            }

            return "";
        }

        public bool AddMedicine(MedicationViewModel oMedicines, out Guid actId)
        {
            bool _result = false;
            try
            {
                if (oMedicines != null && oMedicines.MedicineType != 0)
                {
                    oMedicines.Id = Guid.NewGuid();
                    Mapper.CreateMap<MedicationViewModel, Medication>();
                    Medication objMedicines = Mapper.Map<Medication>(oMedicines);
                    objMedicines.CreatedDate = DateTime.Now;
                    objMedicines.ModifiedDate = DateTime.Now;
                    _db.Medication.Add(objMedicines);
                    int res = _db.SaveChanges();

                    objMedicines = null;
                    _result = res > 0;
                }
            }
            catch (Exception)
            {
            }
            actId = oMedicines.Id;
            oMedicines = null;
            return _result;
        }

        public bool UpdateMedicine(MedicationViewModel oMedicines)
        {
            bool result = false;
            try
            {
                var record = _db.Medication.FirstOrDefault(m => m.Id.Equals(oMedicines.Id));
                if (record != null)
                {
                    record.TakingMedicine = oMedicines.TakingMedicine;
                    record.PrescribedDate = oMedicines.PrescribedDate;
                    record.DispensedDate = oMedicines.DispensedDate;
                    record.Provider = oMedicines.Provider;
                    record.Route = oMedicines.Route;
                    record.ModifiedDate = DateTime.Now;
                    record.Strength = oMedicines.Strength;
                    record.DosValue = oMedicines.DosValue;
                    record.DosUnit = oMedicines.DosUnit;
                    record.Frequency = oMedicines.Frequency;
                    record.LabelInstructions = oMedicines.LabelInstructions;
                    record.Notes = oMedicines.Notes;
                    result = _db.SaveChanges() > 0;
                    record = null;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public MedicationViewModel GetMedicineById(Guid Id)
        {
            MedicationViewModel oMedicationViewModel = null;
            try
            {
                Medication oMedicines = _db.Medication.FirstOrDefault(m => m.Id.Equals(Id));
                if (oMedicines != null)
                {
                    Mapper.CreateMap<Medication, MedicationViewModel>();
                    oMedicationViewModel = Mapper.Map<MedicationViewModel>(oMedicines);
                    oMedicines = null;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return oMedicationViewModel;
        }

        public List<MedicationMasterViewModel> GetMedicationMaster(string str, int skip)
        {
            try
            {
                List<MedicationMaster> list = (from k in _db.MedicationMaster.Where(k=>
                                                k.MedicineName.StartsWith(str)).OrderBy(s=>s.MedicineName).Skip(skip).Take(100)
                                            //   orderby k.MedicineName ascending
                                               select new MedicationMaster
                                               {
                                                   Id = k.Id,
                                                   MedicineName = k.MedicineName
                                               }).ToList();
                //var list = _db.MedicationMaster.Where(m => m.MedicineName.ToLower().StartsWith(str.ToLower())).ToList();
                Mapper.CreateMap<MedicationMaster, MedicationMasterViewModel>();
                List<MedicationMasterViewModel> lstMedicationMasterViewModel = Mapper.Map<List<MedicationMaster>, List<MedicationMasterViewModel>>(list);
                return lstMedicationMasterViewModel;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<MedicineRouteViewModel> GetRoutes()
        {
            try
            {
                var list = _db.MedicineRoute.ToList();
                Mapper.CreateMap<MedicineRoute, MedicineRouteViewModel>();
                List<MedicineRouteViewModel> lstMedicineRouteViewModel = Mapper.Map<List<MedicineRoute>, List<MedicineRouteViewModel>>(list);
                list = null;
                return lstMedicineRouteViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DosageValueViewModel> GetDosagevalues()
        {
            try
            {
                var list = _db.DosageValue.ToList();
                Mapper.CreateMap<DosageValue, DosageValueViewModel>();
                List<DosageValueViewModel> lstDosageValueViewModel = Mapper.Map<List<DosageValue>, List<DosageValueViewModel>>(list);
                list = null;
                return lstDosageValueViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DosageUnitViewModel> GetDosageunits()
        {
            try
            {
                var list = _db.DosageUnit.ToList();
                Mapper.CreateMap<DosageUnit, DosageUnitViewModel>();
                List<DosageUnitViewModel> lstDosageUnitViewModel = Mapper.Map<List<DosageUnit>, List<DosageUnitViewModel>>(list);
                list = null;
                return lstDosageUnitViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<FrequencyTakenViewModel> GetFrequencies()
        {
            try
            {
                var list = _db.FrequencyTaken.ToList();
                Mapper.CreateMap<FrequencyTaken, FrequencyTakenViewModel>();
                List<FrequencyTakenViewModel> lstFrequencyTakenViewModel = Mapper.Map<List<FrequencyTaken>, List<FrequencyTakenViewModel>>(list);
                list = null;
                return lstFrequencyTakenViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MedicationViewModel> GetMedicationCompleteList(Guid Id)
        {
            try
            {
                var list = _db.Medication.Where(m => m.UserId.Equals(Id) && !m.DeleteFlag && m.SourceId != 2).ToList();
                Mapper.CreateMap<Medication, MedicationViewModel>();
                List<MedicationViewModel> lstMedicationViewModel = Mapper.Map<List<Medication>, List<MedicationViewModel>>(list);
                list = null;
                return lstMedicationViewModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
