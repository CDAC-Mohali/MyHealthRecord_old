using System;


namespace PHRMSAdmin.Models
{
    public class AllergyMasterModel
    {
        public int Id { get; set; }
        public string AllergyName { get; set; }
      
    }
    public class AllergyModel
    {
     
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int AllergyType { get; set; }
        public string AllergyName { get; set; }
        public bool Still_Have { get; set; }
        public int Severity { get; set; }
        public string strSeverity { get; set; }
        public int DurationId { get; set; }
        public string strDuration { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string strStill_Have { get; set; }
        public int sno { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }


}