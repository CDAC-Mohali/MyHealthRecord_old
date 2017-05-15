using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PHRMSAdmin.Models
{
    public class NotificationModel
    {
        public long NotificationId { get; set; }
        public Guid ReferenceId { get; set; }
        public int UserTypeId { get; set; }
        public int MessageTypeId { get; set; }

        [Required]
        public string Message { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string strCreatedDate { get; set; }
        public string userName { get; set; }
    }
}