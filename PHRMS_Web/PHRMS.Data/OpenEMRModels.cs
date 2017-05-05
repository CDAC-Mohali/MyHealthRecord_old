using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.Data
{

    [Table("StatesOpenEMR")]
    public class StatesOpenEMR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }

    }


    [Table("Districts")]
    public class Districts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int StateId { get; set; }

    }

    [Table("SubDistricts")]
    public class SubDistricts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubDistrictsId { get; set; }
        public string SubDistrictsName { get; set; }
        public int DistrictId { get; set; }

    }

    [Table("Hospital_OpenEMR")]
    public class Hospital_OpenEMR
    {
        [Key]
        public long Hospital_Id { get; set; }
        public Guid UserId { get; set; }
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

    [Table("OpenEMRT")]
    public class OpenEMRT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
    }


}
