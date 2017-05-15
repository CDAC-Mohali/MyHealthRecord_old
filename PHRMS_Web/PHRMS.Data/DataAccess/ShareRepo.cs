using PHRMS.ViewModels;
using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Http;


namespace PHRMS.Data.DataAccess
{
    public partial class PHRMSRepo
    {
        public bool AddUserShareRecordEntry(UserShareRecordViewModels oUserShareRecordViewModels)
        {
            bool _result = false;
            long userRecId = 0;
            try
            {
                if (oUserShareRecordViewModels != null)
                {
                    Mapper.CreateMap<UserShareRecordViewModels, UserShareRecord>();

                    UserShareRecord objUserShareRecord = Mapper.Map<UserShareRecord>(oUserShareRecordViewModels);
                    //    objUserActivities.TimeStamp = DateTime.Now;
                    var MedicalContactRecords = _db.MedicalContactRecords.Where(s => s.EmailAddress == oUserShareRecordViewModels.DocEmail && s.PrimaryPhone == oUserShareRecordViewModels.DocPhone && s.UserId== oUserShareRecordViewModels.UserId).FirstOrDefault();
                    if (MedicalContactRecords != null)
                    {
                        objUserShareRecord.MedicalContactId = MedicalContactRecords.Id;
                    }
                    _db.UserShareRecord.Add(objUserShareRecord);
                    int res = _db.SaveChanges();
                    userRecId = objUserShareRecord.UserRecordId;

                    #region for saving data to ShareReportNotificationTabel
                    ShareReportNotification oShareReportNotiViewModel = new ShareReportNotification();
                    oShareReportNotiViewModel.UserId = oUserShareRecordViewModels.UserId;
                    oShareReportNotiViewModel.CreatedDate = DateTime.Now;
                    oShareReportNotiViewModel.isViewedByDoctor = false;
                    oShareReportNotiViewModel.isPrescribedByDoctor = false;
                    oShareReportNotiViewModel.isNotificationViewed = false;
                    oShareReportNotiViewModel.UserRecordId = userRecId;
                   _db.ShareReportNotification.Add(oShareReportNotiViewModel);
                    _db.SaveChanges();
                    #endregion
                    oShareReportNotiViewModel = null;
                    oShareReportNotiViewModel = null;
                    objUserShareRecord = null;
                    oUserShareRecordViewModels = null;
                    _result = res > 0;
                }
            }
            catch (Exception ex)
            {
            }
            return _result;
        }
        public bool SaveFeedBack(ShareFeedBack oShareFeedBack,out string DoctorName)
        {
            DoctorName = "";
            try
            {
                ShareReportFeedBack oShareReportFeedBack = new ShareReportFeedBack();
                oShareReportFeedBack.CreatedDate = DateTime.Now;
                oShareReportFeedBack.UserId = oShareFeedBack.UserId;
                oShareReportFeedBack.MedicalContactId = oShareFeedBack.MedicalContactId;
                oShareReportFeedBack.FeedBack = oShareFeedBack.FeedBack;
                oShareReportFeedBack.UserRecordId = oShareFeedBack.UserRecordId;
                _db.ShareReportFeedBack.Add(oShareReportFeedBack);
                _db.SaveChanges();

                #region Save Eprescription from Share Report Feedback
                Eprescription oEprescription = new Eprescription();
                oEprescription.CreatedDate = oShareReportFeedBack.CreatedDate;
                oEprescription.PresDate = oShareReportFeedBack.CreatedDate;
                oEprescription.SourceId = 5;
                oEprescription.UserId = oShareReportFeedBack.UserId;
                oEprescription.ModifiedDate = oShareReportFeedBack.CreatedDate;
                oEprescription.Prescription = oShareReportFeedBack.FeedBack;
                oEprescription.DeleteFlag = false;
                oEprescription.ClinicName = "";
                //Get Medical Contact Detail of Doctor 
                MedicalContactRecords oMedicalContactRecords = _db.MedicalContactRecords.Where(s => s.Id == oShareReportFeedBack.MedicalContactId).FirstOrDefault();
                DoctorName= oEprescription.DocName = oMedicalContactRecords.ContactName;
                oEprescription.DocAddress = oMedicalContactRecords.Address1 + " " + oMedicalContactRecords.Address2;
                oEprescription.DocPhone = oMedicalContactRecords.PrimaryPhone;
                oEprescription.ClinicName = oMedicalContactRecords.ClinicName==null?"":oMedicalContactRecords.ClinicName;
                _db.Eprescription.Add(oEprescription);
                _db.SaveChanges();
                oShareReportFeedBack.EPrescriptionId = oEprescription.Id;
                _db.SaveChanges();

                var shareReportNotification = _db.ShareReportNotification.Where(s => s.UserRecordId == oShareReportFeedBack.UserRecordId ).FirstOrDefault();
                shareReportNotification.isPrescribedByDoctor = true;
                shareReportNotification.isViewedByDoctor = true;
                _db.SaveChanges();

                oMedicalContactRecords = null;
                oEprescription = null;
                #endregion
                oShareReportFeedBack = null;
                return true;

            }
            catch (Exception ex)
            {


            }
            return false;
        }
        //Invoked when doctor view the report shared by user
        public UserShareRecordViewModels GetUserId(string Password)
        {
            UserShareRecord oUserShareRecord = _db.UserShareRecord.Where(s => s.Password == Password).FirstOrDefault();
            if (oUserShareRecord != null)
            {
                #region Update ShareReportNotification table on report viewed by doctor.
                var shareReportNotification = _db.ShareReportNotification.Where(s => s.UserRecordId == oUserShareRecord.UserRecordId).FirstOrDefault();
                shareReportNotification.isViewedByDoctor = true;
                _db.SaveChanges();
                #endregion

                var ss = oUserShareRecord.CreatedDate.AddDays(oUserShareRecord.ValidUpto);
                if (oUserShareRecord.CreatedDate.AddDays(oUserShareRecord.ValidUpto) > DateTime.Now)
                {
                    Mapper.CreateMap<UserShareRecord, UserShareRecordViewModels>();

                    oUserShareRecord.ImagePath = _db.Users.Where(s => s.UserId == oUserShareRecord.UserId).FirstOrDefault().ImgPath;
                    return Mapper.Map<UserShareRecordViewModels>(oUserShareRecord);

                }
                else
                {
                    oUserShareRecord.Password = "Expired....";
                    Mapper.CreateMap<UserShareRecord, UserShareRecordViewModels>();
                    return Mapper.Map<UserShareRecordViewModels>(oUserShareRecord);
                }

            }
            else
                return null;
        }

        public UserShareRecordViewModels GetShareReportDetail(long UserRecordId)
        {
            UserShareRecord oUserShareRecord = _db.UserShareRecord.OrderByDescending(s => s.CreatedDate).Where(s => s.UserRecordId == UserRecordId).FirstOrDefault();
            if (oUserShareRecord != null)
            {
                MedicalContactRecords oMedicalContactRecords= _db.MedicalContactRecords.Where(s => s.Id == oUserShareRecord.MedicalContactId).FirstOrDefault();
                Mapper.CreateMap<UserShareRecord, UserShareRecordViewModels>();
                var obj = Mapper.Map<UserShareRecordViewModels>(oUserShareRecord);
                obj.ClinicName = oMedicalContactRecords.ClinicName;
                obj.DoctorName = oMedicalContactRecords.ContactName;
                return obj;
            }
            else
                return null;
        }

      
    }
}