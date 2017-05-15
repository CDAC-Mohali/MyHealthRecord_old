using AutoMapper;
using EMRLib.DataModels;
using EMRViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EMRLib.DAL
{
    public partial class EMRRepository : IEMRRepository
    {
        EMRDBContext context = new EMRDBContext();

        public bool ChangePassword(Guid DocId, string NewPassword, string OldPassword)
        {
            bool Status = false;
            OldPassword = "";
            var doc = context.Doctor.FirstOrDefault(m => m.docid == DocId && m.password.Equals(OldPassword));
            if (doc != null)
            {
                doc.password = "";
                context.SaveChanges();
                Status = true;
            }
            return Status;
        }
        public DoctorViewModel Login(string UserName, string Password)
        {
            DoctorViewModel DoctorViewModel = new DoctorViewModel();
            try
            {
                Mapper.CreateMap<Doctor, DoctorViewModel>();
                DoctorViewModel = Mapper.Map<Doctor, DoctorViewModel>(context.Doctor.Where(x => (x.email.Equals(UserName) || x.phone_number.Equals(UserName)) && x.password == Password && x.IsApproved == 2).FirstOrDefault());


            }
            catch (Exception ex)
            {
                DoctorViewModel = null;
            }
            return DoctorViewModel;
        }

        public DoctorViewModel LoginByOTP(Guid docid)
        {
            DoctorViewModel DoctorViewModel = new DoctorViewModel();
            try
            {

                Mapper.CreateMap<Doctor, DoctorViewModel>();
                DoctorViewModel = Mapper.Map<Doctor, DoctorViewModel>(context.Doctor.Where(x => (x.docid.Equals(docid)) && x.IsApproved == 2).FirstOrDefault());


            }
            catch (Exception ex)
            {
                DoctorViewModel = null;
            }
            return DoctorViewModel;
        }




        public string GetFilePath(Guid docid)
        {
            string Path = "";
            try
            {

                var obj = context.EMRDocFilePath.Where(x => (x.DocId.Equals(docid) && !x.DeleteFlag)).FirstOrDefault();
                if (obj != null)
                {
                    Path = obj.filePath;
                }
            }
            catch (Exception)
            {


            }
            return Path;
        }
        public Guid AddDoctor(Doctor oModel)
        {
            Guid result = Guid.Empty;
            try
            {
                oModel.request_time = DateTime.Now;
                oModel.docid = Guid.NewGuid();
                oModel.IsApproved = 0;//Pending Approval
                oModel.password = "";
                context.Doctor.Add(oModel);
                if (context.SaveChanges() > 0)
                    result = oModel.docid;
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public bool UpdateDoctorStatus(Doctor oModel)
        {
            Guid result = Guid.Empty;
            try
            {
                var obj = context.Doctor.Where(x => (x.docid == oModel.docid && x.delete_flag == false && x.IsApproved == 0)).FirstOrDefault();
                obj.IsApproved = oModel.IsApproved;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public bool CompleteRegistration(Places_of_Practice oModel, string fileName)
        {
            bool result = false;
            try
            {
                Guid PoPId = AddPlaceofPractice(oModel);
                if (!PoPId.Equals(Guid.Empty))
                    result = SaveFile(fileName, FileTypes.Photo, oModel.docid, PoPId);
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public Guid AddPlaceofPractice(Places_of_Practice oModel)
        {
            Guid PoPId = Guid.Empty;
            try
            {
                oModel.id = Guid.NewGuid();
                context.Places_of_Practice.Add(oModel);
                if (context.SaveChanges() > 0)
                    PoPId = oModel.id;
            }
            catch (Exception ex)
            {
            }
            return PoPId;
        }

        public bool SaveFile(string strFileName, FileTypes fileType, Guid recId, Guid userId)
        {
            bool result = false;
            try
            {
                EMRFiles oFile = new EMRFiles();
                oFile.CreatedDate = DateTime.Now;
                oFile.DeleteFlag = false;
                oFile.FileName = strFileName;
                oFile.FileType = fileType;
                oFile.RecId = recId;
                oFile.UserId = userId;
                context.EMRFiles.Add(oFile);
                result = context.SaveChanges() > 0;
            }
            catch (Exception)
            {
            }
            return result;
        }

        public bool SetProfilePic(string path, Guid docId)
        {
            EMRDocFilePath docFilePath = new EMRDocFilePath();
            docFilePath.FileType = FileType.ProfilePic;
            docFilePath.FileName = Path.GetFileName(path);
            docFilePath.filePath = path;
            docFilePath.DocId = docId;
            if (SoftDeleteFileIfExists(docFilePath))
            {
                SaveFileDetails(docFilePath);
            }
            return true;  //updatePofilePicPath(path, userID);

        }



        public bool SoftDeleteFileIfExists(EMRDocFilePath docFilePath)
        {
            bool res = false;
            try
            {
                var recs = context.EMRDocFilePath.Where(m => m.FileType == (FileType)docFilePath.FileType && m.DocId.Equals(docFilePath.DocId) && !m.DeleteFlag).ToList();
                if (recs != null && recs.Count() > 0)
                {
                    foreach (var rec in recs)
                    {
                        rec.DeleteFlag = true;
                    }
                    res = context.SaveChanges() > 0;
                }
                else
                    res = true;
            }
            catch (Exception)
            {
            }
            return res;
        }

        public bool SaveFileDetails(EMRDocFilePath docFilePath)
        {
            bool result = false;
            try
            {
                //Mapper.CreateMap<EMRViewModels.FileViewModel, FilePath>();
                //FilePath oFilePath = Mapper.Map<FilePath>(docFilePath);
                context.EMRDocFilePath.Add(docFilePath);
                result = context.SaveChanges() > 0;
            }
            catch (Exception)
            {
            }
            return result;
        }
        //public bool updatePofilePicPath(string path, Guid userID)
        //{
        //    int res;
        //    try
        //    {
        //        var user = context.Users.FirstOrDefault(m => m.UserId.Equals(userID));
        //        user.ImgPath = path;
        //        res = context.SaveChanges();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return res > 0;
        //}


        public bool DoesMobileExist(string strMobileNo)
        {
            bool result = true;
            try
            {
                var record = context.Doctor.FirstOrDefault(m => m.phone_number.Equals(strMobileNo));

                if (record != null)
                {
                    result = false;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public bool DoesEmailExist(string strEmail)
        {
            bool result = true;
            try
            {
                var record = context.Doctor.FirstOrDefault(m => m.email.Equals(strEmail));

                if (record != null)
                {
                    result = false;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }

        public ForgotPasswordModel VerifyUsernameforPasswordChange(ForgotPasswordModel oModel)
        {
            DoctorViewModel oUsersViewModel = null;
            try
            {
                oUsersViewModel = GetUsersDetails(oModel.UserName);
                if (oUsersViewModel != null)
                {
                    oModel.Email = oUsersViewModel.email;
                    oModel.MobileNo = oUsersViewModel.phone_number;
                    oModel.StatusCode = FPProcessStatus.Success;
                    oModel.Id = oUsersViewModel.docid;
                    oUsersViewModel = null;
                }
                else
                    oModel.StatusCode = FPProcessStatus.VerificationFailure;
            }
            catch (Exception)
            {
                oModel.StatusCode = FPProcessStatus.SysFailure;
            }

            return oModel;
        }


        public DoctorViewModel GetUsersDetails(string strUsername)
        {
            DoctorViewModel oUsersViewModel = null;
            try
            {
                var user = context.Doctor.FirstOrDefault(m => (m.email.Equals(strUsername) || m.phone_number.Equals(strUsername)) && m.IsApproved == 2);
                Mapper.CreateMap<Doctor, DoctorViewModel>();
                oUsersViewModel = Mapper.Map<DoctorViewModel>(user);
                user = null;
            }
            catch (Exception)
            {

                throw;
            }
            return oUsersViewModel;
        }

        public bool ResetPassword(Guid userId, string Password)
        {
            CommonComponent commonMethods = null;
            bool result = false;
            try
            {
                commonMethods = new CommonComponent();
                Password = "";
                result = SetPassword(userId, "");
            }
            catch (Exception)
            {
            }
            commonMethods = null;
            return result;
        }

        public bool AddContactUs(ContactusViewModel ContactusViewModel)
        {
            bool _result = false;
            try
            {
                if (ContactusViewModel != null)
                {

                    Mapper.CreateMap<ContactusViewModel, ContactUs>();
                    ContactUs objContactUs = new ContactUs();
                    //objContactUs.ContactUsId = ContactusViewModel.ContactUsId;
                    objContactUs.FirstName = ContactusViewModel.FirstName;
                    objContactUs.LastName = ContactusViewModel.LastName;
                    objContactUs.City = ContactusViewModel.City;
                    objContactUs.State = ContactusViewModel.State;

                    objContactUs.MobileNo = ContactusViewModel.MobileNo;
                    objContactUs.Message = ContactusViewModel.Message;
                    objContactUs.Email = ContactusViewModel.Email;
                    objContactUs.Status = 2;
                    context.ContactUs.Add(objContactUs);
                    _result = context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
            }

            return _result;
        }


        public bool SetPassword(Guid userId, string Password)
        {
            bool res = false;
            try
            {
                var user = context.Doctor.FirstOrDefault(m => m.docid.Equals(userId));
                if (!user.password.Equals(Password))
                {
                    user.password = Password;
                    res = context.SaveChanges() > 0;
                }
                else
                    res = true;
                user = null;
            }
            catch (Exception)
            {
            }
            return res;
        }

        public bool DoesEmailOrMobileExist(string strUserName)
        {
            bool result = false;
            try
            {
                var record = context.Doctor.FirstOrDefault(m => m.email.Equals(strUserName) || m.phone_number.Equals(strUserName));

                if (record != null)
                {
                    result = true;
                    record = null;
                }
            }
            catch (Exception)
            { }
            return result;
        }


    }
}
