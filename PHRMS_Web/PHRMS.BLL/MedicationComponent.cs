using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using System.Linq;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<MedicationMasterViewModel> GetMedicationMaster(string str, int skip)
        {
            try
            {
                return _repository.GetMedicationMaster(str, skip);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<MedicineRouteViewModel> GetRoutes()
        {
            try
            {
                return _repository.GetRoutes();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<DosageUnitViewModel> GetDosageUnits()
        {
            try
            {
                return _repository.GetDosageunits();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<DosageValueViewModel> GetDosageValues()
        {
            try
            {
                return _repository.GetDosagevalues();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<FrequencyTakenViewModel> GetFrequencies()
        {
            try
            {
                return _repository.GetFrequencies();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool AddMedicine(MedicationViewModel oMedicines)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            List<FileViewModel> lstFiles = null;
            try
            {
                if (oMedicines != null)
                {
                    if (oMedicines.strPrescribedDate != "")
                        oMedicines.PrescribedDate = DateTime.ParseExact(oMedicines.strPrescribedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oMedicines.strDispensedDate != "")
                        oMedicines.DispensedDate = DateTime.ParseExact(oMedicines.strDispensedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (oMedicines.Id.Equals(Guid.Empty))
                    {
                        oMedicines.Id = Guid.NewGuid();
                        result = _repository.AddMedicine(oMedicines, out ActId);
                        if (result)
                        {
                            flag = 1;
                            lstFiles = new List<FileViewModel>();
                            FileViewModel oFile = null;
                            if (lstFiles != null)
                            {
                                foreach (var name in oMedicines.lstFiles)
                                {
                                    oFile = new FileViewModel();
                                    oFile.FileName = name;
                                    oFile.FileType = FileType.Medication;
                                    oFile.CreatedDate = DateTime.Now;
                                    oFile.RecId = oMedicines.Id;
                                    oFile.UserId = oMedicines.UserId;
                                    lstFiles.Add(oFile);
                                    oFile = null;
                                }
                                _repository.SaveBulkFiles(lstFiles);
                            }
                        }
                    }
                    else
                    {
                        ActId = oMedicines.Id;
                        result = _repository.UpdateMedicine(oMedicines);
                        if (result)
                        {
                            flag = 2;
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 4;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oMedicines.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 4;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oMedicines.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }

        public List<MedicationViewModel> GetMedicationGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetMedicationGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteMedicine(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteMedicine(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 4;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }

        public MedicationViewModel GetMedicineById(Guid Id)
        {
            MedicationViewModel oMedicationViewModel = null;
            try
            {
                oMedicationViewModel = _repository.GetMedicineById(Id);

                if (oMedicationViewModel != null)
                {
                    oMedicationViewModel.strTakingMedicine = oMedicationViewModel.TakingMedicine ? "Yes" : "No";
                    oMedicationViewModel.strPrescribedDate = (oMedicationViewModel.PrescribedDate != null && oMedicationViewModel.PrescribedDate != DateTime.MinValue) ? oMedicationViewModel.PrescribedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oMedicationViewModel.strDispensedDate = (oMedicationViewModel.DispensedDate != null && oMedicationViewModel.DispensedDate != DateTime.MinValue) ? oMedicationViewModel.DispensedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oMedicationViewModel.MedicineName = _repository.GetMedicineNameById(oMedicationViewModel.MedicineType);
                    oMedicationViewModel.strRoute = _repository.GetRouteById(oMedicationViewModel.Route);
                    oMedicationViewModel.strDosValue = _repository.GetDosageValueById(oMedicationViewModel.DosValue);
                    oMedicationViewModel.strDosUnit = _repository.GetDosageUnitById(oMedicationViewModel.DosUnit);
                    oMedicationViewModel.strFrequency = _repository.GetFrequencyById(oMedicationViewModel.Frequency);
                    oMedicationViewModel.lstFileModels = _repository.GetAllAttachments(FileType.Medication, Id);
                }
            }
            catch (Exception)
            { }
            return oMedicationViewModel;

        }

        public List<MedicationViewModel> GetMedicationExportableList(Guid Id)
        {
            List<MedicationViewModel> lstMedicationViewModel = null;
            try
            {
                lstMedicationViewModel = _repository.GetMedicationCompleteList(Id);
                if (lstMedicationViewModel != null)
                {
                    foreach (var item in lstMedicationViewModel)
                    {
                        item.strTakingMedicine = item.TakingMedicine ? "Yes" : "No";
                        item.strPrescribedDate = (item.PrescribedDate != null && item.PrescribedDate != DateTime.MinValue) ? item.PrescribedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strDispensedDate = (item.DispensedDate != null && item.DispensedDate != DateTime.MinValue) ? item.DispensedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.MedicineName = _repository.GetMedicineNameById(item.MedicineType);
                        item.strRoute = _repository.GetRouteById(item.Route);
                        item.strDosValue = _repository.GetDosageValueById(item.DosValue);
                        item.strDosUnit = _repository.GetDosageUnitById(item.DosUnit);
                        item.strFrequency = _repository.GetFrequencyById(item.Frequency);
                        item.lstFileModels = _repository.GetAllAttachments(FileType.Medication, item.Id);
                       
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstMedicationViewModel;
        }

        public List<System.Data.DataRow> GetMedicationsTable(Guid Id)
        {
            List<MedicationViewModel> lstMedicationViewModel = null;
            var medications = new System.Data.DataTable();
            try
            {
                lstMedicationViewModel = _repository.GetMedicationCompleteList(Id);
                if (lstMedicationViewModel != null)
                {
                    medications.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    medications.Columns.Add(new System.Data.DataColumn("Medication Name", Type.GetType("System.String")));
                    medications.Columns.Add(new System.Data.DataColumn("Prescribed Date", Type.GetType("System.String")));
                    medications.Columns.Add(new System.Data.DataColumn("Strength", Type.GetType("System.String")));
                    medications.Columns.Add(new System.Data.DataColumn("Still Taking Medication?", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstMedicationViewModel.Count; i++)
                    {
                        data = new Object[5];
                        data[0] = i + 1;
                        data[1] = _repository.GetMedicineNameById(lstMedicationViewModel[i].MedicineType);
                        data[2] = (lstMedicationViewModel[i].PrescribedDate != null && lstMedicationViewModel[i].PrescribedDate != DateTime.MinValue) ? lstMedicationViewModel[i].PrescribedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[3] = lstMedicationViewModel[i].Strength;
                        data[4] = lstMedicationViewModel[i].TakingMedicine ? "Yes" : "No";
                        medications.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return medications.Select().ToList();
        }
    }
}
