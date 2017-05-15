using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHRMSAdmin.Library
{
    public class DoctorApprovalTrack
    {
        [Key]
        [Required]
        public long Id { get; set; }
        public Guid docId { get; set; }
        public long userId { get; set; }
        public string remarks { get; set; }
        public int status { get; set; }
        public DateTime approvalDateTime { get; set; }
    }
}
