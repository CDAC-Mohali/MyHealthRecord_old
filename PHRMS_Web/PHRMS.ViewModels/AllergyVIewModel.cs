using System;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.ViewModels
{
    public class AllergyViewModel
    {
        public AllergyViewModel()
        {
            SourceId = Source.WebApp;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int AllergyType { get; set; }
        public string AllergyName { get; set; }
        public bool Still_Have { get; set; }
        public int Severity { get; set; }
        public string strSeverity { get; set; }
        public int DurationId { get; set; }
        public string strDuration { get; set; }
        //public DateTime StartDate { get; set; }
        //public string strStartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public string strEndDate { get; set; }
       
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string strStill_Have { get; set; }
        //public string reactDes { get; set; }
        public int sno { get; set; }
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    public class AllergyMasterViewModel
    {
        public int Id { get; set; }
        public string AllergyName { get; set; }
        public string DescriptionId { get; set; }
        public string ConceptId { get; set; }
    }

    public class AllergySeverityViewModel
    {
        public int Id { get; set; }
        public string SCTID { get; set; }
        public string Severity { get; set; }
    }

    public class AllergyDurationViewModel
    {
        public int Id { get; set; }
        public string Duration { get; set; }
    }
}
