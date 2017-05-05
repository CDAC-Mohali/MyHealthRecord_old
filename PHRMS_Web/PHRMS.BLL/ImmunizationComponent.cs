using System.Threading.Tasks;
using PHRMS.Data;
using System.Collections.Generic;
using System.Linq;
using PHRMS.Data.DataAccess;
using System;
using PHRMS.ViewModels;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<ImmunizationsMastersViewModel> GetImmunizationMaster(string strImmmn)
        {
            try
            {
                return _repository.GetImmunizationMaster(strImmmn);
            }
            catch (Exception)
            {
            }
            return null;
        }
       

        public bool AddImmunization(ImmunizationViewModel oImmunization)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            try
            {
                if (oImmunization != null)
                {
                   if (oImmunization.strImmunizationDate != "")
                        oImmunization.ImmunizationDate = DateTime.ParseExact(oImmunization.strImmunizationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (oImmunization.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddImmunization(oImmunization, out ActId);
                        if (result)
                        {
                            flag = 1;
                        }
                    }
                    else
                    {
                        ActId = oImmunization.Id;
                        result = _repository.UpdateImmunization(oImmunization);
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
                oUserActivityViewModels.Module = 2;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oImmunization.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 2;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oImmunization.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
            return result;
        }

        public List<ImmunizationViewModel> GetImmunizationGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetImmunizationGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteImmunization(Guid oGuid, Guid userId)
        {
            var res =  _repository.DeleteImmunization(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 2;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }

        public ImmunizationViewModel GetImmunizationById(Guid Id)
        {
            ImmunizationViewModel oImmunizationViewModel = null;
            try
            {
                oImmunizationViewModel = _repository.GetImmunizationById(Id);


                if (oImmunizationViewModel.strImmunizationDate != "")
                {
                    oImmunizationViewModel.strImmunizationDate = (oImmunizationViewModel.ImmunizationDate != null && oImmunizationViewModel.ImmunizationDate != DateTime.MinValue) ? oImmunizationViewModel.ImmunizationDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    //oImmunizationViewModel.ImmunizationDate = DateTime.ParseExact(oImmunizationViewModel.strImmunizationDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    oImmunizationViewModel.ImmunizationName = _repository.GetImmunizationNameById(oImmunizationViewModel.ImmunizationsTypeId);
                }

            }
            catch (Exception)
            { }
            return oImmunizationViewModel;

        }

        public List<ImmunizationViewModel> GetlstImmunizationExportableList(Guid Id)
        {
            List<ImmunizationViewModel> lstImmunizationViewModel = null;
            try
            {
                lstImmunizationViewModel = _repository.GetImmunizationCompleteList(Id);
                if (lstImmunizationViewModel != null)
                {
                    foreach (var item in lstImmunizationViewModel)
                    {
                        item.strImmunizationDate = (item.ImmunizationDate != null && item.ImmunizationDate != DateTime.MinValue) ? item.ImmunizationDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.ImmunizationName = _repository.GetImmunizationNameById(item.ImmunizationsTypeId);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstImmunizationViewModel;
        }

        public List<System.Data.DataRow> GetImmunizationsTable(Guid Id)
        {
            List<ImmunizationViewModel> lstImmunizationViewModel = null;
            var immunizations = new System.Data.DataTable();
            try
            {
                lstImmunizationViewModel = _repository.GetImmunizationCompleteList(Id);
                if (lstImmunizationViewModel != null)
                {
                    immunizations.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    immunizations.Columns.Add(new System.Data.DataColumn("Immunization Name", Type.GetType("System.String")));
                    immunizations.Columns.Add(new System.Data.DataColumn("Taken On", Type.GetType("System.String")));
                    immunizations.Columns.Add(new System.Data.DataColumn("Comments", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstImmunizationViewModel.Count; i++)
                    {
                        data = new Object[4];
                        data[0] = i + 1;
                        data[1] = _repository.GetImmunizationNameById(lstImmunizationViewModel[i].ImmunizationsTypeId);
                        data[2] = (lstImmunizationViewModel[i].ImmunizationDate != null && lstImmunizationViewModel[i].ImmunizationDate != DateTime.MinValue) ? lstImmunizationViewModel[i].ImmunizationDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[3] = lstImmunizationViewModel[i].Comments;
                        immunizations.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return immunizations.Select().ToList();
        }

    }
}
