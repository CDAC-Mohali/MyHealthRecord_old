using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using System.Linq;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<HealthConditionMasterViewModel> GetHealthConditionMaster(string str)
        {
            try
            {
                return _repository.GetHealthConditionMaster(str);
            }
            catch (Exception)
            {
            }
            return null;
        }       

        public bool AddHealthCondition(HealthConditionViewModel oConditions)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");

            try
            {
                if (oConditions != null)
                {
                    oConditions.ServiceDate = DateTime.Now;
                    if (oConditions.strDiagnosisDate != "")
                        oConditions.DiagnosisDate = DateTime.ParseExact(oConditions.strDiagnosisDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //if (oConditions.strServiceDate != "")
                    //    oConditions.ServiceDate = DateTime.ParseExact(oConditions.strServiceDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (oConditions.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddHealthCondition(oConditions, out ActId);
                        if (result)
                            flag = 1;
                    }
                    else
                    {
                        ActId = oConditions.Id;
                        result = _repository.UpdateHealthCondition(oConditions);
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
                oUserActivityViewModels.Module = 6;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oConditions.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 6;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oConditions.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }

            return result;
        }

        public List<HealthConditionViewModel> GetHealthConditionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetHealthConditionGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteHealthCondition(Guid oGuid, Guid userId)
        {
            var res =  _repository.DeleteHealthCondition(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 6;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }

        public HealthConditionViewModel GetHealthConditionById(Guid Id)
        {
            HealthConditionViewModel oHealthConditionViewModel = null;
            try
            {
                oHealthConditionViewModel = _repository.GetHealthConditionById(Id);

                if (oHealthConditionViewModel != null)
                {
                    oHealthConditionViewModel.strStillHaveCondition = oHealthConditionViewModel.StillHaveCondition ? "Yes" : "No";
                    oHealthConditionViewModel.strDiagnosisDate = (oHealthConditionViewModel.DiagnosisDate != null && oHealthConditionViewModel.DiagnosisDate != DateTime.MinValue) ? oHealthConditionViewModel.DiagnosisDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oHealthConditionViewModel.strServiceDate = (oHealthConditionViewModel.ServiceDate != null && oHealthConditionViewModel.ServiceDate != DateTime.MinValue) ? oHealthConditionViewModel.ServiceDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oHealthConditionViewModel.HealthCondition = _repository.GetHealthConditionNameById(oHealthConditionViewModel.ConditionType);
                    
                }
            }
            catch (Exception)
            { }
            return oHealthConditionViewModel;
        }

        public List<HealthConditionViewModel> GetHealthConditionExportableList(Guid Id)
        {
            List<HealthConditionViewModel> lstHealthConditionViewModel = null;
            try
            {
                lstHealthConditionViewModel = _repository.GetHealthConditionCompleteList(Id);
                if (lstHealthConditionViewModel != null)
                {
                    foreach (var item in lstHealthConditionViewModel)
                    {
                        item.strStillHaveCondition = item.StillHaveCondition ? "Yes" : "No";
                        item.strDiagnosisDate = (item.DiagnosisDate != null && item.DiagnosisDate != DateTime.MinValue) ? item.DiagnosisDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strServiceDate = (item.ServiceDate != null && item.ServiceDate != DateTime.MinValue) ? item.ServiceDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.HealthCondition = _repository.GetHealthConditionNameById(item.ConditionType);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstHealthConditionViewModel;
        }

        public List<System.Data.DataRow> GetHealthConditionsTable(Guid Id)
        {
            List<HealthConditionViewModel> lstHealthConditionViewModel = null;
            var healthConditions = new System.Data.DataTable();
            try
            {
                lstHealthConditionViewModel = _repository.GetHealthConditionCompleteList(Id);
                if (lstHealthConditionViewModel != null)
                {
                    healthConditions.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    healthConditions.Columns.Add(new System.Data.DataColumn("Condition Name", Type.GetType("System.String")));
                    healthConditions.Columns.Add(new System.Data.DataColumn("Diag. Date", Type.GetType("System.String")));
                    healthConditions.Columns.Add(new System.Data.DataColumn("Service Date", Type.GetType("System.String")));
                    healthConditions.Columns.Add(new System.Data.DataColumn("Still Have Condition?", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstHealthConditionViewModel.Count; i++)
                    {
                        data = new Object[5];
                        data[0] = i + 1;
                        data[1] = _repository.GetHealthConditionNameById(lstHealthConditionViewModel[i].ConditionType);
                        data[2] = (lstHealthConditionViewModel[i].DiagnosisDate != null && lstHealthConditionViewModel[i].DiagnosisDate != DateTime.MinValue) ? lstHealthConditionViewModel[i].DiagnosisDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[3] = (lstHealthConditionViewModel[i].ServiceDate != null && lstHealthConditionViewModel[i].ServiceDate != DateTime.MinValue) ? lstHealthConditionViewModel[i].ServiceDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[4] = lstHealthConditionViewModel[i].StillHaveCondition ? "Yes" : "No";
                        healthConditions.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return healthConditions.Select().ToList();
        }
    }
}
