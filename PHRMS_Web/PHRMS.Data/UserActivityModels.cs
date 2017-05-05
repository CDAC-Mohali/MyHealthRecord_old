using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.Data
{
    [Table("UserActivities")]
    public class UserActivities
    {
        [Key]
        public Guid Id { get; set; }
        public int Module { get; set; }
        public int Operation { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid ActivityId { get; set; }
        public Guid UserId { get; set; }
    }
    public class UserShareRecord
    {
        [Key]
        public long UserRecordId { get; set; }
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string strChecks { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ValidUpto { get; set; }
        [NotMapped]
        public string ImagePath { get; set; }

        public virtual Users Users { get; set; }
        [NotMapped]
        public string DocEmail { get; set; }
        [NotMapped]
        public string DocPhone { get; set; }
        public Guid MedicalContactId { get; set; }
        public string Query { get; set; }

    }

    public class ShareReportFeedBack
    {
        [Key]
        public long ShareReportFeedBackId { get; set; }
        public Guid UserId { get; set; }
        public Guid MedicalContactId { get; set; }
        public string FeedBack { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? EPrescriptionId { get; set; }
        public long UserRecordId { get;  set; }
     
    }
    public class ShareReportNotification
    {
        [Key]
        public long NotificationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isViewedByDoctor { get; set; }
        public bool isPrescribedByDoctor { get; set; }
        public bool isNotificationViewed { get; set; }

        //[ForeignKey("UserShareRecord")]
        public long UserRecordId { get; set; }
        public virtual UserShareRecord UserShareRecord { get; set; }

    }


}
