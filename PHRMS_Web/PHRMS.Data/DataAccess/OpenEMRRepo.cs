using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PHRMS.ViewModels;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {


        public List<Hospital_OpenEMRViewModel> GetOpenEMRGridList(Guid userId,string city, int? page, int? limit, string sortBy, string direction, string searchString, out int total)
        {
            int _serialNo = 1;
            var records = (from p in _db.Hospital_OpenEMR
                           where p.Hospital_city.Equals(city)                         
                           select new Hospital_OpenEMRViewModel
                           {
                               Hospital_name = p.Hospital_name,
                               Hospital_address = p.Hospital_address,
                               Hospital_admin = p.Hospital_admin,
                               Hospital_contact  = p.Hospital_contact,
                               Admin_contact=p.Admin_contact,
                               Reg_Date=p.Reg_Date,
                               openemr_IP=p.openemr_IP,
                               Last_Activity=p.Last_Activity,
                               Interoperability_Status=p.Interoperability_Status
                           }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                records = records.Where(k => k.Hospital_name.Contains(searchString));
            }

            total = records.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(direction))
            {
                if (direction.Trim().ToLower() == "asc")
                {
                    records = SortHelper.OrderBy(records, sortBy);
                }
                else
                {
                    records = SortHelper.OrderByDescending(records, sortBy);
                }
            }
            else
            {
                records = SortHelper.OrderBy(records, "Reg_Date");
            }

            //var recs = records.AsEnumerable();

            if (page.HasValue && limit.HasValue)
            {
                int start = (page.Value - 1) * limit.Value;
                records = records.Skip(start).Take(limit.Value);
            }

            var rs = records.ToList();

            foreach (var item in rs)
            {
                item.sno = _serialNo++;
            }

            return rs;
        }
        //public void AddRowToTrigger()
        //{
        //    try
        //    {
               
        //        OpenEMRT oOpenEMRT = new OpenEMRT();
        //        oOpenEMRT.TimeStamp = DateTime.Now;
        //        _db.OpenEMRT.Add(oOpenEMRT);
        //        _db.SaveChanges();
        //        //return res;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
