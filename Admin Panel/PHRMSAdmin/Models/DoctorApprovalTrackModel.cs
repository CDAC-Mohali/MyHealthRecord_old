using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHRMSAdmin.Models
{
    public class DoctorApprovalTrackModel
    {
        public long Id { get; set; }
        public Guid docId { get; set; }
        public long userId { get; set; }
        public string remarks { get; set; }
        public int status { get; set; }
        public DateTime approvalDateTime { get; set; }
    }
}
 