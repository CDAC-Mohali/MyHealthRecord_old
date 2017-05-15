using System;
using System.Collections.Generic;


namespace PHRMS.ViewModels
{

    public class EprescriptionViewModel
    {
        public EprescriptionViewModel()
        {
            SourceId = Source.WebApp;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int sno { get; set; }
        public string DocName { get; set; }       
        public string ClinicName { get; set; }        
        public string DocAddress { get; set; }       
        public string DocPhone { get; set; }       
        public string Prescription { get; set; }       
        public DateTime PresDate { get; set; }
        public string strPresDate { get; set; }
        public string FileName { get; set; }        
        public bool DeleteFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
        public string PhysicalExamination { get; set; }
        public string ProblemDiagnosis { get; set; }
        public string OtherAdvice { get; set; }
    }
}
