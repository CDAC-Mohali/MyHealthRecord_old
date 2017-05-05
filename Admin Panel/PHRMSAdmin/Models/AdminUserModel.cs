
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace PHRMSAdmin.Models
{
    public class AdminUsersModel
    {
        [Key]
        public int AdminUserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public bool Active { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public bool IsSuperAdmin { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Address Required")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string PhoneNumber { get; set; }
        
        public long RoleId { get; set; }
        public long MedicalCollegeId { get; set; }
        public List<MedicalCollegesViewModel> MedicalCollegesViewModel { get; set; }
        public List<UserRoleMappingModel> UserRoleMappingModel { get; set; }
    }

    public class UserRoleMappingModel
    {
        [Key]
        public long UserRoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public long RoleId { get; set; }
        [Required]
        [Display(Name = "User")]
        public long AdminUserId { get; set; }
      


        public AdminUsersModel AdminUsersModel { get; set; }
        public RoleModel RoleModel { get; set; }

    }

    public class RoleModel
    {
        [Key]
        public long RoleId { get; set; }
        [Required(ErrorMessage = "Role field is required")]
      
        public string RoleName { get; set; }
      
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsCreate { get; set; }
        public bool IsView { get; set; }
    }

    public class TaskModel
    {
        [Key]
        public long TaskId { get; set; }
        [Required(ErrorMessage = "User Name field is required")]
        [Display(Name = "Module")]
        public string TaskName { get; set; }

        // [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "The field Value must be a number.")]
        [Required]
        [Display(Name = "Module Position")]

        public int TaskPosition { get; set; }

        [Display(Name = "Remarks")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Heading Name field is required")]
        [Display(Name = "Heading")]
        public string HeadingName { get; set; }

        [Required(ErrorMessage = "Heading Position field is required")]
        // [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "The field Value must be a number.")]
        [Display(Name = "Heading Position")]
        public int HeadingPosition { get; set; }

        [Required(ErrorMessage = "Module Url field is required")]
        [Display(Name = "Module Url")]
        public string TaskUrl { get; set; }

        [Required(ErrorMessage = "ModuleModule Active Text field is required")]
        [Display(Name = "Module Active Text")]
        public string TaskActiveText { get; set; }

        [Display(Name = "Image CSS")]
        public string ImageCSS { get; set; }

        [Display(Name = "Is Sub Module")]
        public bool IsSubTask { get; set; }

        //[RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "The field Value must be a number.")]
        [Display(Name = "Sub Module Level")]

        public int SubTaskLevel { get; set; }

        [Display(Name = "Sub Module Heading")]

        public string SubTaskHeading { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
    public class RoleTaskMappingModel
    {
        public long RoleTaskId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public long RoleId { get; set; }
        [Required]
        [Display(Name = "User")]
        public long TaskId { get; set; }
        public RoleModel RoleModel { get; set; }
        public TaskModel TasksModel { get; set; }
    }
}