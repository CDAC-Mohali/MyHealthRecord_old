
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHRMSAdmin.Models
{
    [Serializable()]
    public class CustomPrincipalSerializeModel
    {
        public long? MedicalCollegeId;

        public long AdminUserId { get; set; }
        public string FirstName { get; set; }
        public string EmailId { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string LastName { get; set; }

    }
}