using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PHRMSAdmin.Library
{
    public class AdminUsers
    {
        [Key]
        public int AdminUserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
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
        public string PhoneNumber { get; set; }
        public long ?MedicalCollegeId { get; set; }
        //public virtual List<MedicalColleges> MedicalColleges { get; set; }
        public virtual List<UserRoleMapping> UserRoleMapping { get; set; }
    }

    public class UserRoleMapping
    {
        [Key]
        public long UserRoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public long RoleId { get; set; }
        [Required]
        [Display(Name = "User")]
        public int AdminUserId { get; set; }
       


        public virtual AdminUsers AdminUsers { get; set; }
        public virtual Role Role { get; set; }

    }

    public class Role
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

    public class Tasks
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
    public class RoleTaskMapping
    {
        [Key]
        public long RoleTaskId { get; set; }
      
        public long RoleId { get; set; }
      
        public long TaskId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Tasks Task { get; set; }
    }

    public class ContactUs
    {
        [Key]
        public long ContactUsId { get; set; }
        [MaxLength(20)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string LastName { get; set; }
        [MaxLength(20)]
        public string City { get; set; }
        [MaxLength(20)]
        public string MobileNo { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(2000)]
        public string Message { get; set; }
        public int State { get; set; }
        public int Status { get; set; }
    }
}
