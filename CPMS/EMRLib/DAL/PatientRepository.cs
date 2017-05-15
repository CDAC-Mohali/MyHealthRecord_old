using System.Collections.Generic;
using System.Linq;
using PagedList;
using EMRLib.DataModels;
using System;
using EMRViewModels;
using AutoMapper;
using System.Data.Entity.Core.Objects;
using System.Security.Cryptography;
using System.Text;

namespace EMRLib.DAL
{
    public static class AppSetting
    {
        public static int PageSize = 6;
    }
    public partial class EMRRepository
    {
        public IPagedList<DocPatientDetailsViewModel> GetPatientsList(int page, Guid DocId)
        {
            IPagedList<DocPatientDetailsViewModel> lstDocPatientDetails = null;
            try
            {
                Mapper.CreateMap<DocPatientDetails, DocPatientDetailsViewModel>();
                lstDocPatientDetails = Mapper.Map<List<DocPatientDetails>, List<DocPatientDetailsViewModel>>(context.DocPatientDetails.Where(m => m.DocId.Equals(DocId)).ToList()).OrderBy(s => s.FirstName).ToList().ToPagedList(page, 100);

                if (page > 1 && lstDocPatientDetails.Count() == 0)
                {
                    lstDocPatientDetails = Mapper.Map<List<DocPatientDetails>, List<DocPatientDetailsViewModel>>(context.DocPatientDetails.Where(m => m.DocId.Equals(DocId)).ToList()).OrderBy(s => s.FirstName).ToList().ToPagedList(page, 100 - 1);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lstDocPatientDetails;
        }

        public IPagedList<DocPatientDetailsViewModel> GetPatientsListSearch(int page, Guid DocId, string Searchtext)
        {
            IPagedList<DocPatientDetailsViewModel> lstDocPatientDetails = null;
            try
            {
                Mapper.CreateMap<DocPatientDetails, DocPatientDetailsViewModel>();
                lstDocPatientDetails = Mapper.Map<List<DocPatientDetails>, List<DocPatientDetailsViewModel>>(context.DocPatientDetails.Where(x => x.PhoneNumber.StartsWith(Searchtext) || x.FirstName.StartsWith(Searchtext)).Where(m => m.DocId.Equals(DocId)).ToList()).OrderBy(s => s.CreatedDate).ToList().ToPagedList(page, AppSetting.PageSize);

                if (page > 1 && lstDocPatientDetails.Count() == 0)
                {
                    lstDocPatientDetails = Mapper.Map<List<DocPatientDetails>, List<DocPatientDetailsViewModel>>(context.DocPatientDetails.Where(x => x.PhoneNumber.StartsWith(Searchtext) || x.FirstName.StartsWith(Searchtext)).Where(m => m.DocId.Equals(DocId)).ToList()).OrderBy(s => s.FirstName).ToList().ToPagedList(page, AppSetting.PageSize - 1);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return lstDocPatientDetails;
        }


        public DocPatientDetailsViewModel CheckPatientByDoc(DocPatientDetailsViewModel oDocPatientDetailsViewModel)
        {
            DocPatientDetailsViewModel result = null;
            try
            {
                DocPatientDetails oDocPatientDetails = context.DocPatientDetails.Where(s => s.DocId == oDocPatientDetailsViewModel.DocId && ((!string.IsNullOrEmpty(oDocPatientDetailsViewModel.PhoneNumber) ? s.PhoneNumber == oDocPatientDetailsViewModel.PhoneNumber : false) || (!string.IsNullOrEmpty(oDocPatientDetailsViewModel.EmailAddress) ? s.EmailAddress == oDocPatientDetailsViewModel.EmailAddress : false))).FirstOrDefault();
                if (oDocPatientDetails != null)
                {
                    Mapper.CreateMap<DocPatientDetails, DocPatientDetailsViewModel>();
                    result = Mapper.Map<DocPatientDetails, DocPatientDetailsViewModel>(oDocPatientDetails);
                    result.strDOB = result.DOB.ToString("dd/MM/yyyy");

                }

            }
            catch (Exception ex)
            {
                result = null;
                //    throw;
            }
            return result;
        }
        public List<Appointment_FeesViewModel> LoadDataForAppointments(Guid docId, DateTime date)
        {
            try
            {
                List<Appointment_FeesViewModel> result = (from m in context.Appointment_Fees.OrderBy(s => s.Date)
                                                          where m.DocId == docId && EntityFunctions.TruncateTime(m.Date) == date.Date
                                                          select new Appointment_FeesViewModel
                                                          {
                                                              Mobile = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().PhoneNumber,
                                                              Name = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().FirstName,
                                                              Date = m.Date,
                                                              Email = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().EmailAddress,
                                                              Mins = m.Mins,
                                                              Hours = m.Hours,
                                                              meridiem = m.meridiem
                                                          }).ToList();
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
            // var obj = context.Appointment_Fees.Where(s => s.DocId == docId && EntityFunctions.TruncateTime(s.Date) == oDate.Date).ToList();

            //    result = Mapper.Map<List<Appointment_Fees>, List<Appointment_FeesViewModel>>(obj);


        }
        public IPagedList<Appointment_FeesViewModel> GetAppointmentsByDate(Guid docId, int Type, int page = 1)
        {
            IPagedList<Appointment_FeesViewModel> result = null;
            DateTime oDate = DateTime.Now;
            DateTime Week = oDate.Date.AddDays(7);
            DateTime Month = oDate.Date.AddMonths(1);
            try
            {
                Mapper.CreateMap<Users, UsersViewModel>();
                Mapper.CreateMap<UsersViewModel, Users>();
                Mapper.CreateMap<Doctor, DoctorViewModel>();
                Mapper.CreateMap<DoctorViewModel, Doctor>();
                Mapper.CreateMap<Appointment_Fees, Appointment_FeesViewModel>().ForMember(s => s.DoctorViewModel, c => c.MapFrom(m => m.doctors)).ForMember(s => s.UsersViewModel, c => c.MapFrom(m => m.users)); ;
                if (Type == 1)//Current Day
                {
                    result = (from m in context.Appointment_Fees.OrderBy(s => s.Date)
                              where m.DocId == docId && EntityFunctions.TruncateTime(m.Date) == oDate.Date
                              select new Appointment_FeesViewModel
                              {
                                  Mobile = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().PhoneNumber,
                                  Name = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().FirstName,
                                  Date = m.Date,
                                  Email = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().EmailAddress,
                                  UserId = m.UserId,
                                  PrescriptionId = m.PrescriptionId,
                                  Mins = m.Mins,
                                  Hours = m.Hours,
                                  meridiem = m.meridiem
                              }).ToList().ToPagedList(page, AppSetting.PageSize);

                    // var obj = context.Appointment_Fees.Where(s => s.DocId == docId && EntityFunctions.TruncateTime(s.Date) == oDate.Date).ToList();

                    //    result = Mapper.Map<List<Appointment_Fees>, List<Appointment_FeesViewModel>>(obj);

                }
                else if (Type == 2)//next week Including Current Day
                {

                    result = (from m in context.Appointment_Fees.OrderBy(s => s.Date)
                              where m.DocId == docId && EntityFunctions.TruncateTime(m.Date) >= oDate.Date && EntityFunctions.TruncateTime(m.Date) < Week
                              select new Appointment_FeesViewModel
                              {
                                  Mobile = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().PhoneNumber,
                                  Name = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().FirstName,
                                  Date = m.Date,
                                  Email = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().EmailAddress,
                                  UserId = m.UserId,
                                  PrescriptionId = m.PrescriptionId,
                                  Mins = m.Mins,
                                  Hours = m.Hours,
                                  meridiem = m.meridiem
                              }).ToList().ToPagedList(page, AppSetting.PageSize);
                }
                else if (Type == 3)//next month Including Current Day
                {

                    result = (from m in context.Appointment_Fees.OrderBy(s => s.Date)
                              where m.DocId == docId && EntityFunctions.TruncateTime(m.Date) >= oDate.Date && EntityFunctions.TruncateTime(m.Date) >= oDate.Date && EntityFunctions.TruncateTime(m.Date) < Month
                              select new Appointment_FeesViewModel
                              {
                                  Mobile = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().PhoneNumber,
                                  Name = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().FirstName,
                                  Date = m.Date,
                                  Email = context.DocPatientDetails.OrderByDescending(s => s.DocPatientId).Where(s => s.DocId == docId && s.PHRMSUserId == m.UserId).FirstOrDefault().EmailAddress,
                                  UserId = m.UserId,
                                  PrescriptionId = m.PrescriptionId,
                                  Mins = m.Mins,
                                  Hours = m.Hours,
                                  meridiem = m.meridiem
                              }).ToList().ToPagedList(page, AppSetting.PageSize);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }
        public long GetTotalAppointmentByDoctor(Guid docId)
        {

            try
            {
                return context.Appointment_Fees.Where(s => s.DocId == docId).Count();
            }
            catch (Exception ex)
            {


            }
            return 0;
        }
        public DocPatientDetailsViewModel GetPatientById(Guid DocId, long PatId)
        {
            DocPatientDetailsViewModel result = null;
            try
            {
                DocPatientDetails oDocPatientDetails = context.DocPatientDetails.Where(s => s.DocId.Equals(DocId) && s.DocPatientId.Equals(PatId)).FirstOrDefault();
                if (oDocPatientDetails != null)
                {
                    Mapper.CreateMap<DocPatientDetails, DocPatientDetailsViewModel>();
                    result = Mapper.Map<DocPatientDetails, DocPatientDetailsViewModel>(oDocPatientDetails);
                    result.strDOB = result.DOB.ToString("dd/MM/yyyy");
                }

            }
            catch (Exception)
            {
                result = null;
                //    throw;
            }
            return result;
        }
        internal static string GetHashInternal(string input)
        {
            using (HashAlgorithm ha = new SHA512CryptoServiceProvider())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = ha.ComputeHash(Encoding.UTF8.GetBytes(input));

                return ByteArrayToString(data);
            }
        }
        private static string ByteArrayToString(byte[] data)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        public Guid SavePatientDetails(DocPatientDetailsViewModel model, out UsersViewModel Status)
        {
            Status = new UsersViewModel();
            Status.IsEmailSend = false;
            Guid PHRMSUserId = Guid.Empty;
            try
            {
                DocPatientDetails oModel = context.DocPatientDetails.FirstOrDefault(m => m.DocId.Equals(model.DocId) && (m.DocPatientId.Equals(model.DocPatientId)));
                if (oModel != null)//Already Clininc Patient
                {
                    if (model.PhoneNumber != null && model.PhoneNumber != "" && model.PhoneNumber != "-" && oModel.PhoneNumber == "-")
                    {
                        Status.IsEmailSend = true;
                        string CreatePassword = new Random().Next(0, 100000).ToString();
                        var Users = context.Users.Where(m => m.UserId == oModel.PHRMSUserId).FirstOrDefault();
                        Users.MobileNo = model.PhoneNumber;
                        Users.Email = model.EmailAddress;
                        Users.Password = "";
                        context.SaveChanges();
                        Status.Email = model.EmailAddress;
                        Status.MobileNo = model.PhoneNumber;
                        Status.Password = CreatePassword;
                        Status.Status = true;
                    }
                    oModel.AadhaarNumber = model.AadhaarNumber;
                    oModel.Address1 = model.Address1;
                    oModel.Address2 = model.Address2;
                    oModel.City_Vill_Town = model.City_Vill_Town;
                    oModel.District = model.District;
                    //oModel.DOB = string.IsNullOrEmpty(model.strDOB) ? (DateTime?)null : DateTime.Parse(model.strDOB);
                    oModel.DOB = DateTime.ParseExact(model.strDOB, "dd/MM/yyyy", null);
                    oModel.EmailAddress = model.EmailAddress;
                    oModel.PhoneNumber = model.PhoneNumber;
                    oModel.State = model.State;
                    oModel.FirstName = model.FirstName;
                    oModel.LastName = model.LastName;
                    oModel.Gender = model.Gender;
                    context.SaveChanges();
                    Status.DocPatientId = model.DocPatientId;
                    Status.Status = true;
                    if (!oModel.Equals(Guid.Empty))
                        PHRMSUserId = oModel.PHRMSUserId.Value;
                }
                else
                {
                    Mapper.CreateMap<DocPatientDetailsViewModel, DocPatientDetails>();
                    oModel = Mapper.Map<DocPatientDetailsViewModel, DocPatientDetails>(model);
                    oModel.CreatedDate = oModel.ModifiedDate = DateTime.Now;

                    string CreatePassword = new Random().Next(0, 100000).ToString();
                    Users phrmsUser = null;
                    if (model.PhoneNumber != null && model.PhoneNumber != "")
                        phrmsUser = context.Users.FirstOrDefault(m => m.MobileNo.Equals(model.PhoneNumber));

                    if (phrmsUser != null)//Already Phrms User
                    {
                        oModel.PHRMSUserId = phrmsUser.UserId;
                    }
                    else
                    {
                        if (model.PhoneNumber != null && model.PhoneNumber != "")
                            Status.IsEmailSend = true;
                        else
                        {
                            model.PhoneNumber = "-";
                            oModel.PhoneNumber = "-";
                        }
                        Users oUsers = new Users();
                        oUsers.UserId = Guid.NewGuid();
                        oUsers.AadhaarNo = model.AadhaarNumber;
                        oUsers.CUG = "";
                        oUsers.CreatedDate = DateTime.Now;
                        oUsers.Email = model.EmailAddress;
                        oUsers.FirstName = model.FirstName;
                        oUsers.LastName = model.LastName;
                        oUsers.MobileNo = model.PhoneNumber;
                        oUsers.Password = "";
                        oUsers.PwdChangeDate = DateTime.Now;
                        oUsers.RegDate = DateTime.Now;
                        oUsers.RoleId = 2;
                        oUsers.Status = true;
                        context.Users.Add(oUsers);
                        context.SaveChanges();
                        oModel.PHRMSUserId = oUsers.UserId;
                        Status.Email = model.EmailAddress;
                        Status.MobileNo = model.PhoneNumber;
                        Status.Password = CreatePassword;
                        Status.Status = true;
                        PersonalInformation oPersonalInformation = new PersonalInformation();
                        try
                        {
                            oPersonalInformation.Id = Guid.NewGuid();
                            oPersonalInformation.AddressLine1 = model.Address1;
                            oPersonalInformation.AddressLine2 = model.Address2;
                            oPersonalInformation.BloodType = 0;
                            oPersonalInformation.Cell_Phone = model.PhoneNumber;
                            oPersonalInformation.City_Vill_Town = model.City_Vill_Town;
                            oPersonalInformation.DAbilityType = 0;
                            oPersonalInformation.DOB = model.DOB;
                            oPersonalInformation.DiffAbled = false;
                            oPersonalInformation.District = model.District;
                            oPersonalInformation.Email = model.EmailAddress;
                            oPersonalInformation.FirstName = model.FirstName;
                            oPersonalInformation.Gender = model.Gender;
                            oPersonalInformation.Home_Phone = model.PhoneNumber;
                            oPersonalInformation.LastName = model.LastName;
                            oPersonalInformation.State = model.State;
                            oPersonalInformation.Uhid = model.AadhaarNumber;
                            oPersonalInformation.UserId = oUsers.UserId;
                            oPersonalInformation.DOB = DateTime.ParseExact(model.strDOB, "dd/MM/yyyy", null);
                            context.PersonalInformation.Add(oPersonalInformation);
                            context.SaveChanges();

                        }
                        catch (Exception ex)
                        {


                        }

                        oPersonalInformation = null;
                        PHRMSUserId = oModel.PHRMSUserId.Value;
                    }

                    //}
                    #region Inssert Doc Patinet Mapping

                    oModel.DOB = DateTime.ParseExact(model.strDOB, "dd/MM/yyyy", null);
                    context.DocPatientDetails.Add(oModel);
                    context.SaveChanges();
                    Status.DocPatientId = oModel.DocPatientId;
                    Status.Status = true;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return PHRMSUserId;
        }


        public List<PatientReginModel> PatientRegionGraph(Guid docId)
        {

            EMRDBContext _db = new EMRDBContext();
            var oPersonalInfo = _db.DocPatientDetails.Where(s => s.State > 0 && s.State < 37 && s.DocId == docId).ToList();
            var oPersonalInformation = oPersonalInfo.GroupBy(s => s.State).ToList();
            var res = oPersonalInformation.Select(s => new PatientReginModel
            { OthersCount = s.Where(q => q.Gender.Contains("U")).Count(), FeMaleCount = s.Where(q => q.Gender.Contains("F")).Count(), MaleCount = s.Where(q => q.Gender.Contains("M")).Count(), StateCount = s.Count(), StateId = _db.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().Id, StateName = s.Key == 0 ? "Not Specified" : _db.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().Name }).ToList();
            //res.Add(new PatientReginModel { StateCount = res.Sum(s => s.OthersCount) + _db.Users.Count() - oPersonalInfo.Count(), StateName = "Not Specified" });

            return res;

        }

        public PatientReginModel PatientGenderGraph(Guid docId)
        {
            PatientReginModel oPatientCustomModel = new PatientReginModel();
            EMRDBContext _db = new EMRDBContext();
            int Count = _db.DocPatientDetails.Where(x => x.DocId == docId).Count();
            var res = _db.DocPatientDetails.Where(x => x.DocId == docId).ToList();
            oPatientCustomModel.MaleCount = res.Where(s => s.Gender == "M").Count();
            oPatientCustomModel.FeMaleCount = res.Where(s => s.Gender == "F").Count();
            oPatientCustomModel.Count = Count.ToString();
            //  oPatientCustomModel.UnSpecifiedCount = res.Where(s => s.Gender == "F").Count().ToString();
            oPatientCustomModel.OthersCount = (Count - res.Where(s => s.Gender == "M").Count() - res.Where(s => s.Gender == "F").Count());
            return oPatientCustomModel;

        }

        public PatientReginModel EMRPatientDetail(Guid docId)
        {
            PatientReginModel oPatientCustomModel = new PatientReginModel();
            EMRDBContext _db = new EMRDBContext();
            DateTime Startdate = DateTime.Now.Date;
            DateTime Nextdate = DateTime.Now.Date.AddDays(1);
            DateTime LastWeek = DateTime.Now.Date.AddDays(-8);
            DateTime LastYear = DateTime.Now.Date.AddYears(-1);
            DateTime LastMonth = DateTime.Now.Date.AddMonths(-1);
            var res = _db.DocPatientDetails.Where(x => x.DocId == docId).ToList();
            oPatientCustomModel.Count = res.Count().ToString();
            oPatientCustomModel.Day = res.Where(s => s.CreatedDate >= Startdate && s.CreatedDate < Nextdate).Count().ToString();
            oPatientCustomModel.Week = res.Where(s => s.CreatedDate > LastWeek && s.CreatedDate < Startdate).Count().ToString();
            oPatientCustomModel.Month = res.Where(s => s.CreatedDate > LastMonth && s.CreatedDate < Startdate).Count().ToString();
            oPatientCustomModel.Year = res.Where(s => s.CreatedDate > LastYear && s.CreatedDate < Startdate).Count().ToString();
            return oPatientCustomModel;
        }

        public WeekModel GetPatientLastWeekDetail(Guid docId)
        {
            WeekModel weemodeobj = new WeekModel();
            EMRDBContext _db = new EMRDBContext();
            DateTime date = DateTime.Now;
            DateTime mondayOfLastWeek = date.AddDays(-(int)date.DayOfWeek - 6);
            DateTime Startdate = DateTime.Now.Date;
            DateTime Nextdate = DateTime.Now.Date;
            var res = _db.DocPatientDetails.Where(x => x.DocId == docId).ToList();
            weemodeobj.Monday = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date).Count();
            weemodeobj.Tuesday = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date.AddDays(1)).Count();
            weemodeobj.Wednesdaty = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date.AddDays(2)).Count();
            weemodeobj.Thursday = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date.AddDays(3)).Count();
            weemodeobj.Friday = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date.AddDays(4)).Count();
            weemodeobj.Saturday = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date.AddDays(5)).Count();
            weemodeobj.Sunday = res.Where(s => s.CreatedDate.Date == mondayOfLastWeek.Date.AddDays(6)).Count();
            return weemodeobj;

        }

    }
    }
