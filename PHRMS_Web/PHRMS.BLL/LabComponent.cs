using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using System.Linq;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<LabTestMasterViewModel> GetLabTestMaster(string str)
        {
            try
            {
                return _repository.GetLabTestMaster(str);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool AddTest(LabTestViewModel oTests)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            List<FileViewModel> lstFiles = null;
            try
            {
                if (oTests != null)
                {
                    if (oTests.strPerformedDate != "")
                        oTests.PerformedDate = DateTime.ParseExact(oTests.strPerformedDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (oTests.Id.Equals(Guid.Empty))
                    {
                        oTests.Id = Guid.NewGuid();
                        result = _repository.AddTest(oTests, out ActId);
                        if (result)
                        {
                            flag = 1;
                            lstFiles = new List<FileViewModel>();
                            FileViewModel oFile = null;
                            if (lstFiles != null)
                            {
                                foreach (var name in oTests.lstFiles)
                                {
                                    oFile = new FileViewModel();
                                    oFile.FileName = name;
                                    oFile.FileType = FileType.LabReport;
                                    oFile.CreatedDate = DateTime.Now;
                                    oFile.RecId = oTests.Id;
                                    oFile.UserId = oTests.UserId;
                                    lstFiles.Add(oFile);
                                    oFile = null;
                                }
                                _repository.SaveBulkFiles(lstFiles);
                            }
                        }
                    }
                    else
                    {
                        ActId = oTests.Id;
                        result = _repository.UpdateResult(oTests);
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
                oUserActivityViewModels.Module = 3;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oTests.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 3;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oTests.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }

        public List<LabTestViewModel> GetLabTestResultGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetLabTestResultGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteResult(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteResult(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 3;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }


        public LabTestViewModel GetResultById(Guid Id)
        {

            LabTestViewModel oLabTestViewModel = null;
            try
            {
                oLabTestViewModel = _repository.GetResultById(Id);
                //oLabTestViewModel = _repository.GetTestById(oLabTestResultViewModel.PresTestId);
                if (oLabTestViewModel != null)
                {
                    oLabTestViewModel.strPerformedDate = (oLabTestViewModel.PerformedDate != null && oLabTestViewModel.PerformedDate != DateTime.MinValue) ? oLabTestViewModel.PerformedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oLabTestViewModel.TestName = _repository.GetTestNameById(oLabTestViewModel.TestId);
                    oLabTestViewModel.lstFileModels = _repository.GetAllAttachments(FileType.LabReport, Id);
                }
            }
            catch (Exception)
            { }
            return oLabTestViewModel;

        }

        public List<LabTestViewModel> GetLabTestExportableList(Guid Id)
        {
            List<LabTestViewModel> lstLabTestViewModel = null;
            try
            {
                lstLabTestViewModel = _repository.GetLabTestCompleteList(Id);
                if (lstLabTestViewModel != null)
                {
                    foreach (var item in lstLabTestViewModel)
                    {
                        item.strPerformedDate = (item.PerformedDate != null && item.PerformedDate != DateTime.MinValue) ? item.PerformedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.TestName = _repository.GetTestNameById(item.TestId);
                        item.lstFileModels = _repository.GetAllAttachments(FileType.LabReport, item.Id);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstLabTestViewModel;
        }

        public List<System.Data.DataRow> GetLabTestTable(Guid Id)
        {
            List<LabTestViewModel> lstLabTestViewModel = null;
            var labtests = new System.Data.DataTable();
            try
            {
                lstLabTestViewModel = _repository.GetLabTestCompleteList(Id);
                if (lstLabTestViewModel != null)
                {
                    labtests.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    labtests.Columns.Add(new System.Data.DataColumn("Test Name", Type.GetType("System.String")));
                    labtests.Columns.Add(new System.Data.DataColumn("Perf. Date", Type.GetType("System.String")));
                    labtests.Columns.Add(new System.Data.DataColumn("Result", Type.GetType("System.String")));
                    labtests.Columns.Add(new System.Data.DataColumn("Unit", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstLabTestViewModel.Count; i++)
                    {
                        data = new Object[5];
                        data[0] = i + 1;
                        data[1] = _repository.GetTestNameById(lstLabTestViewModel[i].TestId);
                        data[2] = (lstLabTestViewModel[i].PerformedDate != null && lstLabTestViewModel[i].PerformedDate != DateTime.MinValue) ? lstLabTestViewModel[i].PerformedDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[3] = lstLabTestViewModel[i].Result;
                        data[4] = lstLabTestViewModel[i].Unit;
                        labtests.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return labtests.Select().ToList();
        }
    }
}
