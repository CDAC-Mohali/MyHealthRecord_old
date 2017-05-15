using AutoMapper;
using PHRMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {
        public List<StatesModel> GetStatesList()
        {
            var states = _db.States.OrderBy(x => x.Name).ToList();
            Mapper.CreateMap<States, StatesModel>();
            List<StatesModel> lstStatesModel = Mapper.Map<List<States>, List<StatesModel>>(states);
            return lstStatesModel;
        }

        public List<StatesOpenEMRViewModel> GetStatesOpenEMRList()
        {
            var states = _db.StatesOpenEMR.OrderBy(x => x.StateName).ToList();
            Mapper.CreateMap<StatesOpenEMR, StatesOpenEMRViewModel>();
            List<StatesOpenEMRViewModel> lstStatesModel = Mapper.Map<List<StatesOpenEMR>, List<StatesOpenEMRViewModel>>(states);
            return lstStatesModel;
        }
        public List<MedicalContactRecordsViewModel> GetDoctorsList(Guid UserId)
        {
            List<MedicalContactRecordsViewModel> ReturnData = null;
            try
            {
                List<MedicalContactRecords> Records = _db.MedicalContactRecords.Where(s => s.DeleteFlag == false && s.UserId == UserId).ToList();

                Mapper.CreateMap<MedicalContactRecords, MedicalContactRecordsViewModel>();
                ReturnData = Mapper.Map<List<MedicalContactRecords>, List<MedicalContactRecordsViewModel>>(Records);
            }
            catch (Exception ex)
            {
            }
            return ReturnData;
        }

        public List<DistrictsViewModel> GetDistrictNameByStateId(int StateId)
        {
            var records = (from p in _db.Districts
                           join k in _db.StatesOpenEMR
                           on p.StateId equals k.StateId
                           where p.StateId.Equals(StateId)
                           select new DistrictsViewModel
                           {
                               DistrictId = p.DistrictId,
                               DistrictName = p.DistrictName

                           }).AsQueryable();

            var rs = records.ToList();

            return rs;
        }

        public List<SubDistrictsViewModel> GetSubDistrictNameByDistrictId(int DistrictId)
        {
            var records = (from p in _db.SubDistricts
                           join k in _db.Districts
                           on p.DistrictId equals k.DistrictId
                           where p.DistrictId.Equals(DistrictId)
                           select new SubDistrictsViewModel
                           {
                               SubDistrictsId = p.SubDistrictsId,
                               SubDistrictsName = p.SubDistrictsName

                           }).AsQueryable();

            var rs = records.ToList();

            return rs;
        }




        public List<ContactTypeModel> GetContactTypesList()
        {
            var contactType = _db.ContactTypes.OrderBy(x => x.MedContType).ToList();
            Mapper.CreateMap<ContactTypes, ContactTypeModel>();
            List<ContactTypeModel> lstContactTypeModel = Mapper.Map<List<ContactTypes>, List<ContactTypeModel>>(contactType);
            return lstContactTypeModel;
        }
        public List<RelationshipModel> GetRelationshipList()
        {
            var rel = _db.Relationship.OrderBy(x => x.Relation).ToList();
            Mapper.CreateMap<Relationship, RelationshipModel>();
            List<RelationshipModel> lstRelationshipModel = Mapper.Map<List<Relationship>, List<RelationshipModel>>(rel);
            return lstRelationshipModel;
        }


        public List<DisabilityTypesModel> GetDisabilityTypesList()
        {
            var list = _db.DisabilityTypes.OrderBy(x => x.Name).ToList();
            Mapper.CreateMap<DisabilityTypes, DisabilityTypesModel>();
            List<DisabilityTypesModel> lstDisabilityTypesModel = Mapper.Map<List<DisabilityTypes>, List<DisabilityTypesModel>>(list);
            return lstDisabilityTypesModel;
        }

        public List<BloodGroupsModel> GetBloodGroupsList()
        {
            var list = _db.BloodGroups.OrderBy(x => x.Name).ToList();
            Mapper.CreateMap<BloodGroups, BloodGroupsModel>();
            List<BloodGroupsModel> lstBloodGroups = Mapper.Map<List<BloodGroups>, List<BloodGroupsModel>>(list);
            return lstBloodGroups;
        }

        public string GetBloodGroupById(int Id)
        {
            string strBloodgp = "";
            try
            {
                if (Id == 0)
                    strBloodgp = "Do Not Specify";
                else
                    strBloodgp = _db.BloodGroups.FirstOrDefault(m => m.Id == Id).Name;
            }
            catch (Exception)
            {
            }
            return strBloodgp;
        }

        public string GetDisabilityTypeById(int Id)
        {
            string strDisabilityTypes = "";
            try
            {
                if (Id == 0)
                    strDisabilityTypes = "Do Not Specify";
                else
                    strDisabilityTypes = _db.DisabilityTypes.FirstOrDefault(m => m.Id == Id).Name;
            }
            catch (Exception)
            {
            }
            return strDisabilityTypes;
        }

        public string GetStateNameById(int Id)
        {
            string strStates = "";
            try
            {
                if (Id == 0)
                    strStates = "Not Available";
                else
                    strStates = _db.States.FirstOrDefault(m => m.Id == Id).Name;
            }
            catch (Exception)
            {
            }
            return strStates;
        }

        public string GetRelationNameById(int Id)
        {
            string strRelation = "";
            try
            {
                if (Id == 0)
                    strRelation = "Not Available";
                else
                    strRelation = _db.Relationship.FirstOrDefault(m => m.Id == Id).Relation;
            }
            catch (Exception)
            {
            }
            return strRelation;
        }



        public List<string> GetPostalCodesFromMaster(string strPostalCode)
        {
            List<string> ReturnData = null;
            try
            {
                ReturnData = (from p in _db.PinCodes
                              where p.Pincode.StartsWith(strPostalCode)
                              select p.Pincode).Distinct().ToList();
            }
            catch (Exception)
            {
            }
            return ReturnData;
        }

        public bool SoftDeleteFileIfExists(FileViewModel oModel)
        {
            bool res = false;
            try
            {
                var recs = _db.FilePath.Where(m => m.FileType == (FileType)oModel.FileType && m.UserId.Equals(oModel.UserId) && !m.DeleteFlag).ToList();
                if (recs != null && recs.Count > 0)
                {
                    foreach (var rec in recs)
                    {
                        rec.DeleteFlag = true;
                    }
                    res = _db.SaveChanges() > 0;
                }
                else
                    res = true;
            }
            catch (Exception)
            {
            }
            return res;
        }

        public bool SaveFileDetails(FileViewModel oModel)
        {
            bool result = false;
            try
            {
                Mapper.CreateMap<FileViewModel, FilePath>();
                FilePath oFilePath = Mapper.Map<FilePath>(oModel);
                _db.Add(oFilePath);
                result = _db.SaveChanges() > 0;
            }
            catch (Exception)
            {
            }
            return result;
        }

        public bool AddContactUs(ContactusViewModel ContactusViewModel)
        {
            bool _result = false;
            try
            {
                if (ContactusViewModel != null )
                {
                 
                    Mapper.CreateMap<ContactusViewModel, ContactUs>();
                    ContactUs objContactUs = new ContactUs();
                    //objContactUs.ContactUsId = ContactusViewModel.ContactUsId;
                    objContactUs.FirstName = ContactusViewModel.FirstName;
                    objContactUs.LastName = ContactusViewModel.LastName;
                    objContactUs.MobileNo = ContactusViewModel.MobileNo;
                    objContactUs.City = ContactusViewModel.City;
                    objContactUs.State = ContactusViewModel.State;
                    objContactUs.Message = ContactusViewModel.Message;
                    objContactUs.Email = ContactusViewModel.Email;
                    objContactUs.Status = 1;
                    _db.Add(objContactUs);
                    _result = _db.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
            }
         
            return _result;
        }


        public bool SaveBulkFiles(List<FileViewModel> lstFiles)
        {
            bool result = false;
            try
            {
                Mapper.CreateMap<FileViewModel, FilePath>();
                List<FilePath> lstFilePaths = Mapper.Map<List<FileViewModel>, List<FilePath>>(lstFiles);
                foreach (var item in lstFilePaths)
                {
                    _db.Add(item);
                }
                result = _db.SaveChanges() > 0;
            }
            catch (Exception)
            {
            }
            return result;
        }

        public List<FileViewModel> GetAllAttachments(PHRMS.ViewModels.FileType type, Guid oGuid)
        {
            List<FileViewModel> lstFileViewModel = null;
            try
            {
                var recs = _db.FilePath.Where(m => m.FileType == (FileType)type && m.RecId.Equals(oGuid)).ToList();
                Mapper.CreateMap<FilePath, FileViewModel>();
                lstFileViewModel = Mapper.Map<List<FilePath>, List<FileViewModel>>(recs);
            }
            catch (Exception)
            {}
            return lstFileViewModel;
        }

        public bool SetProfilePic(string path, Guid userID)
        {
            int res;
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.UserId.Equals(userID));
                user.ImgPath = path;
                res = _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }

            return res > 0;
        }

        public string GetSavedFilePath(Guid Id, PHRMS.ViewModels.FileType type)
        {
            string strPath = "";
            try
            {
                var rec = _db.FilePath.OrderByDescending(m => m.CreatedDate).Where(m => m.UserId.Equals(Id) && !m.DeleteFlag).FirstOrDefault();
                if (rec != null)
                {
                    strPath = FileDirPaths.DyCertPath + rec.FileName;
                    rec = null;
                }
            }
            catch (Exception)
            {
            }
            return strPath;
        }

       public string GetProfilePercentage(Guid userid)
        {
            float per = 0;
            float count = 0;
            try
            {  
                
                //Personal Information Section Field Counter
                var rec = _db.PersonalInformation.Where(m => m.UserId.Equals(userid)).FirstOrDefault();
                if (rec != null)
                {
                    if (rec.AddressLine1 != null && rec.AddressLine1 != "")
                    {
                        count++;
                    }
                    if (rec.BloodType != 0)
                    {
                        count++;
                    }
                    if (rec.DOB != null && rec.DOB.ToString() != "0001-01-01 00:00:00.0000000")
                    {
                        count++;
                    }
                    if (rec.Cell_Phone != null || rec.Cell_Phone != "")
                    {
                        count++;
                    }
                    if (rec.City_Vill_Town != null || rec.City_Vill_Town != "")
                    {
                        count++;
                    }
                    if (rec.District != null || rec.District != "")
                    {
                        count++;
                    }
                    if (rec.Email != null || rec.Email != "")
                    {
                        count++;
                    }
                    if (rec.FirstName != null || rec.FirstName != "")
                    {
                        count++;
                    }
                    if (rec.Gender != null || rec.Gender != "")
                    {
                        count++;
                    }
                    if (rec.Home_Phone != null || rec.Home_Phone != "")
                    {
                        count++;
                    }
                    if (rec.LastName != null || rec.LastName != "")
                    {
                        count++;
                    }
                    if (rec.Pin != null || rec.Pin != "")
                    {
                        count++;
                    }
                    if (rec.State != 0)
                    {
                        count++;
                    }
                    if (rec.Uhid != null || rec.Uhid != "")
                    {
                        count++;
                    }

                }
                
                
                    //Emergency Information Section Field Counter
                    var rec_emergency = _db.EmergencyInformation.Where(p => p.UserId.Equals(userid)).FirstOrDefault();
                    if(rec_emergency != null)
                    {
                        if(rec_emergency.PC_AddressLine1 != null && rec_emergency.PC_AddressLine1 != "")
                        {
                            count++;
                        }
                        if (rec_emergency.PC_City_Vill_Town != null && rec_emergency.PC_City_Vill_Town != "")
                        {
                            count++;
                        }
                        if (rec_emergency.PC_District != null && rec_emergency.PC_District != "")
                        {
                            count++;
                        }
                        if (rec_emergency.PC_Phone1 != null && rec_emergency.PC_Phone1 != "")
                        {
                            count++;
                        }
                        if (rec_emergency.PC_Pin != null && rec_emergency.PC_Pin != "")
                        {
                            count++;
                        }
                        if (rec_emergency.PC_Relationship != 0)
                        {
                            count++;
                        }
                        if (rec_emergency.PC_State != 0)
                        {
                            count++;
                        }
                        if (rec_emergency.Primary_Emergency_Contact != null && rec_emergency.Primary_Emergency_Contact != "")
                        {
                            count++;
                        }

                    }



                //Employer Information Section Field Counter

                var rec_employer = _db.EmployerInformation.Where(p => p.UserId.Equals(userid)).FirstOrDefault();
                    if(rec_employer != null)
                    {
                        if (rec_employer.EmpAddressLine1 != null && rec_employer.EmpAddressLine1 != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmpCity_Vill_Town != null && rec_employer.EmpCity_Vill_Town != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmpDistrict != null && rec_employer.EmpDistrict != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmployerName != null && rec_employer.EmployerName != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmployerOccupation != null && rec_employer.EmployerOccupation != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmployerPhone != null && rec_employer.EmployerPhone != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmpPin != null && rec_employer.EmpPin != "")
                        {
                            count++;
                        }
                        if (rec_employer.EmpState != 0)
                        {
                            count++;
                        }
                    }


                //Insurance Information Section Field Counter

                var rec_insurance = _db.InsuranceInformation.Where(p => p.UserId.Equals(userid)).FirstOrDefault();
                    if(rec_insurance != null)
                    {
                        if(rec_insurance.Insu_Org_Grp_Num != null && rec_insurance.Insu_Org_Grp_Num != "")
                        {
                            count++;
                        }
                        if (rec_insurance.Insu_Org_Name != null && rec_insurance.Insu_Org_Name != "")
                        {
                            count++;
                        }
                        if (rec_insurance.Insu_Org_Phone != null && rec_insurance.Insu_Org_Phone != "")
                        {
                            count++;
                        }
                        if (rec_insurance.ValidTill != null && rec_insurance.ValidTill.ToString() != "0001-01-01 00:00:00.0000000")
                        {
                            count++;
                        }
                    }

                //Preferance Information Section Field Counter

                var rec_preference = _db.Preferences.Where(p => p.UserId.Equals(userid)).FirstOrDefault();
                    if(rec_preference != null)
                    {
                        if(rec_preference.Pref_Hosp != null && rec_preference.Pref_Hosp != "")
                        {
                            count++;
                        }
                        if (rec_preference.Prim_Care_Prov != null && rec_preference.Prim_Care_Prov != "")
                        {
                            count++;
                        }
                        if (rec_preference.Special_Needs != null && rec_preference.Special_Needs != "")
                        {
                            count++;
                        }
                    }

                per = ((count / 37) * 100);      
                return per.ToString("F0", CultureInfo.InvariantCulture);
            }
            catch(Exception)
            {
                return "false";
            }
           
        }

        public string GetEmailByUserId(Guid UserId)
        {
            string strEmail = "";
            try
            {
                var rec = _db.Users.FirstOrDefault(m => m.UserId.Equals(UserId));
                if (rec != null)
                {
                    strEmail = rec.Email;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return strEmail;
        }

        public float GetBMI(string height, string weight)
        {
            float result = 0;
            try
            {
                float h, w;
                if (float.TryParse(height, out h) && float.TryParse(weight, out w))
                {
                    h = h / 100;
                    result = w / (h * h);
                    h = w = 0;
                    result = float.Parse(Math.Round(result, 2).ToString());
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        public bool ChangePassword(Guid UserId, string NewPassword, string OldPassword)
        {
            bool Status = false;

            var doc = _db.Users.FirstOrDefault(m => m.UserId.Equals(UserId) && m.Password.Equals(OldPassword));
            if (doc != null)
            {
                doc.Password = NewPassword;
                _db.SaveChanges();
                Status = true;
            }
            return Status;
        }


    }
}
