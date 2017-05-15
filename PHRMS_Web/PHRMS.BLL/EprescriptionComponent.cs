using System.Collections.Generic;
using System;
using PHRMS.ViewModels;
using PHRMS.Data;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
  
        public bool AddPrescription(EprescriptionViewModel oPrescription)
        {
            bool result = false;
            var flag = 0;
            Guid ActId = Guid.Empty;
            List<FileViewModel> lstFiles = null;
            try
            {
                if (oPrescription != null)
                {
                    if (!string.IsNullOrEmpty(oPrescription.strPresDate))
                    {
                        oPrescription.PresDate = DateTime.ParseExact(oPrescription.strPresDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    if (oPrescription.Id.Equals(Guid.Empty))
                    {
                        oPrescription.Id = Guid.NewGuid();
                        result = _repository.AddPrescription(oPrescription, out ActId);
                        
                        
                        if (result)
                        {
                            flag = 1;
                            lstFiles = new List<FileViewModel>();
                            FileViewModel oFile = null;
                            if (lstFiles != null)
                            {
                                foreach (var name in oPrescription.lstFiles)
                                {
                                    oFile = new FileViewModel();
                                    oFile.FileName = name;
                                    oFile.FileType = PHRMS.ViewModels.FileType.ePrescription;
                                    oFile.CreatedDate = DateTime.Now;
                                    oFile.RecId = oPrescription.Id;
                                    oFile.UserId = oPrescription.UserId;
                                    lstFiles.Add(oFile);
                                    oFile = null;
                                }
                                _repository.SaveBulkFiles(lstFiles);
                            }
                        }
                    }
                    else
                    {
                        ActId = oPrescription.Id;
                        result = _repository.UpdatePrescription(oPrescription);
                        if (result) { flag = 2; }
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
                oUserActivityViewModels.Module = 13;
                oUserActivityViewModels.Operation = 1;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oPrescription.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else if (result == true && flag == 2)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = ActId;
                oUserActivityViewModels.Module = 13;
                oUserActivityViewModels.Operation = 2;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = oPrescription.UserId;
                result = _repository.AddUserActivity(oUserActivityViewModels);
            }
            else
            {
                flag = 0;
            }

            return result;
        }

        public List<EprescriptionViewModel> GetPrescriptionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetPrescriptionGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }
        public List<EprescriptionViewModel> GetSharePrescriptionGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetSharePrescriptionGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }
        public SuperViewModel ViewDetal(Guid PrescriptionId)
        {
            return _repository.ViewDetal(PrescriptionId);
        }
       
        public List<EprescriptionViewModel> GetPrescriptionExportableList(Guid Id)
        {
            List<EprescriptionViewModel> lstPrescriptionViewModel = null;
            try
            {
                lstPrescriptionViewModel = _repository.GetEprescriptionCompleteList(Id);
                if (lstPrescriptionViewModel != null)
                {
                    foreach (var item in lstPrescriptionViewModel)
                    {
                        item.strPresDate = (item.PresDate != null && item.PresDate != DateTime.MinValue) ? item.PresDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return lstPrescriptionViewModel;
        }

        public int  DeletePrescription(Guid oGuid,Guid userId)
        {
            var res =  _repository.DeletePrescription(oGuid);
            if (res == 1)
            {
                UserActivityViewModels oUserActivityViewModels = new UserActivityViewModels();
                oUserActivityViewModels.ActivityId = oGuid;
                oUserActivityViewModels.Module = 13;
                oUserActivityViewModels.Operation = 3;
                oUserActivityViewModels.TimeStamp = DateTime.Now;
                oUserActivityViewModels.Id = Guid.NewGuid();
                oUserActivityViewModels.UserId = userId;
                _repository.AddUserActivity(oUserActivityViewModels);
            }
            return res;

        }

        public EprescriptionViewModel GetPrescriptionById(Guid Id)
        {
            EprescriptionViewModel oEprescriptionViewModel = null;
            try
            {
                oEprescriptionViewModel = _repository.GetPrescriptionById(Id);

                if (oEprescriptionViewModel != null)
                {
                    oEprescriptionViewModel.strPresDate = (oEprescriptionViewModel.PresDate != null && oEprescriptionViewModel.PresDate != DateTime.MinValue) ? oEprescriptionViewModel.PresDate.ToString("dd/MM/yyyy").Replace('-', '/') : "";
                    oEprescriptionViewModel.lstFileModels = _repository.GetAllAttachments(PHRMS.ViewModels.FileType.ePrescription, Id);
                }
            }
            catch (Exception)
            { }
            return oEprescriptionViewModel;

        }
        public PersonalInformation GetPersonById(PersonalInformation oGuid)
        {
            var obj =  _repository.GetPersonById(oGuid);
            return obj;
        }

    }
}
