using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PHRMSAdmin.Models
{


    public class HealthConditionMasterModel
    {
        public int Id { get; set; }
        public string HealthCondition { get; set; }
    }

    public class HealthConditionModel
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int sno { get; set; }
        public int ConditionType { get; set; }
        public string HealthCondition { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public string strDiagnosisDate { get; set; }
        public DateTime ServiceDate { get; set; }
        public string strServiceDate { get; set; }
        public string Provider { get; set; }
        public string Notes { get; set; }
        public bool StillHaveCondition { get; set; }
        public string strStillHaveCondition { get; set; }
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int SourceId { get; set; }
    }
}