using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Mvc;

namespace PHRMS.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessage = "No Special Charatcers allowed")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z0-9]+", ErrorMessage = "No Special Charatcers allowed")]
        public string LastName { get; set; }

        [RegularExpression(@"^[0-9]{12,12}$", ErrorMessage = "Not a valid Aadhaar Number.")]
        [Display(Name = "Aadhaar Number")]
        public string AadhaarNo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "CUG")]
        [StringLength(5, ErrorMessage = "CUG must contain 5 characters")]
        public string CUG { get; set; }

        [Required]
        [Display(Name = "Mobile No.")]
        //[StringLength(15, ErrorMessage = "Mobile Number length Should be less than 15")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number contain only numbers and must be of 10 digits.")]
        [Remote("DoesMobileExist", "Account", HttpMethod = "POST", ErrorMessage = "This Mobile No. is already registered.")]
        public string MobileNo { get; set; }

        //[Display(Name = "Aadhaar No.")]
        //public string AadhaarNo { get; set; }

        [Required]
        [Display(Name = "Email")]
        [Remote("DoesEmailExist", "Account", HttpMethod = "POST", ErrorMessage = "This Email Address is already registered.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        public System.Guid Id { get; set; }
        public string OTP { get; set; }
        public string status { get; set; }
        public int State { get; set; }
    }

    public class RegProcessCode
    {
        public static string Success = "2";
        public static string ProcessFailure = "-1";
        public static string OtpMisMatch = "-2";
        public static string Initial = "0";
        public static string OtpVerification = "1";
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ForgotPasswordModel
    {
        [Required]
        [Display(Name = "Registered Mobile No. or Email Id")]
        [Remote("DoesEmailOrMobileExist", "Account", HttpMethod = "POST", ErrorMessage = "This User Id is not registered with MyHealthReacord.")]
        public string UserName { get; set; }
        public string Email { get; set; }
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number contain only numbers and must be of 10 digits.")]
        [Remote("DoesMobileExistForOTP", "Account", HttpMethod = "POST", ErrorMessage = "This User Id is not registered with MyHealthReacord.")]
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public System.Guid Id { get; set; }
        public int StatusCode { get; set; }
        [Required]
        [Display(Name = "Mobile No.")]
        [RegularExpression(@"^[0-9]{10,10}$", ErrorMessage = "Mobile Number contain only numbers and must be of 10 digits.")]
        [Remote("DoesMobileExistForOTP", "Account", HttpMethod = "POST", ErrorMessage = "This User Id is not registered with MyHealthReacord.")]
        public string OTPMobileNo { get; set; }
    }

    public class FPProcessStatus
    {
        public static int SysFailure = 0;
        public static int VerificationFailure = -1;
        public static int Success = 1;
    }

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
