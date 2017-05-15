using System;
using System.ComponentModel.DataAnnotations;

namespace PHRMSAdmin.Library
{
    public class Notification
    {
        [Key]
        [Required]
        public long NotificationId { get; set; }
        public Guid ReferenceId { get; set; }
        public int UserTypeId { get; set; }
        public int MessageTypeId { get; set; }
        public string Message { get; set; }
        public int Status{ get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
