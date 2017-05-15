using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PHRMS.ViewModels
{
    public class ActivitiesViewModel
    {
        public ActivitiesViewModel()
        {
            SourceId = Source.WebApp;
        }
        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string Result { get; set; }
        public string PathName { get; set; }
        public decimal Distance { get; set; }
        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    public class ActivityMasterViewModel
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
    }

    public class BloodGlucoseViewModel
    {
        public BloodGlucoseViewModel()
        {
            SourceId = Source.WebApp;
        }
        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public string ValueType { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    public class BloodPressureAndPulseViewModel
    {
        public BloodPressureAndPulseViewModel()
        {
            SourceId = Source.WebApp;
        }
        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ResSystolic { get; set; }
        public string ResDiastolic { get; set; }
        public string GoalSystolic { get; set; }
        public string GoalDiastolic { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string GoalPulse { get; set; }
        public string ResPulse { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
    }

    public class SleepViewModel
    {
        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
    }

    public class WeightViewModel
    {
        public WeightViewModel()
        {
            SourceId = Source.WebApp;
        }
        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
        public int SourceId { get; set; }
        public float BMI { get; set; }
    }
    public class TemperatureViewModel
    {
        public int sno { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Goal { get; set; }
        public string Result { get; set; }
        public DateTime CollectionDate { get; set; }
        public string strCollectionDate { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [System.ComponentModel.DefaultValue(false)]
        public bool DeleteFlag { get; set; }
    }


    public class ShareReportFeedBackViewModel
    {

        public long ShareReportFeedBackId { get; set; }
        public Guid UserId { get; set; }
        public Guid MedicalContactId { get; set; }
        public string FeedBack { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EPrescriptionId { get; set; }
        public long UserRecordId { get; internal set; }
       
    }
}
