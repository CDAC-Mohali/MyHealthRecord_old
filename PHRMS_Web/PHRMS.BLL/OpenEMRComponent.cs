using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PHRMS.ViewModels;

namespace PHRMS.BLL
{
    public partial class CatalogService
    {
        public List<Hospital_OpenEMRViewModel> GetOpenEMRGridList(Guid userId,string city, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            return _repository.GetOpenEMRGridList(userId,city, page, limit, sortBy, direction, searchString, out total);
        }

        //public void AddRowToTrigger()
        //{
        //     _repository.AddRowToTrigger();
        //}

    }
}
