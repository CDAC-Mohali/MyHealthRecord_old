using System.Collections.Generic;
using System.Linq;
using System;
using PHRMS.ViewModels;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<AllergyMasterViewModel> GetAllergyMaster(string str)
        {
            try
            {
                return _repository.GetAllergyMaster(str);
            }
            catch (Exception)
            {
            }
            return null;
        }

        public List<AllergySeverityViewModel> GetAllergySeverities()
        {
            try
            {
                return _repository.GetAllergySeverities();
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool AddAllergies(AllergyViewModel oAllergies)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = new Guid("00000000-0000-0000-0000-000000000000");
            
            try
            {
                if (oAllergies != null)
                {
                    //if (oAllergies.strStartDate != "")
                    //    oAllergies.StartDate = DateTime.ParseExact(oAllergies.strStartDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //if (oAllergies.strEndDate != "")
                    //    oAllergies.EndDate = DateTime.ParseExact(oAllergies.strEndDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    if (oAllergies.Id.Equals(Guid.Empty))
                    {
                        
                        result = _repository.AddAllergies(oAllergies,out ActId);
                        if (result) { flag = 1; }
                    }
                    else
                    {
                        ActId = oAllergies.Id;
                        result = _repository.UpdateAllergy(oAllergies);
                        if (result) { flag = 2; }
                    }
                }

            }
            catch (Exception)
            {
            }
            if(result == true && flag == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 1;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oAllergies.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 1;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oAllergies.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }
           
            return result;
        }


        public List<AllergyViewModel> GetAllergiesGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetAllergiesGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteAllergy(Guid oGuid, Guid userId)
        {
            var res = _repository.DeleteAllergy(oGuid);
           
            if (res == 1){
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 1;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;
        }

        public AllergyViewModel GetAllergyById(Guid Id)
        {
            AllergyViewModel oAllergyViewModel = null;
            try
            {
                oAllergyViewModel = _repository.GetAllergyById(Id);

                if (oAllergyViewModel != null)
                {
                    oAllergyViewModel.strStill_Have = oAllergyViewModel.Still_Have ? "Yes" : "No";
                    //oAllergyViewModel.strStartDate = (oAllergyViewModel.StartDate != null && oAllergyViewModel.StartDate != DateTime.MinValue) ? oAllergyViewModel.StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    //oAllergyViewModel.strEndDate = (oAllergyViewModel.EndDate != null && oAllergyViewModel.EndDate != DateTime.MinValue) ? oAllergyViewModel.EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oAllergyViewModel.AllergyName = _repository.GetAllergyNameById(oAllergyViewModel.AllergyType);
                    oAllergyViewModel.strSeverity = _repository.GetSeverityNameById(oAllergyViewModel.Severity);
                    oAllergyViewModel.strDuration = _repository.GetAllergyDurationById(oAllergyViewModel.DurationId);
                }
            }
            catch (Exception)
            { }
            return oAllergyViewModel;
        }

        public List<AllergyViewModel> GetAllergiesExportableList(Guid Id)
        {
            List<AllergyViewModel> lstAllergyViewModel = null;
            try
            {
                lstAllergyViewModel = _repository.GetAllergiesCompleteList(Id);
                if (lstAllergyViewModel != null)
                {
                    foreach (var item in lstAllergyViewModel)
                    {
                        item.strStill_Have = item.Still_Have ? "Yes" : "No";
                        item.strDuration = _repository.GetAllergyDurationById(item.DurationId);
                        //item.strStartDate = (item.StartDate != null && item.StartDate != DateTime.MinValue) ? item.StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        //item.strEndDate = (item.EndDate != null && item.EndDate != DateTime.MinValue) ? item.EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                        item.strSeverity = _repository.GetSeverityById(item.Severity);
                        item.AllergyName = _repository.GetAllergyNameById(item.AllergyType);
                        item.strDuration = _repository.GetAllergyDurationById(item.DurationId);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstAllergyViewModel;
        }

        public List<System.Data.DataRow> GetAllergiesTable(Guid Id)
        {
            List<AllergyViewModel> lstAllergyViewModel = null;
            var allergies = new System.Data.DataTable();
            try
            {
                lstAllergyViewModel = _repository.GetAllergiesCompleteList(Id);
                if (lstAllergyViewModel != null)
                {
                    allergies.Columns.Add(new System.Data.DataColumn("Sno", Type.GetType("System.Int32")));
                    allergies.Columns.Add(new System.Data.DataColumn("Allergy Name", Type.GetType("System.String")));
                    allergies.Columns.Add(new System.Data.DataColumn("From", Type.GetType("System.String")));
                    //allergies.Columns.Add(new System.Data.DataColumn("Start Date", Type.GetType("System.String")));
                    //allergies.Columns.Add(new System.Data.DataColumn("End Date", Type.GetType("System.String")));
                    allergies.Columns.Add(new System.Data.DataColumn("Still Have?", Type.GetType("System.String")));
                    Object[] data = null;
                    for (int i = 0; i < lstAllergyViewModel.Count; i++)
                    {
                        data = new Object[4];
                        data[0] = i + 1;
                        data[1] = _repository.GetAllergyNameById(lstAllergyViewModel[i].AllergyType);
                        data[2] = _repository.GetAllergyDurationById(lstAllergyViewModel[i].DurationId);
                        //data[2] = (lstAllergyViewModel[i].StartDate != null && lstAllergyViewModel[i].StartDate != DateTime.MinValue) ? lstAllergyViewModel[i].StartDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        //data[3] = (lstAllergyViewModel[i].EndDate != null && lstAllergyViewModel[i].EndDate != DateTime.MinValue) ? lstAllergyViewModel[i].EndDate.ToString("dd/MM/yyyy").Replace('-', '/') : "NA";
                        data[3] = lstAllergyViewModel[i].Still_Have ? "Yes" : "No";
                        allergies.Rows.Add(data);
                        data = null;
                    }
                }
            }
            catch (Exception)
            {

            }
            return allergies.Select().ToList();
        }

        public List<AllergyDurationViewModel> GetAllergyDurationList()
        {
            return _repository.GetAllergyDurationList();
        }
    }
}