using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PHRMS.ViewModels
{
    public class LabTestMasterViewModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }

    }

    public class LabTestViewModel
    {
        public LabTestViewModel()
        {
            SourceId = Source.WebApp;
        }
        public Guid Id { get; set; }
        public int sno { get; set; }
        public Guid UserId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime PerformedDate { get; set; }
        public string strPerformedDate { get; set; }
        public string Result { get; set; }
        public string Unit { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool DeleteFlag { get; set; }
        public List<string> lstFiles { get; set; }
        public List<FileViewModel> lstFileModels { get; set; }
        public int SourceId { get; set; }
    }
    //public class LabTestPrescribedViewModel
    //{
    //    public Guid Id { get; set; }
    //    public Guid UserId { get; set; }
    //    public int TestId { get; set; }
    //    public string TestName { get; set; }
    //    public string Comments { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //    public bool DeleteFlag { get; set; }
    //}

    //public class LabTestResultViewModel
    //{
    //    public Guid Id { get; set; }
    //    public int sno { get; set; }
    //    public Guid UserId { get; set; }
    //    public string TestName { get; set; }
    //    public DateTime PerformedDate { get; set; }
    //    public string strPerformedDate { get; set; }
    //    public string Result { get; set; }
    //    public string Unit { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //    public bool DeleteFlag { get; set; }
    //    public Guid PresTestId { get; set; }
    //}

}
