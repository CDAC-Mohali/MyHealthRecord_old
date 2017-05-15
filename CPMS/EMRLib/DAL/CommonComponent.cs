using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMRLib.DAL;
using AutoMapper;

using EMRLib.DataModels;
using EMRViewModels;
using System.Web.Mvc;

namespace EMRLib.DAL
{
    public class CommonComponent
    {

        static List<SelectListItem> StatesList;

        public IEnumerable<SelectListItem> GetStatesList()
        {
            EMRDBContext db = new EMRDBContext();
            if (StatesList == null)
            {

                StatesList = db.States.OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
                StatesList.Insert(0, new SelectListItem { Text = "---Select State---", Value = "0" });
            }

            return StatesList;
        }
        List<SelectListItem> GenderList = new List<SelectListItem>();
        public IEnumerable<SelectListItem> GetGenderList()
        {


            GenderList.Add(new SelectListItem { Text = "Male", Value = "M" });
            GenderList.Add(new SelectListItem { Text = "Female", Value = "F" });
            GenderList.Add(new SelectListItem { Text = "Not Specified", Value = "U" });

            return GenderList;
        }


        ////////////////////////
        /// <summary>
        /// /////////////////Speciality lsit
        /// </summary>
        /// 

        List<SelectListItem> SpecialityList = new List<SelectListItem>();
        public IEnumerable<SelectListItem> GetSpecialityList()
        {
            EMRDBContext db = new EMRDBContext();
            SpecialityList = db.ContactTypes.Where(s => s.Id != 1051).OrderBy(s => s.MedContType).Select(x => new SelectListItem { Text = x.MedContType, Value = x.Id.ToString() }).ToList();
            SpecialityList.Insert(0, new SelectListItem { Text = "---Select Speciality---", Value = "" });
            SpecialityList.Add(new SelectListItem { Text = "Other", Value = "1051" });
            return SpecialityList;
        }
        List<SelectListItem> HospitalList = new List<SelectListItem>();
        public IEnumerable<SelectListItem> GetHospitalList()
        {
            EMRDBContext db = new EMRDBContext();
            HospitalList = db.MedicalColleges.Select(x => new SelectListItem { Text = x.MedicalCollegeName, Value = x.MedicalCollegeId.ToString() }).ToList();
            HospitalList.Insert(0, new SelectListItem { Text = "---Select Hospital---", Value = "" });
            return SpecialityList;
        }

    }
}
