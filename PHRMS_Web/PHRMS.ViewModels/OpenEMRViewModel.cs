using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.ViewModels
{


    public class OpenEMRViewModel
    {
        public StatesOpenEMRViewModel oStatesOpenEMRViewModel { get; set; }
        public DistrictsViewModel oDistrictsViewModel { get; set; }
        public SubDistrictsViewModel oSubDistrictsViewModel { get; set; }
        public Hospital_OpenEMRViewModel oHospital_OpenEMRViewModel { get; set; }


        public OpenEMRViewModel()
        {
            oStatesOpenEMRViewModel = new StatesOpenEMRViewModel();
            oDistrictsViewModel = new DistrictsViewModel();
            oSubDistrictsViewModel = new SubDistrictsViewModel();
            oHospital_OpenEMRViewModel = new Hospital_OpenEMRViewModel();
        }


    }
    public class StatesOpenEMRViewModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

    }

    public class DistrictsViewModel
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }

    }
    public class SubDistrictsViewModel
    {
        public int SubDistrictsId { get; set; }
        public string SubDistrictsName { get; set; }
        public int DistrictId { get; set; }

    }
   
    public class Hospital_OpenEMRViewModel
    {
        public int sno { get; set; }
        public Guid UserId { get; set; }
        public long Hospital_Id { get; set; }
        public string Hospital_name { get; set; }
        public string Hospital_state { get; set; }
        public string Hospital_city { get; set; }
        public string Hospital_address { get; set; }
        public string Hospital_admin { get; set; }
        public string Hospital_contact { get; set; }
        public string Admin_contact { get; set; }
        public DateTime Reg_Date { get; set; }
        public string openemr_IP { get; set; }
        public DateTime Last_Activity { get; set; }
        public int block { get; set; }
        public int Interoperability_Status { get; set; }
        public string C1 { get; set; }
        public int C2 { get; set; }
    }

}
