using System;
using System.ComponentModel.DataAnnotations;



namespace PHRMS.ViewModels
{
    public class BloodGroupsModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class StatesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

   

    public class ContactTypeModel
    {
        public int Id { get; set; }
        public string MedContType { get; set; }

    }

    public class RelationshipModel
    {
        public int Id { get; set; }
        public string Relation { get; set; }

    }

    public class DisabilityTypesModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PinCodesViewModel
    {
        public int Id { get; set; }
        public string PostOfficeName { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
    }

    public class FileViewModel
    {
        public int FilePathId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public FileType FileType { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public Guid UserId { get; set; }
        public Guid RecId { get; set; }

        public FileViewModel()
        {
            RecId = Guid.Empty;
            DeleteFlag = false;
            UserId = Guid.Empty;
            CreatedDate = DateTime.Now;
        }
    }

    public enum FileType
    {
        ProfilePic = 1, DisablityCert, ePrescription, LabReport, Medication, Procedure, Feedback
    }

    public enum FileStatus
    {
        Pushed = 1, Saved, Deleted, Removed
    }

    public sealed class Source
    {
        public const int WebApp = 1, EMR = 2, MobApp = 3;
    }

    public sealed class ReportParameters
    {
        public const int PersonalInformation = 1, EmergencyInformation = 2, EmployerInformation = 3, InsuranceInformation = 4,
        LegalInformation = 5, Preferences = 6, Allergies = 7, Immunizations = 8, Medications = 9, Procedures = 10, Tests = 11, Problems = 12, Activities = 13,
        BP = 14, Glucose = 15, Weight = 16;
    }

    public sealed class FileDirPaths
    {
        public static readonly string DyCertPath = @"Images/DisabilityCert/";
        public static readonly string LabReportPath = @"Images/DisabilityCert/";
    }

    public class MasterViewModels
    {
        public string str { get; set; }
        public int resType { get; set; }
    }

    public sealed class MasterSearchType
    {
        public static readonly int Categorial = 1;
        public static readonly int Complete = 2;
    }



    public class ContactusViewModel
    {

        public long ContactUsId { get; set; }



        //[Required]
        //[Display(Name = "Name")]
        //public string Name { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessage = "No Special Charatcers allowed")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessage = "No Special Charatcers allowed")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Mobile No.")]
        //[StringLength(15, ErrorMessage = "Mobile Number length Should be less than 15")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number contain only numbers and must be of 10 digits.")]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required]
        [Display(Name = "E-Mail")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public string City { get; set; }
        //public string State { get; set; }
        public int State { get; set; }
        [Display(Name = "State/UT")]
        public int Status { get; set; }
    }
    }
