using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using System.Linq;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<ProcedureMasterViewModel> GetProcedureMaster(string str)
        {
            try
            {
                return _repository.GetProcedureMaster(str);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool SaveProcedure(ProceduresViewModel oProceduresViewModel)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            List<FileViewModel> lstFiles = null;
            try
            {
                if (oProceduresViewModel != null)
                {
                    if (oProceduresViewModel.strStartDate != "")
                        oProceduresViewModel.StartDate = DateTime.ParseExact(oProceduresViewModel.strStartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    if (oProceduresViewModel.strEndDate != "")
                        oProceduresViewModel.EndDate = DateTime.ParseExact(oProceduresViewModel.strEndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (oProceduresViewModel.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddProcedure(oProceduresViewModel, out ActId);
                        if (result)
                        {
                            flag = 1;
                            lstFiles = new List<FileViewModel>();
                            FileViewModel oFile = null;
                            if (lstFiles != null)
                            {
                                foreach (var name in oProceduresViewModel.lstFiles)
                                {
                                    oFile = new FileViewModel();
                                    oFile.FileName = name;
                                    oFile.FileType = FileType.Procedure;
                                    oFile.CreatedDate = DateTime.Now;
                                    oFile.RecId = oProceduresViewModel.Id;
                                    oFile.UserId = oProceduresViewModel.UserId;
                                    lstFiles.Add(oFile);
                                    oFile = null;
                                }
                                _repository.SaveBulkFiles(lstFiles);
                            }
                        }
                    }
                    else
                    {
                        ActId = oProceduresViewModel.Id;
                        result = _repository.UpdateProcedure(oProceduresViewModel);
                        if (result)
                        {
                            flag = 2;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
            if (result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 5;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oProceduresViewModel.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 5;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oProceduresViewModel.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }


        public List<ProceduresViewModel> GetProceduresGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetProceduresGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteProcedure(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteProcedure(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 5;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }

        public ProceduresViewModel GetProcedureById(Guid Id)
        {
            ProceduresViewModel oProceduresViewModel = null;
            try
            {
                oProceduresViewModel = _repository.GetProcedureById(Id);

                if (oProceduresViewModel != null)
                {
                    oProceduresViewModel.strStartDate = (oProceduresViewModel.StartDate != null && oProceduresViewModel.StartDate != DateTime.MinValue) ? oProceduresViewModel.StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oProceduresViewModel.strEndDate = (oProceduresViewModel.EndDate != null && oProceduresViewModel.EndDate != DateTime.MinValue) ? oProceduresViewModel.EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oProceduresViewModel.ProcedureName = _repository.GetProcedureNameById(oProceduresViewModel.ProcedureType);
                    oProceduresViewModel.lstFileModels = _repository.GetAllAttachments(FileType.Procedure, Id);
                }
            }
            catch (Exception)
            { }
            return oProceduresViewModel;
        }

        public List<ProceduresViewModel> GetProceduresExportableList(Guid Id)
        {
            List<ProceduresViewModel> lstProceduresViewModel = null;
            try
            {
                lstProceduresViewModel = _repository.GetProceduresCompleteList(Id);
                if (lstProceduresViewModel != null)
                {
                    foreach (var item in lstProceduresViewModel)
                    {
                        item.strStartDate = (item.StartDate != null && item.StartDate != DateTime.MinValue) ? item.StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strEndDate = (item.EndDate != null && item.EndDate != DateTime.MinValue) ? item.EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.ProcedureName = _repository.GetProcedureNameById(item.ProcedureType);
                        item.lstFileModels = _repository.GetAllAttachments(FileType.Procedure, item.Id);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstProceduresViewModel;
        }

        public List<System.Data.DataRow> GetProceduresTable(Guid Id)
        {
            List<ProceduresViewModel> lstProceduresViewModel = null;
            var procedures = new System.Data.DataTable();
            try
            {
                lstProceduresViewModel = _repository.GetProceduresCompleteList(Id);
                if (lstProceduresViewModel != null)
                {
                    procedures.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    procedures.Columns.Add(new System.Data.DataColumn("Procedure Name", Type.GetType("System.String")));
                    procedures.Columns.Add(new System.Data.DataColumn("Date of Procedure", Type.GetType("System.String")));
                    procedures.Columns.Add(new System.Data.DataColumn("Diagnosed by Doctor/Hospital", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstProceduresViewModel.Count; i++)
                    {
                        data = new Object[4];
                        data[0] = i + 1;
                        data[1] = _repository.GetProcedureNameById(lstProceduresViewModel[i].ProcedureType);
                        //data[2] = (lstProceduresViewModel[i].StartDate != null && lstProceduresViewModel[i].StartDate != DateTime.MinValue) ? lstProceduresViewModel[i].StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[2] = (lstProceduresViewModel[i].EndDate != null && lstProceduresViewModel[i].EndDate != DateTime.MinValue) ? lstProceduresViewModel[i].EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[3] = lstProceduresViewModel[i].SurgeonName;
                        procedures.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            { }
            return procedures.Select().ToList();
        }

        public string GetFirstAttachment(int filetype, Guid Id)
        {
            string res = "";
            try
            {
                var result = _repository.GetAllAttachments((FileType)filetype, Id);
                if (result != null && result.Count > 0)
                {
                    res = result[0].FileName;
                }
            }
            catch (Exception)
            {

            }
            return res;
        }
    }
}
