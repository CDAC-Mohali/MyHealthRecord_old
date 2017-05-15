using System;
using System.Collections.Generic;
using PHRMS.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public bool AddContact(MedicalContactRecordsViewModel oContact)
        {
            bool result = false;
            try
            {
                if (oContact != null)
                {
                   
                    if (oContact.Id.Equals(Guid.Empty))
                    {
                        result = _repository.AddContact(oContact);
                    }
                    else
                        result = _repository.UpdateContact(oContact);
                }

            }
            catch (Exception)
            {
            }

            return result;
        }
        public List<MedicalContactRecordsViewModel> GetContactGridList(Guid userId, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetContactGridList(userId, page, limit, sortBy, direction, searchString, out total);
        }

        public int DeleteContact(Guid oGuid)
        {
            return _repository.DeleteContact(oGuid);
        }
        public MedicalContactRecordsViewModel GetContactById(Guid Id)
        {
            MedicalContactRecordsViewModel oMedicalContactRecordsViewModel = null;
            try
            {
               
                oMedicalContactRecordsViewModel = _repository.GetContactById(Id);
                oMedicalContactRecordsViewModel.strMedContType = _repository.GetContactTypeById(oMedicalContactRecordsViewModel.MedContType);

            }
            catch (Exception)
            { }
            return oMedicalContactRecordsViewModel;

        }

    }
}
