using PHRMSAdmin.DALayer;
using PHRMSAdmin.Library;
using PHRMSAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PHRMSAdmin.Controllers
{
    [AdminAuthorizationFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DashBoard()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult DoctorDashBoard()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PatientDetail()
        {
            PatientCustomModel oPatientCustomModel = new PatientCustomModel();
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {


                DateTime Startdate = DateTime.Now.Date;
                DateTime Nextdate = DateTime.Now.Date;

                var res = oPHRMSDBContext.Users.ToList();

                oPatientCustomModel.Count = res.Count().ToString();
                oPatientCustomModel.Day = res.Where(s => s.CreatedDate.Date == Startdate).Count().ToString();
                oPatientCustomModel.Week = res.Where(s => s.CreatedDate.Date > Startdate.AddDays(-7) && s.CreatedDate.Date <= Startdate).Count().ToString();
                oPatientCustomModel.Month = res.Where(s => s.CreatedDate.Date > Startdate.AddMonths(-1) && s.CreatedDate.Date <= Startdate).Count().ToString();
                oPatientCustomModel.Year = res.Where(s => s.CreatedDate.Date > Startdate.AddYears(-1) && s.CreatedDate.Date <= Startdate).Count().ToString();
            }
            catch (Exception)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_PatientDetail", oPatientCustomModel);
        }

        public ActionResult EMRPatientDetail()
        {
            PatientCustomModel oPatientCustomModel = new PatientCustomModel();
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);

                DateTime Startdate = DateTime.Now.Date;
                DateTime Nextdate = DateTime.Now.Date.AddDays(1);
                DateTime LastWeek = DateTime.Now.Date.AddDays(-8);
                DateTime LastYear = DateTime.Now.Date.AddYears(-1);
                DateTime LastMonth = DateTime.Now.Date.AddMonths(-1);
                var res = oPHRMSDBContext.DocPatientDetails    // your starting point - table in the "from" statement
    .Join(oPHRMSDBContext.Doctor, // the source table of the inner join
       post => post.DocId,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
       meta => meta.docid,   // Select the foreign key (the second part of the "on" clause)
       (post, meta) => new { Post = post, Meta = meta }) // selection
    .Where(postAndMeta => postAndMeta.Meta.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId);

                // var res = oPHRMSDBContext.DocPatientDetails.Where(s => s.medi ==.ToList();

                oPatientCustomModel.Count = res.Count().ToString();
                oPatientCustomModel.Day = res.Where(s => s.Post.CreatedDate >= Startdate && s.Post.CreatedDate < Nextdate).Count().ToString();
                oPatientCustomModel.Week = res.Where(s => s.Post.CreatedDate > LastWeek && s.Post.CreatedDate < Startdate).Count().ToString();
                oPatientCustomModel.Month = res.Where(s => s.Post.CreatedDate > LastMonth && s.Post.CreatedDate < Startdate).Count().ToString();
                oPatientCustomModel.Year = res.Where(s => s.Post.CreatedDate > LastYear && s.Post.CreatedDate < Startdate).Count().ToString();
            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_EMRPatientDetail", oPatientCustomModel);
        }

        public ActionResult DoctorDetail()
        {
            PatientCustomModel oPatientCustomModel = new PatientCustomModel();
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {


                DateTime Startdate = DateTime.Now.Date;
                DateTime Nextdate = DateTime.Now.Date;
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                List<Doctor> res;
                if (oCustomPrincipalSerializeModel.IsSuperAdmin)
                    res = oPHRMSDBContext.Doctor.Where(s => s.IsApproved == 2 && s.ApprovedDate != null).ToList();
                else
                    res = oPHRMSDBContext.Doctor.Where(s => s.IsApproved == 2 && s.ApprovedDate != null && s.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId).ToList();

                oPatientCustomModel.Count = res.Count().ToString();
                oPatientCustomModel.Day = res.Where(s => s.ApprovedDate.Value.Date == Startdate).Count().ToString();
                oPatientCustomModel.Week = res.Where(s => s.ApprovedDate.Value.Date > Startdate.AddDays(-7) && s.ApprovedDate.Value.Date <= Startdate).Count().ToString();
                oPatientCustomModel.Month = res.Where(s => s.ApprovedDate.Value.Date > Startdate.AddMonths(-1) && s.ApprovedDate.Value.Date <= Startdate).Count().ToString();
                oPatientCustomModel.Year = res.Where(s => s.ApprovedDate.Value.Date > Startdate.AddYears(-1) && s.ApprovedDate.Value.Date <= Startdate).Count().ToString();
            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            AllergyGraph();
            return PartialView("_DoctorDetail", oPatientCustomModel);
        }
        public ActionResult AllergyGraph()
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                var res = oPHRMSDBContext.Allergies.Where(s => s.DeleteFlag == false && s.Still_Have == true).GroupBy(s => s.AllergyType).Select(s => new AllergyModel { AllergyType = s.Count(), AllergyName = oPHRMSDBContext.AllergyMaster.Where(q => q.Id == s.Key).FirstOrDefault().AllergyName }).ToList();
                return PartialView("_AllergyGraph", res);

            }
            catch (Exception)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_AllergyGraph", new List<AllergyMasterModel>());
        }
        public ActionResult ProblemGraph()
        {
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                var res = oPHRMSDBContext.HealthCondition.Where(s => s.DeleteFlag == false && s.StillHaveCondition == true).GroupBy(s => s.ConditionType).Select(s => new HealthConditionModel { ConditionType = s.Count(), HealthCondition = oPHRMSDBContext.HealthConditionMaster.Where(q => q.Id == s.Key).FirstOrDefault().HealthCondition }).ToList();
                return PartialView("_ProblemGraph", res);

            }
            catch (Exception)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_ProblemGraph", new List<HealthConditionModel>());
        }

        public ActionResult DoctorMaleFemaleGraph()
        {
            PatientCustomModel oPatientCustomModel = new PatientCustomModel();
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                List<Doctor> res;
                if (oCustomPrincipalSerializeModel.IsSuperAdmin)
                    res = oPHRMSDBContext.Doctor.Where(s => s.IsApproved == 2).ToList();
                else
                    res = oPHRMSDBContext.Doctor.Where(s => s.IsApproved == 2 && s.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId).ToList();
                oPatientCustomModel.Count = res.Count().ToString();
                oPatientCustomModel.MaleCount = res.Where(s => s.Gender.Contains("M")).Count();
                oPatientCustomModel.FeMaleCount = res.Where(s => s.Gender.Contains("F")).Count();
                oPatientCustomModel.OthersCount = res.Where(s => s.Gender.Contains("U") || s.Gender == "" || s.Gender == "\0" || s.Gender == null).Count();
                return PartialView("_DoctorMaleFemaleGraph", oPatientCustomModel);

            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_DoctorMaleFemaleGraph", new PatientCustomModel());
        }
        public ActionResult EMRPatientMaleFemaleGraph()
        {
            PatientCustomModel oPatientCustomModel = new PatientCustomModel();
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);

                var res = oPHRMSDBContext.DocPatientDetails    // your starting point - table in the "from" statement
.Join(oPHRMSDBContext.Doctor, // the source table of the inner join
   post => post.DocId,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
   meta => meta.docid,   // Select the foreign key (the second part of the "on" clause)
   (post, meta) => new { Post = post, Meta = meta }) // selection
.Where(postAndMeta => postAndMeta.Meta.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId);
                int Count = res.Count();
                // var res = oPHRMSDBContext.PersonalInformation.ToList();
                oPatientCustomModel.MaleCount = res.Where(s => s.Post.Gender == "M").Count();
                oPatientCustomModel.FeMaleCount = res.Where(s => s.Post.Gender == "F").Count();
                oPatientCustomModel.Count = Count.ToString();
                //  oPatientCustomModel.UnSpecifiedCount = res.Where(s => s.Gender == "F").Count().ToString();
                oPatientCustomModel.OthersCount = (Count - res.Where(s => s.Post.Gender == "M").Count() - res.Where(s => s.Post.Gender == "F").Count());
                return PartialView("_EMRPatientMaleFemaleGraph", oPatientCustomModel);

            }
            catch (Exception)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_EMRPatientMaleFemaleGraph", new PatientCustomModel());
        }
        public ActionResult PatientMaleFemaleGraph()
        {
            PatientCustomModel oPatientCustomModel = new PatientCustomModel();
            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                int Count = oPHRMSDBContext.Users.Count();
                var res = oPHRMSDBContext.PersonalInformation.ToList();
                oPatientCustomModel.MaleCount = res.Where(s => s.Gender == "M").Count();
                oPatientCustomModel.FeMaleCount = res.Where(s => s.Gender == "F").Count();
                oPatientCustomModel.Count = Count.ToString();
                //  oPatientCustomModel.UnSpecifiedCount = res.Where(s => s.Gender == "F").Count().ToString();
                oPatientCustomModel.OthersCount = (Count - res.Where(s => s.Gender == "M").Count() - res.Where(s => s.Gender == "F").Count());
                return PartialView("_PatientMaleFemaleGraph", oPatientCustomModel);

            }
            catch (Exception)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_PatientMaleFemaleGraph", new PatientCustomModel());
        }

        public ActionResult DoctorRegionGraph()
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                List<PatientCustomModel> res;
                if (oCustomPrincipalSerializeModel.IsSuperAdmin)
                    res = oPHRMSDBContext.Places_of_Practice.Where(s => s.Doctor.IsApproved == 2).GroupBy(s => s.state).Select(s => new PatientCustomModel { StateCount = s.Count(), StateName = s.FirstOrDefault().States.Name }).ToList();
                else
                    res = oPHRMSDBContext.Places_of_Practice.Where(s => s.Doctor.IsApproved == 2 && s.Doctor.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId).GroupBy(s => s.state).Select(s => new PatientCustomModel { StateCount = s.Count(), StateName = s.FirstOrDefault().States.Name }).ToList();



                return PartialView("_DoctorRegionGraph", res);

            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_DoctorRegionGraph", new List<PatientCustomModel>());
        }
        public ActionResult PatientRegionGraph()
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {

                var oPersonalInfo = oPHRMSDBContext.PersonalInformation.Where(s => s.State != null && s.State > 0 && s.State < 37).ToList();
                var oPersonalInformation = oPersonalInfo.GroupBy(s => s.State).ToList();
                var res = oPersonalInformation.Select(s => new PatientCustomModel { OthersCount = s.Where(q => q.Gender.Contains("U")).Count(), FeMaleCount = s.Where(q => q.Gender.Contains("F")).Count(), MaleCount = s.Where(q => q.Gender.Contains("M")).Count(), StateCount = s.Count(), StateId = oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().StateId, StateName = s.Key == 0 ? "Not Specified" : oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().Name }).ToList();
                res.Add(new PatientCustomModel { StateCount = res.Sum(s => s.OthersCount) + oPHRMSDBContext.Users.Count() - oPersonalInfo.Count(), StateName = "Not Specified" });
                return PartialView("_PatientRegionGraph", res);

            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_PatientRegionGraph", new List<PatientCustomModel>());
        }
        public ActionResult EMRPatientRegionGraph()
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);

                var res = oPHRMSDBContext.DocPatientDetails    // your starting point - table in the "from" statement
  .Join(oPHRMSDBContext.Doctor, // the source table of the inner join
     post => post.DocId,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
     meta => meta.docid,   // Select the foreign key (the second part of the "on" clause)
     (post, meta) => new { Post = post, Meta = meta }) // selection
  .Where(postAndMeta => postAndMeta.Meta.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId);

                var oPersonalInfo = res.Where(s => s.Post.State != null && s.Post.State > 0 && s.Post.State < 37).ToList();
                var oPersonalInformation = oPersonalInfo.GroupBy(s => s.Post.State).ToList();
                var result = oPersonalInformation.Select(s => new PatientCustomModel { OthersCount = s.Where(q => q.Post.Gender.Contains("U")).Count(), FeMaleCount = s.Where(q => q.Post.Gender.Contains("F")).Count(), MaleCount = s.Where(q => q.Post.Gender.Contains("M")).Count(), StateCount = s.Count(), StateId = oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().StateId, StateName = s.Key == 0 ? "Not Specified" : oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().Name }).ToList();
                //     result.Add(new PatientCustomModel { StateCount = res.Sum(s => s.Post.OthersCount) + oPHRMSDBContext.Users.Count() - oPersonalInfo.Count(), StateName = "Not Specified" });
                return PartialView("_EMRPatientRegionGraph", result);

            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_EMRPatientRegionGraph", new List<PatientCustomModel>());
        }

        public ActionResult DoctorMapRepresentation()
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                //   DateTime Startdate = DateTime.Now.Date;
                // DateTime Nextdate = DateTime.Now.Date;
                // States oStates = oPHRMSDBContext.States.Where(s => s.StateId == Id).FirstOrDefault();
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["UserData"]);
                DateTime date = DateTime.Now;
                DateTime mondayOfLastWeek = date.AddDays(-(int)date.DayOfWeek - 6);
                List<Doctor> oDoctor;
                if (oCustomPrincipalSerializeModel.IsSuperAdmin)
                    oDoctor = oPHRMSDBContext.Doctor.Where(s => s.ApprovedDate != null && s.IsApproved == 2 && s.Places_of_Practice.Count > 0).ToList();
                else
                    oDoctor = oPHRMSDBContext.Doctor.Where(s => s.ApprovedDate != null && s.IsApproved == 2 && s.Places_of_Practice.Count > 0 && s.MedicalCollegeId == oCustomPrincipalSerializeModel.MedicalCollegeId).ToList();

                var Result = new WeekModel
                {
                    Jan = oDoctor.Where(s => s.ApprovedDate.Value.Month == 1 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    Feb = oDoctor.Where(s => s.ApprovedDate.Value.Month == 2 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    March = oDoctor.Where(s => s.ApprovedDate.Value.Month == 3 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    April = oDoctor.Where(s => s.ApprovedDate.Value.Month == 4 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    May = oDoctor.Where(s => s.ApprovedDate.Value.Month == 5 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    June = oDoctor.Where(s => s.ApprovedDate.Value.Month == 6 && s.ApprovedDate.Value.Year == date.Year).Count(),

                    July = oDoctor.Where(s => s.ApprovedDate.Value.Month == 7 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    Aug = oDoctor.Where(s => s.ApprovedDate.Value.Month == 8 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    Sep = oDoctor.Where(s => s.ApprovedDate.Value.Month == 9 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    Oct = oDoctor.Where(s => s.ApprovedDate.Value.Month == 10 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    Nov = oDoctor.Where(s => s.ApprovedDate.Value.Month == 11 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    Dec = oDoctor.Where(s => s.ApprovedDate.Value.Month == 12 && s.ApprovedDate.Value.Year == date.Year).Count(),
                    // Sunday = oDoctor.Where(s => s.ApprovedDate.Value.Date == mondayOfLastWeek.Date.AddDays(6)).Count()

                };

                ViewBag.Data = Result;

                var oPersonalInfo = oPHRMSDBContext.Places_of_Practice.Where(s => s.state > 0 && s.Doctor.IsApproved == 2).ToList();
                var oPersonalInformation = oPersonalInfo.GroupBy(s => s.state).ToList();
                var res = oPersonalInformation.Select(s => new DoctorCustomModel
                {
                    OthersCount = s.Where(q => q.Doctor.Gender.Contains("U")).Count() + s.Where(q => q.Doctor.Gender == "").Count() + s.Where(q => q.Doctor.Gender == null).Count(),//oDoctor.Where(q => q.Gender.Contains("U")&&q.Places_of_Practice.Where(w=>w.state==s.Key)).Count(),
                    FeMaleCount = s.Where(q => q.Doctor.Gender.Contains("F")).Count(),
                    MaleCount = s.Where(q => q.Doctor.Gender.Contains("M")).Count(),
                    StateCount = s.Count(),
                    StateId = oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().StateId,
                    StateName = s.Key == 0 ? "Not Specified" : oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().Name
                }).ToList();
                if (res.Count > 0)
                {
                    res[0].Total = oDoctor.Count();
                    res[0].OthersCount = res.Sum(s => s.OthersCount) + (oDoctor.Count() - oPersonalInfo.Count());
                }
                foreach (var Item in oPHRMSDBContext.States.ToList())
                {
                    if (res.Where(s => s.StateId == Item.StateId).FirstOrDefault() == null)
                    {
                        res.Add(new DoctorCustomModel { OthersCount = 0, MaleCount = 0, FeMaleCount = 0, StateCount = 0, StateId = Item.StateId, StateName = Item.Name });
                    }
                }

                oDoctor = null;
                return PartialView("_DoctorMapRepresentation", res);

            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_MapRepresentation", new List<PatientCustomModel>());
        }
        public ActionResult MapRepresentation()
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                //   DateTime Startdate = DateTime.Now.Date;
                // DateTime Nextdate = DateTime.Now.Date;
                // States oStates = oPHRMSDBContext.States.Where(s => s.StateId == Id).FirstOrDefault();
                DateTime date = DateTime.Now;
                DateTime mondayOfLastWeek = date.AddDays(-(int)date.DayOfWeek - 6);
                List<Users> oUsers = oPHRMSDBContext.Users.ToList();
                var Result = new WeekModel
                {
                    Jan = oUsers.Where(s => s.CreatedDate.Month == 1 && s.CreatedDate.Year == date.Year).Count(),
                    Feb = oUsers.Where(s => s.CreatedDate.Month == 2 && s.CreatedDate.Year == date.Year).Count(),
                    March = oUsers.Where(s => s.CreatedDate.Month == 3 && s.CreatedDate.Year == date.Year).Count(),
                    April = oUsers.Where(s => s.CreatedDate.Month == 4 && s.CreatedDate.Year == date.Year).Count(),
                    May = oUsers.Where(s => s.CreatedDate.Month == 5 && s.CreatedDate.Year == date.Year).Count(),
                    June = oUsers.Where(s => s.CreatedDate.Month == 6 && s.CreatedDate.Year == date.Year).Count(),

                    July = oUsers.Where(s => s.CreatedDate.Month == 7 && s.CreatedDate.Year == date.Year).Count(),
                    Aug = oUsers.Where(s => s.CreatedDate.Month == 8 && s.CreatedDate.Year == date.Year).Count(),
                    Sep = oUsers.Where(s => s.CreatedDate.Month == 9 && s.CreatedDate.Year == date.Year).Count(),
                    Oct = oUsers.Where(s => s.CreatedDate.Month == 10 && s.CreatedDate.Year == date.Year).Count(),
                    Nov = oUsers.Where(s => s.CreatedDate.Month == 11 && s.CreatedDate.Year == date.Year).Count(),
                    Dec = oUsers.Where(s => s.CreatedDate.Month == 12 && s.CreatedDate.Year == date.Year).Count(),

                };

                ViewBag.Data = Result;

                var oPersonalInfo = oPHRMSDBContext.PersonalInformation.Where(s => s.State > 0 && s.State < 40).ToList();
                var oPersonalInformation = oPersonalInfo.GroupBy(s => s.State).ToList();
                var res = oPersonalInformation.Select(s => new PatientCustomModel
                {
                    OthersCount = s.Where(q => q.Gender.Contains("U")).Count(),
                    FeMaleCount = s.Where(q => q.Gender.Contains("F")).Count(),
                    MaleCount = s.Where(q => q.Gender.Contains("M")).Count(),
                    StateCount = s.Count(),
                    StateId = oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().StateId,
                    StateName = s.Key == 0 ? "Not Specified" : oPHRMSDBContext.States.Where(q => s.Key > 0 && q.Id == s.Key).FirstOrDefault().Name
                }).ToList();
                if (res.Count > 0)
                {
                    res[0].Total = oUsers.Count();
                    res[0].OthersCount = res.Sum(s => s.OthersCount) + (oUsers.Count() - oPersonalInfo.Count());
                }
                foreach (var Item in oPHRMSDBContext.States.ToList())
                {
                    if (res.Where(s => s.StateId == Item.StateId).FirstOrDefault() == null)
                    {
                        res.Add(new PatientCustomModel { OthersCount = 0, MaleCount = 0, FeMaleCount = 0, StateCount = 0, StateId = Item.StateId, StateName = Item.Name });
                    }
                }

                oUsers = null;
                return PartialView("_MapRepresentation", res);

            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return PartialView("_MapRepresentation", new List<PatientCustomModel>());
        }
        public JsonResult GetData(string Id)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                States oStates = oPHRMSDBContext.States.Where(s => s.StateId == Id).FirstOrDefault();
                var oPersonalInfo = oPHRMSDBContext.PersonalInformation.Where(s => s.State == oStates.Id).ToList();
                PatientCustomModel res = new PatientCustomModel { OthersCount = oPersonalInfo.Where(q => q.Gender.Contains("U")).Count(), FeMaleCount = oPersonalInfo.Where(q => q.Gender.Contains("F")).Count(), MaleCount = oPersonalInfo.Where(q => q.Gender.Contains("M")).Count(), StateCount = oPersonalInfo.Count(), StateId = oStates.StateId, StateName = oStates.Name };
                oStates = null;
                oPersonalInfo = null;
                oPHRMSDBContext = null;
                DateTime date = DateTime.Now;


                return Json(res);
            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return Json(null);
        }
        public JsonResult GetDoctorData(string Id)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            try
            {
                States oStates = oPHRMSDBContext.States.Where(s => s.StateId == Id).FirstOrDefault();
                int ID = oStates.Id;
                var oPersonalInfo = oPHRMSDBContext.Doctor.Where(s => s.IsApproved == 2 && s.Places_of_Practice.Where(q => q.state == ID).FirstOrDefault() != null).ToList();
                DoctorCustomModel res = new DoctorCustomModel
                {
                    OthersCount = oPersonalInfo.Where(q => q.Gender.Contains("U")).Count(),
                    FeMaleCount = oPersonalInfo.Where(q => q.Gender.Contains("F")).Count(),
                    MaleCount = oPersonalInfo.Where(q => q.Gender.Contains("M")).Count(),
                    StateCount = oPersonalInfo.Count(),
                    StateId = oStates.StateId,
                    StateName = oStates.Name
                };
                oStates = null;
                oPersonalInfo = null;
                oPHRMSDBContext = null;
                DateTime date = DateTime.Now;


                return Json(res);
            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return Json(null);
        }
        public JsonResult GetPatientLastWeekDetail(string Id)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            DateTime date = DateTime.Now;
            DateTime mondayOfLastWeek = date.AddDays(-(int)date.DayOfWeek - 6);
            try
            {


                DateTime Startdate = DateTime.Now.Date;
                DateTime Nextdate = DateTime.Now.Date;
                States oStates = oPHRMSDBContext.States.Where(s => s.StateId == Id).FirstOrDefault();
                var oUsers = (from s in oPHRMSDBContext.Users
                              join w in oPHRMSDBContext.PersonalInformation.Where(s => s.State == oStates.Id) on s.UserId equals w.UserId
                              select new { CreatedDate = s.CreatedDate }
                           ).ToList();


                var Result = new
                {
                    Jan = oUsers.Where(s => s.CreatedDate.Month == 1 && s.CreatedDate.Year == date.Year).Count(),
                    Feb = oUsers.Where(s => s.CreatedDate.Month == 2 && s.CreatedDate.Year == date.Year).Count(),
                    March = oUsers.Where(s => s.CreatedDate.Month == 3 && s.CreatedDate.Year == date.Year).Count(),
                    April = oUsers.Where(s => s.CreatedDate.Month == 4 && s.CreatedDate.Year == date.Year).Count(),
                    May = oUsers.Where(s => s.CreatedDate.Month == 5 && s.CreatedDate.Year == date.Year).Count(),
                    June = oUsers.Where(s => s.CreatedDate.Month == 6 && s.CreatedDate.Year == date.Year).Count(),

                    July = oUsers.Where(s => s.CreatedDate.Month == 7 && s.CreatedDate.Year == date.Year).Count(),
                    Aug = oUsers.Where(s => s.CreatedDate.Month == 8 && s.CreatedDate.Year == date.Year).Count(),
                    Sep = oUsers.Where(s => s.CreatedDate.Month == 9 && s.CreatedDate.Year == date.Year).Count(),
                    Oct = oUsers.Where(s => s.CreatedDate.Month == 10 && s.CreatedDate.Year == date.Year).Count(),
                    Nov = oUsers.Where(s => s.CreatedDate.Month == 11 && s.CreatedDate.Year == date.Year).Count(),
                    Dec = oUsers.Where(s => s.CreatedDate.Month == 12 && s.CreatedDate.Year == date.Year).Count(),

                };
                return Json(Result);
            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return Json(null);
        }



        public JsonResult GetDoctorLastWeekDetail(string Id)
        {

            PHRMSDBContext oPHRMSDBContext = new PHRMSDBContext();
            DateTime date = DateTime.Now;
            DateTime mondayOfLastWeek = date.AddDays(-(int)date.DayOfWeek - 6);
            try
            {


                DateTime Startdate = DateTime.Now.Date;
                DateTime Nextdate = DateTime.Now.Date;
                States oStates = oPHRMSDBContext.States.Where(s => s.StateId == Id).FirstOrDefault();
                var res = (from s in oPHRMSDBContext.Doctor.Where(s => s.IsApproved == 2)
                           join w in oPHRMSDBContext.Places_of_Practice.Where(q => q.state == oStates.Id) on s.docid equals w.docid
                           where s.ApprovedDate != null
                           select new { CreatedDate = s.ApprovedDate.Value }
                           ).ToList();


                var Result = new
                {
                    Jan = res.Where(s => s.CreatedDate.Month == 1 && s.CreatedDate.Year == date.Year).Count(),
                    Feb = res.Where(s => s.CreatedDate.Month == 2 && s.CreatedDate.Year == date.Year).Count(),
                    March = res.Where(s => s.CreatedDate.Month == 3 && s.CreatedDate.Year == date.Year).Count(),
                    April = res.Where(s => s.CreatedDate.Month == 4 && s.CreatedDate.Year == date.Year).Count(),
                    May = res.Where(s => s.CreatedDate.Month == 5 && s.CreatedDate.Year == date.Year).Count(),
                    June = res.Where(s => s.CreatedDate.Month == 6 && s.CreatedDate.Year == date.Year).Count(),

                    July = res.Where(s => s.CreatedDate.Month == 7 && s.CreatedDate.Year == date.Year).Count(),
                    Aug = res.Where(s => s.CreatedDate.Month == 8 && s.CreatedDate.Year == date.Year).Count(),
                    Sep = res.Where(s => s.CreatedDate.Month == 9 && s.CreatedDate.Year == date.Year).Count(),
                    Oct = res.Where(s => s.CreatedDate.Month == 10 && s.CreatedDate.Year == date.Year).Count(),
                    Nov = res.Where(s => s.CreatedDate.Month == 11 && s.CreatedDate.Year == date.Year).Count(),
                    Dec = res.Where(s => s.CreatedDate.Month == 12 && s.CreatedDate.Year == date.Year).Count(),

                };
                return Json(Result);
            }
            catch (Exception ex)
            {


            }
            oPHRMSDBContext.Dispose();
            oPHRMSDBContext = null;
            return Json(null);
        }
    }
}