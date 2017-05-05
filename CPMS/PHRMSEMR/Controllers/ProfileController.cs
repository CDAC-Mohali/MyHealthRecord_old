using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMRLib.DAL;
using AutoMapper;
using PHRMSEMR.Models;
using EMRLib.DataModels;
using EMRViewModels;
using static PHRMSEMR.FilterConfig;
using System.Threading.Tasks;
using System.IO;
using System.Web.Hosting;


namespace PHRMSEMR.Controllers
{
    [Error]
    [AuthorizationFilter]
    public class ProfileController : Controller
    {
        IEMRRepository _repo;
        CommonComponent oComponent = new CommonComponent();


        public ProfileController()
        {
            _repo = new EMRRepository();
        }
        // GET: Profile
        public ActionResult Index()
        {
            EMRDBContext db = new EMRDBContext();
            CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["DocSessionData"]);
            DoctorViewModel oGet = new DoctorViewModel();
            try
            {

                //To calculate the total patient count against the doctor
                int totalPatient = db.DocPatientDetails.Where(s => s.DocId == oCustomPrincipalSerializeModel.DocId).Count();
                ViewBag.PatientCount = totalPatient;
                ViewBag.StatesList = oComponent.GetStatesList();
                ViewBag.GenderList = oComponent.GetGenderList();
                ViewBag.SpecialityList = oComponent.GetSpecialityList();
                //To populate Doctor detail in grid
                Doctor doctor = db.Doctor.Find(oCustomPrincipalSerializeModel.DocId);
                Mapper.CreateMap<Doctor, DoctorViewModel>();
                oGet = Mapper.Map<Doctor, DoctorViewModel>(doctor);
                var place_view_model = db.Places_of_Practice.Where(s => s.docid == doctor.docid).FirstOrDefault();
                Mapper.CreateMap<Places_of_Practice, PlaceViewModel>();
                if (place_view_model != null)
                {
                    oGet.PlaceViewModel = Mapper.Map<Places_of_Practice, PlaceViewModel>(place_view_model);
                }


                var EMRDocFilePath = db.EMRFiles.Where(s => s.RecId == doctor.docid).Select(x=>x.FileName).FirstOrDefault();             
                if (EMRDocFilePath != null)
                {
                    oGet.DocFile = EMRDocFilePath;
                }

            }
            catch (Exception ex)
            {
                //ex.ToString();
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Profile");
            }
            db.Dispose();
            db = null;
            return View(oGet);

        }



        [HttpPost]
        public ActionResult UpdateProfile(DoctorViewModel oUpdateTask)
        {
            EMRDBContext db = new EMRDBContext();
            try
            {
                CustomPrincipalSerializeModel oCustomPrincipalSerializeModel = (CustomPrincipalSerializeModel)(Session["DocSessionData"]);
                Doctor oCreate = db.Doctor.Where(s => s.docid == oCustomPrincipalSerializeModel.DocId).FirstOrDefault();
                if (oCreate != null)
                {
                    oCreate.name = oUpdateTask.name;
                    oCreate.date_of_birth = oUpdateTask.date_of_birth;
                    //oCreate.phone_number = oUpdateTask.phone_number;
                    //oCreate.email = oUpdateTask.email;
                    oCreate.qualification_set = oUpdateTask.qualification_set;
                    oCreate.Speciality = oUpdateTask.Speciality;
                    oCreate.Gender = oUpdateTask.Gender.ToString();
                    oCreate.AadhaarNo = oUpdateTask.AadhaarNo;
                    oCreate.LastName = oUpdateTask.LastName;
                    oCreate.OtherSpeciality = (oUpdateTask.OtherSpeciality) == null ? "" : oUpdateTask.OtherSpeciality;
                    db.SaveChanges();

                    Places_of_Practice oPlaceOfPractice = db.Places_of_Practice.Where(s => s.docid == oCustomPrincipalSerializeModel.DocId).FirstOrDefault();
                    if (oPlaceOfPractice != null)
                    {
                        oPlaceOfPractice.license_number = oUpdateTask.PlaceViewModel.license_number;
                        oPlaceOfPractice.name = oUpdateTask.PlaceViewModel.name;
                        oPlaceOfPractice.AddressLine1 = oUpdateTask.PlaceViewModel.AddressLine1;
                        oPlaceOfPractice.AddressLine2 = oUpdateTask.PlaceViewModel.AddressLine2;
                        oPlaceOfPractice.city = oUpdateTask.PlaceViewModel.city;
                        oPlaceOfPractice.state = oUpdateTask.PlaceViewModel.state;
                        oPlaceOfPractice.pincode = oUpdateTask.PlaceViewModel.pincode;
                        db.SaveChanges();
                    }
                    oCreate = null;
                }
            }
            catch (Exception ex)
            {
               Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Profile");
            }
            db.Dispose();
            db = null;
            return RedirectToAction("Index");

        }
        // GET: /<controller>/
        public ActionResult ChangePicture()
        {
            try { 
            return View();
              }
            catch (Exception ex)
            {
                //ex.ToString();
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Profile");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePicture(string imgpath)
        {
            try
            {
                bool result = _repo.SetProfilePic(imgpath, ((CustomPrincipalSerializeModel)Session["DocSessionData"]).DocId);
                if (result)
                {
                    //set image path in the session
                    ((CustomPrincipalSerializeModel)Session["DocSessionData"]).ImgPath = imgpath;
                }
                return View();
            }
            catch (Exception ex)
            {
                //ex.ToString();
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Profile");
            }
            return View();
         }

        [HttpPost]
        public async Task<JsonResult> UploadFile()
        {
            string str = "";
            int stat = 500;
            try
            {
                var body = HttpContext.Request.InputStream;
                var fileName = HttpContext.Request.Headers["X_FILENAME"].ToString().ToLower();
                string extension = Path.GetExtension(fileName);
                //var extension = fileName.EndsWith("png") ? "png" : (fileName.EndsWith("jpg") || fileName.EndsWith("jpeg") ? "jpeg" : "gif");
                //using (FileStream fs = System.IO.File.Create(string.Format(@"D:\{0}.{1}", Guid.NewGuid(), extension)))
                //var path = Path.Combine(Microsoft.Net.Http.Server.MapPath("~/App_Data/Images"), fileName);
                str = string.Format(@"\Images\ProfilePics\Custom\{0}{1}", Guid.NewGuid(), extension);

                using (FileStream fs = System.IO.File.Create(HostingEnvironment.ApplicationPhysicalPath + str))
                {
                    await body.CopyToAsync(fs);
                }
                str = str.Replace("\\", "/");
                stat = 200;
            }
             catch (Exception ex)
            {
                str = ex.Message;
                Common.CreateLog(Common.ExecptionMessage(ex), MessageType.Error, "Profile");
            }
          
            var model = new
            {
                status = stat,
                path = str
            };
            return Json(model);
        }
    }
}